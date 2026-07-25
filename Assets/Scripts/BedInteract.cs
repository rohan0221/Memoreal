using UnityEngine;

public class BedInteract : MonoBehaviour
{
    bool playerNearby;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (!DayCycleManager.Instance.IsDayObjectiveComplete())
            {
                DialogueManager.Instance.StartDialogue("", new string[] { "You're not tired yet — there's something you still need to do." });
                return;
            }

            InteractPromptUI.Instance.Hide();
            DayCycleManager.Instance.EndDay();
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