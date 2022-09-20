using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public Camera main;

    public float mouseSensitivity = 100f;
    private float _xRotation, _yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        var position = player.transform.position;
        transform.position = new Vector3(position.x, position.y + 1, position.z);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 10 * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 10 * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);

        player.Rotate(Vector3.up * mouseX);
    }
}
