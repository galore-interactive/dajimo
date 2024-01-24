using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pruebaHealth : MonoBehaviour
{
    private HealthController _healthController;

    private void Awake()
    {
        _healthController = GetComponent<HealthController>();
        _healthController.Init(100);
    }

    private void OnEnable()
    {
        _healthController.OnDamageReceived += () => Debug.Log($"Ouch, {_healthController.CurrentHealth}");
        _healthController.OnKill += () => Debug.Log("Killed");
    }

    private void OnDisable()
    {
        _healthController.OnDamageReceived -= () => Debug.Log($"Ouch, {_healthController.CurrentHealth}");
        _healthController.OnKill -= () => Debug.Log("Killed");
    }

    public void TakeDamage(int damage)
    {
        _healthController.Server_TakeDamage(damage);
    }
}
