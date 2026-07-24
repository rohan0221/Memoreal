using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI lineText;
    public MonoBehaviour playerMovementScript;

    public bool IsActive { get; private set; }

    void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(string speaker, string[] lines, Action onComplete = null)
    {
        if (IsActive) return;
        StartCoroutine(PlayLines(speaker, lines, onComplete));
    }

    IEnumerator PlayLines(string speaker, string[] lines, Action onComplete)
    {
        IsActive = true;
        playerMovementScript.enabled = false;
        dialoguePanel.SetActive(true);

        foreach (string line in lines)
        {
            speakerText.text = speaker;
            lineText.text = line;

            yield return new WaitForSeconds(0.15f); // avoids the triggering keypress instantly skipping
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space));
        }

        dialoguePanel.SetActive(false);
        playerMovementScript.enabled = true;
        IsActive = false;
        onComplete?.Invoke();
    }
}