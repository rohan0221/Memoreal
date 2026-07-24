using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    public Transform playerBody; // drag the Player capsule here
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 45f;

    float pitch = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Yaw: rotate the whole player body left/right (unrestricted)
        playerBody.Rotate(Vector3.up * mouseX);

        // Pitch: rotate only the camera up/down, clamped
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
        transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }
}