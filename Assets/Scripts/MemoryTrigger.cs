using UnityEngine;

public class MemoryTrigger : MonoBehaviour
{
    public Recolourable.MemoryFlag governingFlag;
    public Sprite[] memoryFrames;
    public float jitterInterval = 0.15f;
    bool playerNearby;

    void Update()
    {
        if (playerNearby && !MemoryCutsceneController.Instance.IsActive && Input.GetKeyDown(KeyCode.E))
        {
            InteractPromptUI.Instance.Hide();
            MemoryCutsceneController.Instance.PlayMemory(transform, memoryFrames, jitterInterval, OnMemoryComplete);
        }
    }

    void OnMemoryComplete()
    {
        MemoryManager.Instance.UnlockByFlag(governingFlag);
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