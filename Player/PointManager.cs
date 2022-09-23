using UnityEngine;

public class PointManager : MonoBehaviour
{
    [Header("Point settings")]
    [SerializeField] [Tooltip("Must be positive")] private uint startingPoints;
    [SerializeField] [Tooltip("Must be positive")] private uint pointsOnDamage;
    [SerializeField] [Tooltip("Must be positive")] private uint pointsOnKill;

    public static uint Points { get; set; }

    private void Start()
    {
        EnemyHealth.DamageEvent += AddDamagePoints;
        EnemyHealth.KillEvent += AddKillPoints;

        Points = startingPoints;
    }

    private void AddDamagePoints() => Points += pointsOnDamage;
    private void AddKillPoints() => Points += pointsOnKill;
}
