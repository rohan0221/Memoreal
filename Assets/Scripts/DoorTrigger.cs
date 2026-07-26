using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public string targetSceneName;
    public string targetSpawnPointName;
    public Transform playerCamera;
    public float facingAngleThreshold = 45f;
    public int minDayRequired = 1; // door only opens once currentDay >= this
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
                if (DayCycleManager.Instance.GetCurrentDay() < minDayRequired)
                {
                    DialogueManager.Instance.StartDialogue("", new string[] { "The door is locked." });
                    return;
                }

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