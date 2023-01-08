using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Gradient gradient;
        [SerializeField] private Image fill;

        private EnemyHealth _enemy;
        private Transform _eyes;
        
        private void Start()
        {
            _eyes= GameObject.FindWithTag("MainCamera").transform;
            slider.minValue = 0;
            slider.maxValue = EnemyHealth.StartingHealth;
            slider.value = EnemyHealth.StartingHealth;

            fill.color = gradient.Evaluate(EnemyHealth.StartingHealth);
        }
        
        private void Update()
        {
            Transform transform1;
            (transform1 = transform).LookAt(_eyes);

            var rotation = transform1.rotation;
            transform.Rotate(new Vector3(rotation.x, rotation.y - 180, rotation.z));
        }

        public void SetValues(float health)
        { 
            slider.value = health;

            fill.color = gradient.Evaluate(slider.normalizedValue);
        }

        public void SetMaxValues(float health)
        {
            slider.maxValue = health;
            slider.value = health;

            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}