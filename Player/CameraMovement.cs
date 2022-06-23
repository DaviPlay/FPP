using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public Camera main;

    public float mouseSensivity = 100f;
    float xRotation = 0f, yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensivity * 10 * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * 10 * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        player.Rotate(Vector3.up * mouseX);
    }
}
