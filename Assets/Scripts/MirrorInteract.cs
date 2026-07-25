using UnityEngine;

public class MirrorInteract : MonoBehaviour
{
    public MirrorViewController mirrorView;
    public MonoBehaviour playerMovementScript;
    public MonoBehaviour firstPersonLookScript;
    bool playerNearby;

    void Update()
    {
        if (mirrorView.IsViewing)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
            {
                mirrorView.StopViewing();
                playerMovementScript.enabled = true;
                firstPersonLookScript.enabled = true;
                MemoryManager.Instance.mirrorSeen = true;
            }
            return;
        }

        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            InteractPromptUI.Instance.Hide();
            playerMovementScript.enabled = false;
            firstPersonLookScript.enabled = false;
            mirrorView.StartViewing();
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