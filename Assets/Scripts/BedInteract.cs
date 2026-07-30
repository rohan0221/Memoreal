using UnityEngine;

public class BedInteract : MonoBehaviour
{
    public Transform playerCamera;
    public float facingAngleThreshold = 45f;
    bool playerNearby;
    bool promptShown;

    void Update()
    {
        if (playerNearby)
        {
            bool facing = FacingCheck.IsFacing(playerCamera, transform.position, facingAngleThreshold);

            if (facing && !promptShown && !DayCycleManager.Instance.IsBusy)
            {
                InteractPromptUI.Instance.Show();
                promptShown = true;
            }
            else if ((!facing || DayCycleManager.Instance.IsBusy) && promptShown)
            {
                InteractPromptUI.Instance.Hide();
                promptShown = false;
            }

            if (facing && !DayCycleManager.Instance.IsBusy && Input.GetKeyDown(KeyCode.E))
            {
                if (!DayCycleManager.Instance.IsDayObjectiveComplete())
                {
                    DialogueManager.Instance.StartDialogue("", new string[] { "You're not tired yet — there's something you still need to do." });
                    return;
                }

                InteractPromptUI.Instance.Hide();
                promptShown = false;
                DayCycleManager.Instance.EndDay();
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