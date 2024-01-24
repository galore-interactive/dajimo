using System.Collections.Generic;
using UnityEngine;
using System;
using GONet;
using System.Linq;

/// <summary>
/// Performs Hit registration with lag compensation.
/// </summary>
public class ServerHitRegistrator
{
    private readonly Queue<ShotConfiguration> _shootersShotConfigurationsBuffer;
    private readonly RaycastShooter _raycastShooter;
    private readonly NetworkController _networkController;

    private readonly HashSet<INetworkEntity> _friendyFireEntities;

    private bool _succesfulShot = false;
    private ShotHitData _lastShotHitData;
    private readonly int _raycastLayerMask;

    private readonly int _shootableLayerMaskIndex;

    private INetworkEntity _lastShooterEntityProcessed;
    public INetworkEntity LastShooterEntityProcessed => _lastShooterEntityProcessed;

    public ServerHitRegistrator(RaycastShooter raycastShooter, NetworkController networkController)
    {
        _shootersShotConfigurationsBuffer = new Queue<ShotConfiguration>();
        _raycastShooter = raycastShooter;
        _networkController = networkController;

        const string GHOST_OBJECTS_LAYER_MASK_NAME = "GhostObjects";
        const string NON_SHOOTABLE_LAYER_MASK_NAME = "NonShootable";
        const string IGNORE_RAYCAST_LAYER_MASK_NAME = "Ignore Raycast";
        _raycastLayerMask = ~(LayerMask.GetMask(GHOST_OBJECTS_LAYER_MASK_NAME) | LayerMask.GetMask(NON_SHOOTABLE_LAYER_MASK_NAME) | LayerMask.GetMask(IGNORE_RAYCAST_LAYER_MASK_NAME));

        const string SHOOTABLE_LAYER_MASK_NAME = "Shootable";
        _shootableLayerMaskIndex = LayerMask.NameToLayer(SHOOTABLE_LAYER_MASK_NAME);

        _friendyFireEntities = new HashSet<INetworkEntity>();
    }

    /// <summary>
    /// Adds the network entity ID of a shooter to be processed.
    /// </summary>
    /// <param name="shootersShotConfig"></param>
    public void AddShooter(ShotConfiguration shootersShotConfig)
    {
        //GONetLog.Debug($"Add shot to queue");
        _shootersShotConfigurationsBuffer.Enqueue(shootersShotConfig);
    }

    /// <summary>
    /// Process the entity shots that ocurred since last time calling this.
    /// </summary>
    public void ProcessShots(IReadOnlyDictionary<uint, INetworkEntity> activeEntities)
    {
        if (IsShootersShotConfigurationsBufferEmpty()) return;

        _raycastShooter.OnShotHit += OnShotHit;

        while (!IsShootersShotConfigurationsBufferEmpty())
        {
            ShotConfiguration shotConfiguration = GetNextShootersShotConfigurationFromBuffer();
            //GONetLog.Debug($"Processing shot to queue");

            var shooterNetworkEntityKVP = activeEntities.FirstOrDefault(x => x.Value?.GetNetworkObject() == shotConfiguration.shooterGNP);
            INetworkEntity shooterNetworkEntity = shooterNetworkEntityKVP.Value;
            if (shooterNetworkEntityKVP.Key == default || shooterNetworkEntity == default)
            {
                //Debug.LogWarning($"The Shooter Network Enity: {shotConfiguration.shooterGNP} could not be found in active entities");
                continue;
            }

            _lastShooterEntityProcessed = shooterNetworkEntity;

            try
            {
                Vector3 shotPoint = Vector3.zero;
                Vector3 shotDirection = Vector3.zero;
                GetShotPointAndDirectionFromEntity(shooterNetworkEntity, out shotPoint, out shotDirection);

                _raycastShooter.ShootWithRaycast(shotPoint, shotDirection, _raycastLayerMask, shotConfiguration.maxDistance);

                if (_succesfulShot)
                {
                    bool isShooterAndShotSameTeam = false;
                    if (shooterNetworkEntity is IColorTeamMember shootersColorTeamMember)
                    {
                        byte shootersEntityColor = shootersColorTeamMember.GetColorTeam();

                        FullBodyPlayerController shotColorTeamMember = _lastShotHitData.gameObject.GetComponentInParent<FullBodyPlayerController>();
                        if (shotColorTeamMember && shotColorTeamMember.PlayerController.GetColorTeam() == shootersEntityColor)
                        {
                            isShooterAndShotSameTeam = true;
                        }
                    }

                    if (!isShooterAndShotSameTeam)
                    {
                        TryToTakeDamage(shotConfiguration.damage);
                        PublishShotEvent(shotConfiguration.shooterGNP.GONetId, _lastShotHitData.gameObject);
                    }

                    _succesfulShot = false;
                }
            }
            catch (Exception ex)
            {
                GONetLog.Error(string.Concat(ex.Message, '\n', ex.StackTrace));
            }
        }

        _raycastShooter.OnShotHit -= OnShotHit;
    }

    private bool IsShootersShotConfigurationsBufferEmpty()
    {
        return _shootersShotConfigurationsBuffer.Count == 0;
    }

    private ShotConfiguration GetNextShootersShotConfigurationFromBuffer()
    {
        return _shootersShotConfigurationsBuffer.Dequeue();
    }

    private void GetShotPointAndDirectionFromEntity(INetworkEntity entity, out Vector3 shotPoint, out Vector3 shotDirection)
    {
        IPlayerNetworkEntity pne = entity as IPlayerNetworkEntity;
        IPlayerState playerState = pne.GetPlayerState();

        Vector3 cameraLookAtEulerAngles;
        playerState.GetShotPointAndShotDirection(out shotPoint, out cameraLookAtEulerAngles);

        float elevation = Mathf.Deg2Rad * cameraLookAtEulerAngles.x;
        float heading = Mathf.Deg2Rad * cameraLookAtEulerAngles.y;
        shotDirection = new Vector3(Mathf.Cos(elevation) * Mathf.Sin(heading), -Mathf.Sin(elevation), Mathf.Cos(elevation) * Mathf.Cos(heading));
    }

    private void OnShotHit(ShotHitData hitData)
    {
        _lastShotHitData = hitData;
        _succesfulShot = true;
    }

    private void PublishShotEvent(uint shooterEntityId, GameObject hitObject)
    {
        LocalContextEntityEvent entityEvent;
        if (_lastShotHitData.surfaceLayerIndex != _shootableLayerMaskIndex)
        {
            entityEvent = new LocalContextEntityEvent(EntityEventType.EV_BULLET_HIT_NON_SHOOTABLE, true, BitConverter.GetBytes((int)shooterEntityId), shooterEntityId);
        }
        else
        {
            INetworkEntityID victimNetworkEntity = hitObject.GetComponent<INetworkEntityID>();
            byte[] entityEventParameters = new byte[8];
            Buffer.BlockCopy(BitConverter.GetBytes((int)shooterEntityId), 0, entityEventParameters, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((int)victimNetworkEntity.GetNetworkEntityId()), 0, entityEventParameters, 4, 4);
            
            entityEvent = new LocalContextEntityEvent(EntityEventType.EV_BULLET_HIT_SHOOTABLE, true, entityEventParameters, shooterEntityId);
        }

        _networkController.Server_PublishShotEvent(
            _lastShotHitData.position, Quaternion.LookRotation(_lastShotHitData.surfaceNormal), entityEvent);
    }

    private void TryToTakeDamage(int damage)
    {
        //{
            //GONetLog.Debug($"shot to queue...same layer? {_lastShotHitData.surfaceLayerIndex == _shootableLayerMaskIndex}  iDammie? {_lastShotHitData.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable)}");
        //}

        if (_lastShotHitData.surfaceLayerIndex == _shootableLayerMaskIndex)
        {
            if (_lastShotHitData.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.Server_TakeDamage(damage);
            }
        }
    }
}
