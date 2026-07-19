using UnityEngine;

public class MirrorInteract : MonoBehaviour
{
    public GameObject mirrorCameraRig;
    public Camera mainCamera;
    public PlayerMovement playerMovementScript; // whatever your player controller script is called
    bool playerNearby;
    bool inMirrorView;

    void Update()
    {
        if (playerNearby && !inMirrorView && Input.GetKeyDown(KeyCode.E))
        {
            EnterMirror();
        }
        else if (inMirrorView && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)))
        {
            ExitMirror();
        }
    }

    void EnterMirror()
    {
        inMirrorView = true;
        mainCamera.gameObject.SetActive(false);
        mirrorCameraRig.SetActive(true);
        if (playerMovementScript != null) playerMovementScript.enabled = false;
    }

    void ExitMirror()
    {
        inMirrorView = false;
        mirrorCameraRig.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        if (playerMovementScript != null) playerMovementScript.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerNearby = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerNearby = false;
    }
}