using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private static float maxHealth = 150;
    private readonly HealthSystem healthSystem = new HealthSystem(maxHealth);
    public HealthBar healthBar;

    public void Damage(float damage)
    {
        healthSystem.Damage(damage);

        healthBar.SetValues(healthSystem.Health);
    }

    public void SetMaxHealth(float healthMax)
    {
        maxHealth = healthMax;

        healthBar.SetMaxValues(healthMax);
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
