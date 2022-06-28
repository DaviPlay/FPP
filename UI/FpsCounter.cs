using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] private Text fpsText;
    [SerializeField] private float hudRefreshRate = 1;

    private float timer;

    void Start()
    {
        MenuFunctions.fpsCheckEvent += EnableAndDisable;
    }

    private void Update()
    {
        if (Time.unscaledTime > timer)
        {
            int fps = (int)(1 / Time.unscaledDeltaTime);
            fpsText.text = fps + " FPS";
            timer = Time.unscaledTime + hudRefreshRate;
        }
    }

    private void EnableAndDisable()
    {
        gameObject.SetActive(MenuFunctions.FpsCheck);
    }
}
