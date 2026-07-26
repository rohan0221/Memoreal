using UnityEngine;

public class MemoryTrigger : MonoBehaviour
{
    public Recolourable.MemoryFlag governingFlag;
    public Sprite[] memoryFrames;
    public float jitterInterval = 0.15f;
    public Transform cutsceneViewPoint;
    public string[] postMemoryDialogueLines;
    public int requiredDay = 0;
    public string wrongDayMessage = "There's nothing to do here right now.";
    bool playerNearby;

    void Update()
    {
        if (playerNearby && !MemoryCutsceneController.Instance.IsActive && !DialogueManager.Instance.IsActive && Input.GetKeyDown(KeyCode.E))
        {
            if (requiredDay != 0 && MemoryManager.Instance.currentDay != requiredDay)
            {
                InteractPromptUI.Instance.Hide();
                DialogueManager.Instance.StartDialogue("", new string[] { wrongDayMessage });
                return;
            }

            InteractPromptUI.Instance.Hide();
            MemoryCutsceneController.Instance.PlayMemory(cutsceneViewPoint, memoryFrames, jitterInterval, OnMemoryComplete);
        }
    }

    void OnMemoryComplete()
    {
        MemoryManager.Instance.UnlockByFlag(governingFlag);

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