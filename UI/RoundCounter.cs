using UnityEngine;
using UnityEngine.UI;

public class RoundCounter : MonoBehaviour
{
    public Text round;

    private void Start()
    {
        EnemySpawn.roundSwitch += UpdateText;
    }

    private void UpdateText()
    {
        round.text = (EnemySpawn.nextRound + 1).ToString();
    }
}
