using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public string targetSceneName;
    public string targetSpawnPointName;
    bool playerNearby;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            InteractPromptUI.Instance.Hide();
            SceneTransitionManager.Instance.TransitionTo(targetSceneName, targetSpawnPointName);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            InteractPromptUI.Instance.Show();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            InteractPromptUI.Instance.Hide();
        }
    }
}