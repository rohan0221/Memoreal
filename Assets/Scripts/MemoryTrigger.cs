using System.Collections;
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
    public string afterCompletionMessage = "";
    public GameObject objectToHideOnComplete;
    public GameObject objectToShowOnComplete;

    [Header("Guilt Twist Override (optional)")]
    public int guiltTwistDay = 0;
    public Sprite[] guiltTwistFrames;

    bool playerNearby;
    bool guiltTwistPlayed;

    void Update()
    {
        if (!playerNearby || DialogueManager.Instance.IsActive || MemoryCutsceneController.Instance.IsActive) return;
        if (!Input.GetKeyDown(KeyCode.E)) return;

        if (guiltTwistDay != 0 && MemoryManager.Instance.currentDay == guiltTwistDay)
        {
            if (guiltTwistPlayed) return;
            guiltTwistPlayed = true;
            InteractPromptUI.Instance.Hide();
            MemoryCutsceneController.Instance.PlayMemory(cutsceneViewPoint, guiltTwistFrames, jitterInterval, OnGuiltTwistMemoryComplete);
            return;
        }

        if (requiredDay != 0 && MemoryManager.Instance.currentDay != requiredDay)
        {
            InteractPromptUI.Instance.Hide();

            bool alreadyDone = MemoryManager.Instance.IsFlagUnlocked(governingFlag);
            string message = (alreadyDone && !string.IsNullOrEmpty(afterCompletionMessage)) ? afterCompletionMessage : wrongDayMessage;

            DialogueManager.Instance.StartDialogue("", new string[] { message }, null, false);
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

    void OnGuiltTwistMemoryComplete()
    {
        MemoryManager.Instance.UnlockByFlag(Recolourable.MemoryFlag.GuiltTwist);
        StartCoroutine(FadeAndEndGame());
    }

    IEnumerator FadeAndEndGame()
    {
        yield return StartCoroutine(DayCycleManager.Instance.FadeToBlack(0.6f));
        DayCycleManager.Instance.EndGame();
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