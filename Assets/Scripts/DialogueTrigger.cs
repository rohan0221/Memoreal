using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string speakerName = "Nurse";
    public string[] lines = { "This is the first line.", "This is the second line." };
    bool hasTriggered;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // remove this line + check if you want it to repeat every time you walk in
            DialogueManager.Instance.StartDialogue(speakerName, lines);
        }
    }
}