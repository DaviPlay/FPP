using TMPro;
using UnityEngine;

namespace UI
{
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text fpsText;
        [SerializeField] private float hudRefreshRate = 1;

        private float _timer;

        private void Start() => MenuFunctions.FPSCheckEvent += EnableAndDisable;

        private void Update()
        {
            if (!(Time.unscaledTime > _timer)) return;
        
            int fps = (int)(1 / Time.unscaledDeltaTime);
            fpsText.text = fps + " FPS";
            _timer = Time.unscaledTime + hudRefreshRate;
        }

        private void EnableAndDisable() => gameObject.SetActive(MenuFunctions.FpsCheck);
    }
}
