using System.Collections;
using UnityEngine;

public class GuiltTwistTrigger : MonoBehaviour
{
    public Sprite[] bloodyWatchFrames;
    public int requiredDay = 6;
    public string wrongDayMessage = "There's nothing to do here right now.";
    public MonoBehaviour playerMovementScript;
    public MonoBehaviour firstPersonLookScript;
    bool playerNearby;
    bool hasPlayed;

    void Update()
    {
        if (playerNearby && !hasPlayed && !DialogueManager.Instance.IsActive && Input.GetKeyDown(KeyCode.E))
        {
            if (MemoryManager.Instance.currentDay != requiredDay)
            {
                InteractPromptUI.Instance.Hide();
                DialogueManager.Instance.StartDialogue("", new string[] { wrongDayMessage });
                return;
            }

            hasPlayed = true;
            InteractPromptUI.Instance.Hide();
            playerMovementScript.enabled = false;
            firstPersonLookScript.enabled = false;
            StartCoroutine(PlayGuiltTwist());
        }
    }

    IEnumerator PlayGuiltTwist()
    {
        MemoryManager.Instance.UnlockByFlag(Recolourable.MemoryFlag.GuiltTwist);

        JumpscareFlash.Instance.StartFlash(bloodyWatchFrames);
        yield return new WaitForSeconds(1.5f);

        yield return StartCoroutine(DayCycleManager.Instance.FadeToBlack(0.6f));

        JumpscareFlash.Instance.StopFlash();
        DayCycleManager.Instance.EndGame();
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