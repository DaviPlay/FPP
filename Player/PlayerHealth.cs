using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private static float maxHealth = 150;
    private readonly HealthSystem healthSystem = new HealthSystem(maxHealth);
    [SerializeField] private HealthBar healthBar;

    [SerializeField] private float healthPerSecond;
    [SerializeField] private float healthRegenDelay;
    private float timeSinceLastHit;
    private bool isRegenning;

    Coroutine regen;

    private void Start() => timeSinceLastHit = healthRegenDelay;

    private void Update()
    {
        if (timeSinceLastHit <= 0 && healthSystem.Health < healthSystem.HealthMax)
        {
            Regen();
        }
        else
        {
            timeSinceLastHit -= Time.deltaTime;
            isRegenning = false;

            if (timeSinceLastHit < 0)
                timeSinceLastHit = 0;
        }
    }

    public void Damage(float damage)
    {
        healthSystem.Damage(damage);
        timeSinceLastHit = healthRegenDelay;

        healthBar.SetValues(healthSystem.Health);
    }

    private void Regen()
    {
        isRegenning = true;

        healthSystem.Health += healthPerSecond * Time.deltaTime;
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
