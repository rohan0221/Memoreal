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
    public MonoBehaviour firstPersonLookScript;

    public GameObject dialoguePanelLeft;
    public TextMeshProUGUI speakerTextLeft;
    public TextMeshProUGUI lineTextLeft;
    public GameObject dialoguePanelRight;
    public TextMeshProUGUI speakerTextRight;
    public TextMeshProUGUI lineTextRight;

    public AudioSource blipSource;
    public AudioClip blipClip;
    public float blipMaxDurationPerLine = 2.5f;

    bool hasStartedBlip;
    Coroutine blipCapRoutine;

    public bool IsActive { get; private set; }

    void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
        dialoguePanelLeft.SetActive(false);
        dialoguePanelRight.SetActive(false);
    }

    void StartBlipForLine()
    {
        if (blipSource == null || blipClip == null) return;

        if (blipSource.clip != blipClip) blipSource.clip = blipClip;

        if (!hasStartedBlip)
        {
            blipSource.Play();
            hasStartedBlip = true;
        }
        else
        {
            blipSource.UnPause();
        }

        if (blipCapRoutine != null) StopCoroutine(blipCapRoutine);
        blipCapRoutine = StartCoroutine(AutoPauseBlipAfterDelay(blipMaxDurationPerLine));
    }

    void StopBlipForLine()
    {
        if (blipCapRoutine != null) { StopCoroutine(blipCapRoutine); blipCapRoutine = null; }
        if (blipSource != null && blipSource.isPlaying) blipSource.Pause();
    }

    IEnumerator AutoPauseBlipAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (blipSource != null && blipSource.isPlaying) blipSource.Pause();
    }

    void ResetBlip()
    {
        hasStartedBlip = false;
        if (blipCapRoutine != null) { StopCoroutine(blipCapRoutine); blipCapRoutine = null; }
        if (blipSource != null) blipSource.Stop();
    }

    public void StartDialogue(string speaker, string[] lines, Action onComplete = null, bool playBlip = true)
    {
        if (IsActive) return;
        StartCoroutine(PlayLines(speaker, lines, onComplete, playBlip));
    }

    IEnumerator PlayLines(string speaker, string[] lines, Action onComplete, bool playBlip)
    {
        IsActive = true;
        playerMovementScript.enabled = false;
        firstPersonLookScript.enabled = false;
        dialoguePanel.SetActive(true);
        ResetBlip();

        foreach (string line in lines)
        {
            speakerText.text = speaker;
            speakerText.gameObject.SetActive(!string.IsNullOrEmpty(speaker));
            lineText.text = line;

            if (playBlip) StartBlipForLine();

            yield return new WaitForSeconds(0.15f);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space));

            if (playBlip) StopBlipForLine();
        }

        dialoguePanel.SetActive(false);
        playerMovementScript.enabled = true;
        firstPersonLookScript.enabled = true;
        IsActive = false;
        onComplete?.Invoke();
    }

    public void StartAlternatingDialogue(string speakerA, string speakerB, string[] lines, Action onComplete = null)
    {
        if (IsActive) return;
        StartCoroutine(PlayAlternatingLines(speakerA, speakerB, lines, onComplete));
    }

    IEnumerator PlayAlternatingLines(string speakerA, string speakerB, string[] lines, Action onComplete)
    {
        IsActive = true;
        playerMovementScript.enabled = false;
        firstPersonLookScript.enabled = false;
        ResetBlip();

        for (int i = 0; i < lines.Length; i++)
        {
            bool isA = i % 2 == 0;

            dialoguePanelLeft.SetActive(isA);
            dialoguePanelRight.SetActive(!isA);

            if (isA)
            {
                speakerTextLeft.text = speakerA;
                speakerTextLeft.gameObject.SetActive(!string.IsNullOrEmpty(speakerA));
                lineTextLeft.text = lines[i];
            }
            else
            {
                speakerTextRight.text = speakerB;
                speakerTextRight.gameObject.SetActive(!string.IsNullOrEmpty(speakerB));
                lineTextRight.text = lines[i];
            }

            StartBlipForLine();

            yield return new WaitForSeconds(0.15f);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space));

            StopBlipForLine();
        }

        dialoguePanelLeft.SetActive(false);
        dialoguePanelRight.SetActive(false);
        playerMovementScript.enabled = true;
        firstPersonLookScript.enabled = true;
        IsActive = false;
        onComplete?.Invoke();
    }
}