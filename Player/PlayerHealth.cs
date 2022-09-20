using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private static float _maxHealth = 150;
    private readonly HealthSystem _healthSystem = new(_maxHealth);
    [SerializeField] private HealthBar healthBar;

    [SerializeField] private float healthPerSecond;
    [SerializeField] private float healthRegenDelay;
    private float _timeSinceLastHit;
    private bool _isRegenerating;

    private Coroutine _regen;

    private void Start() => _timeSinceLastHit = healthRegenDelay;

    private void Update()
    {
        if (_timeSinceLastHit <= 0 && _healthSystem.Health < _healthSystem.HealthMax)
        {
            Regen();
        }
        else
        {
            _timeSinceLastHit -= Time.deltaTime;
            _isRegenerating = false;

            if (_timeSinceLastHit < 0)
                _timeSinceLastHit = 0;
        }
    }

    public void Damage(float damage)
    {
        _healthSystem.Damage(damage);
        _timeSinceLastHit = healthRegenDelay;

        healthBar.SetValues(_healthSystem.Health);
    }

    private void Regen()
    {
        _isRegenerating = true;

        _healthSystem.Health += healthPerSecond * Time.deltaTime;
        healthBar.SetValues(_healthSystem.Health);
    }

    public void SetMaxHealth(float healthMax)
    {
        _maxHealth = healthMax;

        healthBar.SetMaxValues(healthMax);
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
