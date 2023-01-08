using System;
using Interfaces;
using Player;
using UnityEngine;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float startingMaxHealth = 150;
        public static float StartingHealth;
        [SerializeField] private EnemyHealthBar healthBar;
        private HealthSystem _healthSystem;

        public static Action DamageEvent;
        public static Action KillEvent;

        private void Start()
        {
            StartingHealth = startingMaxHealth;
            
            _healthSystem = new HealthSystem(startingMaxHealth);
        }

        private void Heal(float heal)
        {
            _healthSystem.Heal(heal);
            healthBar.SetValues(_healthSystem.Health);
        }
        
        public void Damage(float damage)
        {
            _healthSystem.Damage(damage);
            DamageEvent?.Invoke();
            healthBar.SetValues(_healthSystem.Health);

            if (_healthSystem.Health == 0) Kill();
        }

        private void Kill()
        {
            Destroy(gameObject);
            KillEvent?.Invoke();
        }

        public void SetMaxHealth(float healthMax)
        {
            _healthSystem.HealthMax = healthMax;

            healthBar.SetMaxValues(healthMax);
        }

        public float GetMaxHealth()
        {
            try
            {
                return _healthSystem.HealthMax;
            }
            catch (NullReferenceException)
            {
                
            }

            return startingMaxHealth;
        }

        public float GetHealth() => _healthSystem.Health;
    }
}
