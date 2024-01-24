using GONet;
using System;
using UnityEngine;

public class Health
{
    public event Action OnKilled;
    private int _currentHealth;
    private readonly int _maxHealth;

    public Health(int maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        //GONetLog.Debug($"shot to queue...currentHealth: {_currentHealth}..OnKilled? {OnKilled != null}");
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Kill();
        }
    }

    private void Kill()
    {
        OnKilled?.Invoke();
    }

    public void ResetHealthToMaximum()
    {
        _currentHealth = _maxHealth;
    }
}
