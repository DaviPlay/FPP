using Enemy;
using TMPro;
using UnityEngine;

namespace UI
{
    public class RoundCounter : MonoBehaviour
    {
        public TMP_Text round;

        private void Start()
        {
            EnemySpawn.RoundSwitch += UpdateText;
        }

        private void UpdateText() => round.text = (EnemySpawn.NextRound + 1).ToString();
    }
}
