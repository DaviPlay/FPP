using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;
        public Gradient gradient;
        public Image fill;

        public PlayerHealth player;

        private void Start()
        {
            slider.minValue = 0;
            slider.maxValue = player.GetMaxHealth();
            slider.value = player.GetMaxHealth();

            fill.color = gradient.Evaluate(player.GetMaxHealth());
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
