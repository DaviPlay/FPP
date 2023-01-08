using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuFunctions : MonoBehaviour
    {
        public GameObject menu;
        public GameObject player;
        public new Camera camera;
        public Slider fovSlider, sensitivitySlider;
        public TMP_InputField fovInput, sensitivityInput;

        public static bool IsGamePaused;
        public static bool IsAutoReload { get; private set; } = true;
        public static bool FpsCheck { get; private set; } = true;
        public static int HoldToSprint { get; private set; }
        public static int HoldToCrouch { get; private set; }

        private static Action _autoReloadEvent;
        public static Action FPSCheckEvent;

        private void Start()
        {
            fovSlider.maxValue = 180;
            fovSlider.value = camera.fieldOfView;
            fovInput.text = camera.fieldOfView.ToString();

            sensitivitySlider.maxValue = 100;
            sensitivitySlider.value = camera.GetComponent<CameraMovement>().mouseSensitivity;
            sensitivityInput.text = camera.GetComponent<CameraMovement>().mouseSensitivity.ToString();
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
            camera.fieldOfView = (int)newFov;
            fovInput.text = ((int)newFov).ToString();
        }

        public void FovInput(string newFov)
        {
            var fov = 0;
            try
            {
                fov = Mathf.Clamp((int)float.Parse(newFov), 0, 180);
            }
            catch (FormatException)
            {
                
            }

            fovInput.text = fov.ToString();
            player.GetComponent<PlayerMovement>().fov = fov;
            camera.fieldOfView = fov;
            fovSlider.value = fov;
        }

        public void SensitivitySlider(float newSensitivity)
        {
            camera.GetComponent<CameraMovement>().mouseSensitivity = (int)newSensitivity;
            sensitivityInput.text = ((int)newSensitivity).ToString();
        }

        public void SensitivityInput(string newSensitivity)
        {
            var sensitivity = 0;
            try
            {
                sensitivity = (int)Mathf.Clamp(float.Parse(newSensitivity), 0, 100);
            }
            catch (FormatException)
            {
            }

            sensitivityInput.text = sensitivity.ToString();
            camera.GetComponent<CameraMovement>().mouseSensitivity = sensitivity;
            sensitivitySlider.value = sensitivity;
        }

        public void AutoReloadCheck(bool value)
        {
            IsAutoReload = value;
            _autoReloadEvent?.Invoke();
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
}
