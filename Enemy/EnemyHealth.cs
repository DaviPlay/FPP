using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private static float _maxHealth = 50;
    private readonly HealthSystem _healthSystem = new(_maxHealth);

    public static Action DamageEvent;
    public static Action KillEvent;

    public void Damage(float damage)
    {
        _healthSystem.Damage(damage);
        DamageEvent?.Invoke();

        if (_healthSystem.Health == 0) Kill();
    }

    private void Kill()
    {
        Destroy(gameObject);
        KillEvent?.Invoke();
    }

    public void SetMaxHealth(float healthMax)
    {
        _maxHealth = healthMax;
    }

    public float GetMaxHealth()
    {
        return _healthSystem.HealthMax;
    }

    public float GetHealth()
    {
        return _healthSystem.Health;
    }
}
