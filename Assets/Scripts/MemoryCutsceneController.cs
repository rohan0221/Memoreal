using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MemoryCutsceneController : MonoBehaviour
{
    public static MemoryCutsceneController Instance;
    public bool IsActive { get; private set; }

    public Camera playerCamera;
    public MonoBehaviour playerMovementScript;
    public MonoBehaviour firstPersonLookScript;
    public CanvasGroup memoryImageGroup;
    public Image memoryImage;
    public float cameraMoveDuration = 1f;
    public float fadeDuration = 0.5f;

    Action onComplete;
    Sprite[] currentFrames;
    float jitterInterval;
    Coroutine jitterRoutine;

    void Awake() { Instance = this; }

    public void PlayMemory(Transform focusTarget, Sprite[] frames, float jitterInt, Action onCompleteCallback)
    {
        if (IsActive) return;
        IsActive = true;
        onComplete = onCompleteCallback;
        currentFrames = frames;
        jitterInterval = jitterInt;
        StartCoroutine(PlaySequence(focusTarget));
    }

    IEnumerator PlaySequence(Transform focusTarget)
    {
        playerMovementScript.enabled = false;
        firstPersonLookScript.enabled = false;

        Quaternion startRot = playerCamera.transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation((focusTarget.position - playerCamera.transform.position).normalized);

        float t = 0f;
        while (t < cameraMoveDuration)
        {
            t += Time.deltaTime;
            playerCamera.transform.rotation = Quaternion.Slerp(startRot, targetRot, t / cameraMoveDuration);
            yield return null;
        }
        playerCamera.transform.rotation = targetRot;

        memoryImage.sprite = currentFrames[0];
        yield return StartCoroutine(FadeCanvas(memoryImageGroup, 0f, 1f, fadeDuration));

        jitterRoutine = StartCoroutine(JitterLoop());
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space));
        if (jitterRoutine != null) StopCoroutine(jitterRoutine);

        yield return StartCoroutine(FadeCanvas(memoryImageGroup, 1f, 0f, fadeDuration));

        firstPersonLookScript.enabled = true;
        playerMovementScript.enabled = true;
        IsActive = false;

        onComplete?.Invoke();
    }

    IEnumerator JitterLoop()
    {
        int index = 0;
        while (true)
        {
            index = (index + 1) % currentFrames.Length;
            memoryImage.sprite = currentFrames[index];
            yield return new WaitForSeconds(jitterInterval);
        }
    }

    IEnumerator FadeCanvas(CanvasGroup group, float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        group.alpha = to;
    }
}