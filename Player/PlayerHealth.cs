using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {

        [SerializeField] private float maxHealth = 150;
        [SerializeField] private float healthPerSecond;
        [SerializeField] private float healthRegenDelay;
        private float _timeSinceLastHit;
        [HideInInspector] public bool isRegenerating;
        
        private HealthSystem _healthSystem;
        [SerializeField] private HealthBar healthBar;

        private Coroutine _regen;

        private void Start()
        {
            _healthSystem = new HealthSystem(maxHealth);
            
            _timeSinceLastHit = healthRegenDelay;
        }

        private void Update()
        {
            if (_timeSinceLastHit <= 0 && _healthSystem.Health < _healthSystem.HealthMax)
            {
                Regen();
            }
            else
            {
                _timeSinceLastHit -= Time.deltaTime;
                isRegenerating = false;

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
        
        private void Heal(float heal)
        {
            _healthSystem.Heal(heal);
            healthBar.SetValues(_healthSystem.Health);
        }
        
        private void Regen()
        {
            isRegenerating = true;

            _healthSystem.Health += healthPerSecond * Time.deltaTime;
            healthBar.SetValues(_healthSystem.Health);
        }

        public void SetMaxHealth(float healthMax)
        {
            maxHealth = healthMax;

            healthBar.SetMaxValues(healthMax);
        }

        public float GetMaxHealth() => _healthSystem.HealthMax;

        public float GetHealth() => _healthSystem.Health;
    }
}
