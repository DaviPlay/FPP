using UnityEngine;
using UnityEngine.UI;

public class PointsCounter : MonoBehaviour
{
    [SerializeField] private Text points;

    private void Start()
    {
        EnemyHealth.DamageEvent += OnUpdateText;
        EnemyHealth.KillEvent += OnUpdateText;
        Shooting.UpdateText += OnUpdateText;

        points.text = PointManager.Points.ToString();
    }

    private void OnUpdateText() => points.text = PointManager.Points.ToString();
}
