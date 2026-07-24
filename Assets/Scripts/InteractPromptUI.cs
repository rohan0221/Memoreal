using System.Collections;
using UnityEngine;

public class InteractPromptUI : MonoBehaviour
{
    public static InteractPromptUI Instance;
    public RectTransform promptRoot;
    public CanvasGroup canvasGroup;
    public float bobAmplitude = 8f;
    public float bobSpeed = 3f;
    public float fadeDuration = 0.1f; // fast, per your request

    Vector2 basePosition;
    Coroutine bobRoutine;
    Coroutine fadeRoutine;

    void Awake()
    {
        Instance = this;
        basePosition = promptRoot.anchoredPosition;
        canvasGroup.alpha = 0f;
        promptRoot.gameObject.SetActive(false);
    }

    public void Show()
    {
        promptRoot.gameObject.SetActive(true);
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(Fade(1f));
        if (bobRoutine == null) bobRoutine = StartCoroutine(Bob());
    }

    public void Hide()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(Fade(0f));
    }

    IEnumerator Fade(float target)
    {
        float start = canvasGroup.alpha;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, target, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = target;

        if (target <= 0f)
        {
            if (bobRoutine != null) { StopCoroutine(bobRoutine); bobRoutine = null; }
            promptRoot.anchoredPosition = basePosition;
            promptRoot.gameObject.SetActive(false);
        }
    }

    IEnumerator Bob()
    {
        while (true)
        {
            float offset = Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
            promptRoot.anchoredPosition = basePosition + new Vector2(0, offset);
            yield return null;
        }
    }
}