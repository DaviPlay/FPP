using UnityEngine;
using UnityEngine.UI;

public class RoundCounter : MonoBehaviour
{
    public Text round;

    private void Start()
    {
        EnemySpawn.RoundSwitch += UpdateText;
    }

    private void UpdateText() => round.text = (EnemySpawn.NextRound + 1).ToString();
}
