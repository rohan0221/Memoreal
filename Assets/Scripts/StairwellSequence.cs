using System.Collections;
using UnityEngine;

public class StairwellSequence : MonoBehaviour
{
    public Sprite[] smellMemoryFrames;
    public float jitterInterval = 0.15f;
    public Transform cutsceneViewPoint;
    public Transform doorLookPoint;
    public string[] nurseDialogueLines;
    public Sprite[] jumpscareFrames;
    public string requiredDayMessage = "There's nothing to do here right now.";
    public int requiredDay = 4;

    bool playerNearby;
    bool hasPlayed;

    void Update()
    {
        if (playerNearby && !hasPlayed && !MemoryCutsceneController.Instance.IsActive && !DialogueManager.Instance.IsActive && Input.GetKeyDown(KeyCode.E))
        {
            if (MemoryManager.Instance.currentDay != requiredDay)
            {
                InteractPromptUI.Instance.Hide();
                DialogueManager.Instance.StartDialogue("", new string[] { requiredDayMessage });
                return;
            }

            hasPlayed = true;
            InteractPromptUI.Instance.Hide();
            MemoryCutsceneController.Instance.PlayMemory(cutsceneViewPoint, smellMemoryFrames, jitterInterval, OnSmellMemoryComplete);
        }
    }

    void OnSmellMemoryComplete()
    {
        MemoryManager.Instance.UnlockByFlag(Recolourable.MemoryFlag.Smell);
        StartCoroutine(DoorDialogueThenJumpscare());
    }

    IEnumerator DoorDialogueThenJumpscare()
    {
        yield return StartCoroutine(MemoryCutsceneController.Instance.RotateCameraTo(doorLookPoint, 1f));

        bool dialogueDone = false;
        DialogueManager.Instance.StartAlternatingDialogue("Nurse 1", "Nurse 2", nurseDialogueLines, () => dialogueDone = true);
        yield return new WaitUntil(() => dialogueDone);

        JumpscareFlash.Instance.Play(jumpscareFrames, OnJumpscareShown);
    }

    void OnJumpscareShown()
    {
        DialogueManager.Instance.StartDialogue("", new string[] { "You should be resting." }, DoFall);
    }

    void DoFall()
    {
        MemoryManager.Instance.wheelchairActive = true;
        DayCycleManager.Instance.EndDay();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            if (!hasPlayed) InteractPromptUI.Instance.Show();
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