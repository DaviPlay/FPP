using UnityEngine;
using UnityEngine.UI;

public class PointsCounter : MonoBehaviour
{
    [SerializeField] private Text points;

    private void Start()
    {
        EnemyHealth.DamageEvent += UpdateText;
        EnemyHealth.KillEvent += UpdateText;

        points.text = PointManager.Points.ToString();
    }

    private void UpdateText() => points.text = PointManager.Points.ToString();
}
