using System;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamageable
{
    public event Action OnDamageReceived;
    public event Action OnKill;

    private int _currentHealth;
    private int _maxHealth;
    private bool _isDead;

    public int CurrentHealth => _currentHealth;

    public void Init(int maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
        _isDead = false;
    }

    public void Server_TakeDamage(int damage)
    {
        if(_isDead)
        {
            return;
        }

        _currentHealth -= damage;

        if(_currentHealth < 0)
        {
            _currentHealth = 0;
        }

        OnDamageReceived?.Invoke();

        if (_currentHealth == 0)
        {
            Kill();
        }
    }

    private void Kill()
    {
        _isDead = true;
        OnKill?.Invoke();
    }
}
