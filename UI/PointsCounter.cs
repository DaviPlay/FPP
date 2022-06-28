using UnityEngine;
using UnityEngine.UI;

public class PointsCounter : MonoBehaviour
{
    [SerializeField] private Text points;

    void Start()
    {
        EnemyHealth.damageEvent += UpdateText;
        EnemyHealth.killEvent += UpdateText;

        points.text = "0";
    }

    private void UpdateText() => points.text = PointManager.points.ToString();
}
