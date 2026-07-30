using UnityEngine;

public class StairFallTrigger : MonoBehaviour
{
    public int requiredDay = 4;
    public Sprite jumpscareSprite;
    bool hasFired;

    void OnTriggerEnter(Collider other)
    {
        if (hasFired) return;
        if (!other.CompareTag("Player")) return;
        if (MemoryManager.Instance.currentDay != requiredDay) return;

        hasFired = true;
        JumpscareFlash.Instance.Play(jumpscareSprite, ShowDialogue);
    }

    void ShowDialogue()
    {
        DialogueManager.Instance.StartDialogue("", new string[] { "You should be resting." }, DoFall);
    }

    void DoFall()
    {
        MemoryManager.Instance.wheelchairActive = true;
        DayCycleManager.Instance.EndDay();
    }
}