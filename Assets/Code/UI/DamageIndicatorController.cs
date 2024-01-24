using System.Collections.Generic;
using UnityEngine;

public class DamageIndicatorController : MonoBehaviour
{
    private float _lifeTime = 3.5f;
    private Transform _centerTransform;
    private List<DamageIndicator> _activeIndicators;
    [SerializeField] private DamageIndicator _damageIndicatorPrefab;
    private Transform _playerTransform;

    private void Awake()
    {
        _centerTransform = transform;
        _activeIndicators = new List<DamageIndicator>();
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }

    public void RemovePlayerTransform()
    {
        _playerTransform = null;
    }

    public void CleanDamageIndicator()
    {
        foreach (DamageIndicator damageIndicator in _activeIndicators)
        {
            Destroy(damageIndicator.gameObject);
        }

        _activeIndicators.Clear();
    }

    private void Update()
    {
        if ((object)_playerTransform == null) return;

        List<DamageIndicator> damageIndicatorsToRemove = new List<DamageIndicator>();
        foreach (DamageIndicator damageIndicator in _activeIndicators)
        {
            if (damageIndicator.CurtrentLifeTime > 0)
            {
                damageIndicator.UpdateIndicator(_playerTransform.forward, _playerTransform.up, Time.deltaTime);
            }
            else
            {
                damageIndicatorsToRemove.Add(damageIndicator);
            }
        }

        foreach (DamageIndicator damageIndicator in damageIndicatorsToRemove)
        {
            _activeIndicators.Remove(damageIndicator);
            Destroy(damageIndicator.gameObject);
        }
    }

    public void Add(Vector3 direction)
    {
        if ((object)_playerTransform == null) return;

        DamageIndicator newDamageIndicator = Instantiate(_damageIndicatorPrefab, _centerTransform.position, Quaternion.identity, _centerTransform);
        newDamageIndicator.Initialize(_lifeTime, direction);
        _activeIndicators.Add(newDamageIndicator);
    }
}
