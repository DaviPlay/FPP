using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuFunctions : MonoBehaviour
{
    public GameObject menu;
    public GameObject player;
    public GameObject guns;
    public Camera camera;
    public Slider fovSlider, sensitivitySlider;

    public static bool IsGamePaused;
    public static bool IsAutoReload { get; private set; } = true;
    public static bool FpsCheck { get; private set; } = true;
    public static int HoldToSprint { get; private set; }
    public static int HoldToCrouch { get; private set; }

    public static Action AutoReloadEvent;
    public static Action FPSCheckEvent;

    private void Start()
    {
        fovSlider.value = camera.fieldOfView;
        fovSlider.GetComponentInChildren<InputField>().text = camera.fieldOfView.ToString();

        sensitivitySlider.value = camera.GetComponent<CameraMovement>().mouseSensitivity;
        sensitivitySlider.GetComponentInChildren<InputField>().text = camera.GetComponent<CameraMovement>().mouseSensitivity.ToString();
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        
        if (!IsGamePaused)
            Pause();
        else
            Resume();
    }

    private void Pause()
    {
        menu.SetActive(true);

        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0;

        IsGamePaused = true;
    }

    private void Resume()
    {
        menu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1;

        IsGamePaused = false;
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

    public void SensitivitySlider(float newSensitivity)
    {
        camera.GetComponent<CameraMovement>().mouseSensitivity = newSensitivity;
        sensitivitySlider.GetComponentInChildren<InputField>().text = newSensitivity.ToString();
    }

    public void SensitivityInput(string newSensitivity)
    {
        camera.GetComponent<CameraMovement>().mouseSensitivity = float.Parse(newSensitivity);
        sensitivitySlider.value = float.Parse(newSensitivity);
    }

    public void AutoReloadCheck(bool value)
    {
        IsAutoReload = value;
        AutoReloadEvent?.Invoke();
    }

    public void ReadFpsCheck(bool value)
    {
        FpsCheck = value;
        FPSCheckEvent?.Invoke();
    }

    public void ReadHoldToSprint(int value)
    {
        HoldToSprint = value;
    }

    public void ReadHoldToCrouch(int value)
    {
        HoldToCrouch = value;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
