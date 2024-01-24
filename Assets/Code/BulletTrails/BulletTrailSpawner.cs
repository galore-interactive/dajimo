using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BulletTrailSpawner : MonoBehaviour
{
    [SerializeField] private BulletTrail _bulletTrailPrefab;
    private float _bulletTrailSpeed = 260f;
    private HashSet<BulletTrail> _activeBulletTrails;

    private void Awake()
    {
        _activeBulletTrails = new HashSet<BulletTrail>();
    }

    public void Spawn(Vector3 originPosition, Vector3 destinationPosition)
    {
        BulletTrail bulletTrail = Instantiate(_bulletTrailPrefab, originPosition, Quaternion.identity);
        bulletTrail.SetDestinationPosition(destinationPosition);
        _activeBulletTrails.Add(bulletTrail);
    }

    private void Update()
    {
        if(_activeBulletTrails.Count == 0)
        {
            return;
        }

        HashSet<BulletTrail> bulletTrailsToRemove = new HashSet<BulletTrail>();

        foreach (BulletTrail trail in _activeBulletTrails)
        {
            trail.UpdateTrail(_bulletTrailSpeed);
            if(trail.HasReachedDestination())
            {
                bulletTrailsToRemove.Add(trail);
            }
        }

        foreach (BulletTrail trail in bulletTrailsToRemove)
        {
            _activeBulletTrails.Remove(trail);
            Destroy(trail.gameObject);
        }
    }
}