using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialHintUI : MonoBehaviour
{
    public static TutorialHintUI Instance;
    public GameObject hintRoot;
    public TextMeshProUGUI hintText;
    public CanvasGroup canvasGroup;
    public MonoBehaviour playerMovementScript;
    public MonoBehaviour firstPersonLookScript;
    public float displayDuration = 3f;
    public float fadeDuration = 0.3f;
    public float inputFreezeDuration = 1f;
    Coroutine activeRoutine;

    void Awake()
    {
        Instance = this;
        hintRoot.SetActive(false);
    }

    public void ShowHint(string key, string text)
    {
        if (MemoryManager.Instance.HasShownHint(key)) return;
        MemoryManager.Instance.MarkHintShown(key);

        if (activeRoutine != null) StopCoroutine(activeRoutine);
        activeRoutine = StartCoroutine(DisplaySequence(text));
    }

    IEnumerator DisplaySequence(string text)
    {
        hintRoot.SetActive(true);
        hintText.text = text;
        canvasGroup.alpha = 0f;

        playerMovementScript.enabled = false;
        firstPersonLookScript.enabled = false;

        yield return StartCoroutine(Fade(0f, 1f));

        yield return new WaitForSeconds(inputFreezeDuration);

        playerMovementScript.enabled = true;
        firstPersonLookScript.enabled = true;

        yield return new WaitForSeconds(Mathf.Max(0f, displayDuration - inputFreezeDuration));
        yield return StartCoroutine(Fade(1f, 0f));

        hintRoot.SetActive(false);
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}