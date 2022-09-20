using UnityEngine;

public class PointManager : MonoBehaviour
{
    public int pointsOnDamage;
    public int pointsOnKill;
    
    public static int Points;

    private void Start()
    {
        EnemyHealth.DamageEvent += AddDamagePoints;
        EnemyHealth.KillEvent += AddKillPoints;
    }

    private void AddDamagePoints() => Points += pointsOnDamage;
    private void AddKillPoints() => Points += pointsOnKill;
}
