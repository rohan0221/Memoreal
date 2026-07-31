using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public Recolourable.MemoryFlag watchFlag;
    public string[] beforeLines;
    public string[] afterLines;
    bool playerNearby;

    void Update()
    {
        if (playerNearby && !DialogueManager.Instance.IsActive && !MemoryCutsceneController.Instance.IsActive && Input.GetKeyDown(KeyCode.E))
        {
            InteractPromptUI.Instance.Hide();

            bool done = MemoryManager.Instance.IsFlagUnlocked(watchFlag);
            string[] lines = done ? afterLines : beforeLines;

            DialogueManager.Instance.StartDialogue("", lines);
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