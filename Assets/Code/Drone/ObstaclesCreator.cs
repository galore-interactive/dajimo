using GONet;
using UnityEngine;

public class ObstaclesCreator
{
    private readonly NetworkController _networkController;
    private readonly Transform _lookAtTransform;
    private readonly bool _isServer;
    private readonly GameObject _objectBlueprintPrefab;
    private GameObject _currentBlueprintobjectToPlace;
    private Vector3 _blueprintPosition;
    private const string GROUND_LAYER_MASK_NAME = "Ground";
    private readonly LayerMask _raycastLayerMask;
    private readonly float _obstacleLifetime;

    private Renderer _blueprintRenderer;

    private bool _isPlacingObject;
    private float _timeUntilObjctIsPlaced;
    private float _placingObjectForwardY;

    public bool IsPlacingObject => _isPlacingObject;
    public float TimeUntilObjectIsPlaced => _timeUntilObjctIsPlaced;

    public ObstaclesCreator(NetworkController networkController, Transform lookAtTransform, GameObject objectBlueprintPrefab, bool isServer, float obstacleBlueprintLifetime)
    {
        _networkController = networkController;
        _lookAtTransform = lookAtTransform;
        _objectBlueprintPrefab = objectBlueprintPrefab;
        _isServer = isServer;

        const string NON_SHOOTABLE_LAYER_MASK_NAME = "NonShootable";
        const string IGNORE_RAYCAST_LAYER_MASK_NAME = "Ignore Raycast";
        _raycastLayerMask = ~(LayerMask.GetMask(NON_SHOOTABLE_LAYER_MASK_NAME) | LayerMask.GetMask(IGNORE_RAYCAST_LAYER_MASK_NAME));
        _obstacleLifetime = obstacleBlueprintLifetime;
    }

    public void Simulate(bool shouldStartPlacingObjectNow, float deltaTime, float placingObjectForwardY = 0)
    {
        _placingObjectForwardY = placingObjectForwardY;

        if (shouldStartPlacingObjectNow && !_isPlacingObject)
        {
            StartPlacingObject();
        }

        if (_isPlacingObject)
        {
            UpdateObjectPosition();

            if (!_isServer)
            {
                _currentBlueprintobjectToPlace.transform.position = _blueprintPosition;
            }

            UpdateCountdown(deltaTime);

            if (!_isServer)
            {
                UpdateBlueprintVisibility();
            }
        }
    }

    private void UpdateCountdown(float deltaTime)
    {
        _timeUntilObjctIsPlaced -= deltaTime;

        if (_timeUntilObjctIsPlaced <= 0)
        {
            if (_isServer)
            {
                Server_PlaceObject();
            }

            StopPlacingObject();
        }
    }

    private void UpdateBlueprintVisibility()
    {
        float transparency = Mathf.Clamp01(1f - (_timeUntilObjctIsPlaced / _obstacleLifetime));
        Color currentColor = _blueprintRenderer.material.color; // In order to be able to apply transparency to a material, it needs to have the 'Surface Type' setting as 'Transparent' instead of 'Opaque'
        _blueprintRenderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, transparency);
    }

    private void StartPlacingObject()
    {
        _isPlacingObject = true;
        _timeUntilObjctIsPlaced = _obstacleLifetime;

        if (!_isServer)
        {
            CreateBlueprint();
        }
    }

    private void CreateBlueprint()
    {
        _currentBlueprintobjectToPlace = GameObject.Instantiate(_objectBlueprintPrefab);
        _blueprintRenderer = _currentBlueprintobjectToPlace.GetComponentInChildren<Renderer>();
    }

    private void StopPlacingObject()
    {
        _isPlacingObject = false;
        _timeUntilObjctIsPlaced = -1f;

        if (!_isServer)
        {
            DestroyBlueprint();
        }
    }

    private void DestroyBlueprint()
    {
        GameObject.Destroy(_currentBlueprintobjectToPlace);
    }

    private void Server_PlaceObject()
    {
        _networkController.Server_SpawnObstacleEntity(_blueprintPosition, Quaternion.identity);
    }

    private void UpdateObjectPosition()
    {
        RaycastHit hit;
        Vector3 forward = _lookAtTransform.forward;
        if (GONetMain.IsServer)
        {
            forward.y = _placingObjectForwardY;
        }
        //GONetLog.Debug($"_lookAtTransform.position: {_lookAtTransform.position}, forward: {forward}");
        if (Physics.Raycast(_lookAtTransform.position, forward, out hit, Mathf.Infinity, _raycastLayerMask))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer(GROUND_LAYER_MASK_NAME))
            {
                _blueprintPosition = hit.point;
                //GONetLog.Debug($"blueprint position: {_blueprintPosition}");
            }
        }
    }

    public void Disable()
    {
        if (_isPlacingObject)
        {
            StopPlacingObject();
        }
    }

    public void CancelPlacement()
    {
        if (GONetMain.IsServer)
        {
            _isPlacingObject = false;
        }
        else
        {
            StopPlacingObject();
        }
    }
}
