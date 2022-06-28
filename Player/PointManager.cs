using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static int points = 0;

    private void Start()
    {
        EnemyHealth.damageEvent += AddDamagePoints;
        EnemyHealth.killEvent += AddKillPoints;
    }

    private void AddDamagePoints() => points += 10;
    private void AddKillPoints() => points += 50;
}
