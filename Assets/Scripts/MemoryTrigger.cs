using UnityEngine;

public class MemoryTrigger : MonoBehaviour
{
    public Recolourable.MemoryFlag governingFlag;
    public Sprite[] memoryFrames;
    public float jitterInterval = 0.15f;
    public Transform cutsceneViewPoint;
    public string[] postMemoryDialogueLines; // optional — leave empty array if not needed
    bool playerNearby;

    void Update()
    {
        if (playerNearby && !MemoryCutsceneController.Instance.IsActive && !DialogueManager.Instance.IsActive && Input.GetKeyDown(KeyCode.E))
        {
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