using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private static float maxHealth = 50;
    private readonly HealthSystem healthSystem = new HealthSystem(maxHealth);

    public void Damage(float damage)
    {
        healthSystem.Damage(damage);

        if (healthSystem.Health == 0) Destroy(gameObject);
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
