using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private static float maxHealth = 50;
    private readonly HealthSystem healthSystem = new HealthSystem(maxHealth);

    public static Action damageEvent;
    public static Action killEvent;

    public void Damage(float damage)
    {
        healthSystem.Damage(damage);
        damageEvent?.Invoke();

        if (healthSystem.Health == 0) Kill();
    }

    public void Kill()
    {
        Destroy(gameObject);
        killEvent?.Invoke();
    }

    public void SetMaxHealth(float healthMax)
    {
        maxHealth = healthMax;
    }

    public float GetMaxHealth()
    {
        return healthSystem.HealthMax;
    }

    public float GetHealth()
    {
        return healthSystem.Health;
    }
}
