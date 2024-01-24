using GONet;
using System;
using UnityEngine;

public class FullBodyPlayerController : MonoBehaviour
{
    const string RAGDOLL_LAYER_MASK_NAME = "Ragdoll";
    int _ragdollLayer;
    const string SHOOTABLE_LAYER_MASK_NAME = "Shootable";
    int _shootableLayer;

    public PlayerController PlayerController { get; private set; }
    private Transform _interpolatedPlayerTransform;
    private Rigidbody[] _ragdollRigidbodies;
    private Collider[] _ragdollColliders;
    [SerializeField] private Animator _animator;
    private bool _isRagdoll = false;
    private DamageablePart[] _damageableParts;
    [SerializeField] private Transform _weaponHolderTransform;
    [SerializeField] private Renderer _fullBodyVisualsRenderer;

    public Animator FullBodyAnimator => _animator;
    public Transform WeaponsHolderTransform => _weaponHolderTransform;

    private void Awake()
    {
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        SetRigidbodiesKinematicOption(true);

        _ragdollColliders = GetComponentsInChildren<Collider>();
        SetCollidersIsTriggerOption(true);

        _damageableParts = GetDamageableParts();

        _ragdollLayer = LayerMask.NameToLayer(RAGDOLL_LAYER_MASK_NAME);
        _shootableLayer = LayerMask.NameToLayer(SHOOTABLE_LAYER_MASK_NAME);
    }

    private DamageablePart[] GetDamageableParts()
    {
        return GetComponentsInChildren<DamageablePart>();
    }

    public void SetTransform(Transform a)
    {
        _interpolatedPlayerTransform = a;
    }

    public void SetCorrelationData(PlayerController playerController)
    {
        this.PlayerController = playerController;
    }

    private void Update()
    {
        if (_isRagdoll) return;

        Vector3 newPos = _interpolatedPlayerTransform.position;
        transform.position = newPos;
        transform.rotation = _interpolatedPlayerTransform.rotation;
    }

    public void SetDamageableEntityToParts(IDamageable damageableEntity)
    {
        for (int i = 0; i < _damageableParts.Length; ++i)
        {
            _damageableParts[i].SetDamageableEntity(damageableEntity);
        }
    }

    public void SetNetworkEntityIDToDamageableParts(uint networkEntityId)
    {
        for (int i = 0; i < _damageableParts.Length; ++i)
        {
            _damageableParts[i].SetNetworkEntityID(networkEntityId);
        }
    }

    public void ActivateRagdoll()
    {
        _isRagdoll = true;
        SetRigidbodiesKinematicOption(false);
        SetCollidersIsTriggerOption(false);
        _animator.enabled = false;
        SwitchToLayer(_ragdollLayer); //To avoid physics collisions between this ragdoll and ghost objects
    }

    public void DeactivateRagdoll()
    {
        _isRagdoll = false;
        SetRigidbodiesKinematicOption(true);
        SetCollidersIsTriggerOption(true);
        _animator.enabled = true;
        SwitchToLayer(_shootableLayer);
    }

    private void SetRigidbodiesKinematicOption(bool status)
    {
        for (int i = 0; i < _ragdollRigidbodies.Length; ++i)
        {
            _ragdollRigidbodies[i].isKinematic = status;
        }
    }

    private void SetCollidersIsTriggerOption(bool status)
    {
        for (int i = 0; i < _ragdollColliders.Length; ++i)
        {
            _ragdollColliders[i].isTrigger = status;
        }
    }

    private void SwitchToLayer(int layer)
    {
        gameObject.layer = layer;

        for (int i = 0; i < _ragdollColliders.Length; ++i)
        {
            _ragdollColliders[i].gameObject.layer = layer;
        }
    }

    public void SetColorTeam(Color color)
    {
        _fullBodyVisualsRenderer.material.color = color;
    }

    public void SetRagdollCollidersVisibility(bool visibility)
    {
        for (int i = 0; i < _ragdollColliders.Length; ++i)
        {
            Collider collider = _ragdollColliders[i];
            if (collider)
            {
                collider.enabled = visibility;
            }
        }
    }
}
