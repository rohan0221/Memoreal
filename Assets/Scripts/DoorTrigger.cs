using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public string targetSceneName;
    public string targetSpawnPointName;
    public Transform playerCamera; // drag the scene's first-person camera here
    public float facingAngleThreshold = 45f;
    bool playerNearby;
    bool promptShown;

    void Update()
    {
        if (playerNearby)
        {
            bool facing = FacingCheck.IsFacing(playerCamera, transform.position, facingAngleThreshold);

            if (facing && !promptShown)
            {
                InteractPromptUI.Instance.Show();
                promptShown = true;
            }
            else if (!facing && promptShown)
            {
                InteractPromptUI.Instance.Hide();
                promptShown = false;
            }

            if (facing && Input.GetKeyDown(KeyCode.E))
            {
                InteractPromptUI.Instance.Hide();
                promptShown = false;
                SceneTransitionManager.Instance.TransitionTo(targetSceneName, targetSpawnPointName);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            if (promptShown)
            {
                InteractPromptUI.Instance.Hide();
                promptShown = false;
            }
        }
    }
}