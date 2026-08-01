using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public string targetSceneName;
    public string targetSpawnPointName;
    public Transform playerCamera;
    public float facingAngleThreshold = 45f;
    public int minDayRequired = 1;
    public Transform facingCheckPoint; // optional — falls back to this object's own transform if empty
    bool playerNearby;
    bool promptShown;

    void Update()
    {
        if (playerNearby)
        {
            Transform checkTarget = facingCheckPoint != null ? facingCheckPoint : transform;
            bool facing = FacingCheck.IsFacing(playerCamera, checkTarget.position, facingAngleThreshold);

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

            if (facing && Input.GetKeyDown(KeyCode.E) && !SceneTransitionManager.Instance.IsTransitioning)
            {
                if (DayCycleManager.Instance.GetCurrentDay() < minDayRequired)
                {
                    DialogueManager.Instance.StartDialogue("", new string[] { "The door is locked." }, null, false);
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