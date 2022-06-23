using UnityEngine;
using UnityEngine.UI;

public class MenuFunctions : MonoBehaviour
{
    public GameObject menu;
    public GameObject player;
    public GameObject guns;
    public new Camera camera;
    public Slider fovSlider, sensivitySlider;

    public static bool isGamePaused;
    private bool autoReload = true;

    void Start()
    {
        fovSlider.value = camera.fieldOfView;
        fovSlider.GetComponentInChildren<InputField>().text = camera.fieldOfView.ToString();

        sensivitySlider.value = camera.GetComponent<CameraMovement>().mouseSensivity;
        sensivitySlider.GetComponentInChildren<InputField>().text = camera.GetComponent<CameraMovement>().mouseSensivity.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Enable menu
            if (!isGamePaused)
            {
                Pause();
            }
            //Disable menu
            else
            {
                Resume();
            }
        }
    }

    private void Pause()
    {
        menu.SetActive(true);

        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0;

        isGamePaused = true;
    }

    private void Resume()
    {
        menu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1;

        isGamePaused = false;
    }

    public void FovSlider(float newFov)
    {
        player.GetComponent<PlayerMovement>().fov = newFov;
        camera.fieldOfView = newFov;
        fovSlider.GetComponentInChildren<InputField>().text = newFov.ToString();
    }

    public void FovInput(string newFov)
    {
        player.GetComponent<PlayerMovement>().fov = float.Parse(newFov);
        camera.fieldOfView = float.Parse(newFov);
        fovSlider.value = float.Parse(newFov);
    }

    public void SensivitySlider(float newSensivity)
    {
        camera.GetComponent<CameraMovement>().mouseSensivity = newSensivity;
        sensivitySlider.GetComponentInChildren<InputField>().text = newSensivity.ToString();
    }

    public void SensivityInput(string newSensivity)
    {
        camera.GetComponent<CameraMovement>().mouseSensivity = float.Parse(newSensivity);
        sensivitySlider.value = float.Parse(newSensivity);
    }

    public void ReadAutoReload(bool value)
    {
        autoReload = value;
    }

    public bool AutoReload { get => autoReload; }

    public void ExtiGame()
    {
        Application.Quit();
    }
}
