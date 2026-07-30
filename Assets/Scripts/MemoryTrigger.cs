using UnityEngine;

public class MemoryTrigger : MonoBehaviour
{
    public Recolourable.MemoryFlag governingFlag;
    public Sprite[] memoryFrames;
    public float jitterInterval = 0.15f;
    public Transform cutsceneViewPoint;
    public string[] preMemoryDialogueLines;
    public string[] postMemoryDialogueLines;
    public int requiredDay = 0;
    public string wrongDayMessage = "There's nothing to do here right now.";
    public string afterCompletionMessage = ""; // shown instead, once this memory's already been done
    public GameObject objectToHideOnComplete;
    public GameObject objectToShowOnComplete;
    bool playerNearby;

    void Update()
    {
        if (playerNearby && !MemoryCutsceneController.Instance.IsActive && !DialogueManager.Instance.IsActive && Input.GetKeyDown(KeyCode.E))
        {
            if (requiredDay != 0 && MemoryManager.Instance.currentDay != requiredDay)
            {
                InteractPromptUI.Instance.Hide();

                bool alreadyDone = MemoryManager.Instance.IsFlagUnlocked(governingFlag);
                string message = (alreadyDone && !string.IsNullOrEmpty(afterCompletionMessage)) ? afterCompletionMessage : wrongDayMessage;

                DialogueManager.Instance.StartDialogue("", new string[] { message });
                return;
            }

            InteractPromptUI.Instance.Hide();

            if (preMemoryDialogueLines != null && preMemoryDialogueLines.Length > 0)
            {
                DialogueManager.Instance.StartDialogue("", preMemoryDialogueLines, StartMemory);
            }
            else
            {
                StartMemory();
            }
        }
    }

    void StartMemory()
    {
        MemoryCutsceneController.Instance.PlayMemory(cutsceneViewPoint, memoryFrames, jitterInterval, OnMemoryComplete);
    }

    void OnMemoryComplete()
    {
        MemoryManager.Instance.UnlockByFlag(governingFlag);

        if (objectToHideOnComplete != null) objectToHideOnComplete.SetActive(false);
        if (objectToShowOnComplete != null) objectToShowOnComplete.SetActive(true);

        if (postMemoryDialogueLines != null && postMemoryDialogueLines.Length > 0)
        {
            DialogueManager.Instance.StartDialogue("", postMemoryDialogueLines);
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