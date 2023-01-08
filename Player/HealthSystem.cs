namespace Player
{
    public class HealthSystem
    {
        public float Health { get; set; }
        public float HealthMin { get; set; }
        public float HealthMax { get; set; }

        public HealthSystem(float healthMax, float healthMin = 0)
        {
            Health = healthMax;
            HealthMin = healthMin;
            HealthMax = healthMax;
        }

        public void Damage(float damage)
        {
            Health -= damage;

            if (Health < HealthMin) Health = HealthMin;
        }

        public void Heal(float heal)
        {
            Health += heal;

            if (Health > HealthMax) Health = HealthMax;
        }
    }
}
