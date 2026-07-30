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

    public void PlayMemory(Transform viewPoint, Sprite[] frames, float jitterInt, Action onCompleteCallback)
    {
        if (IsActive) return;
        IsActive = true;
        onComplete = onCompleteCallback;
        currentFrames = frames;
        jitterInterval = jitterInt;
        StartCoroutine(PlaySequence(viewPoint));
    }

    IEnumerator PlaySequence(Transform viewPoint)
    {
        playerMovementScript.enabled = false;
        firstPersonLookScript.enabled = false;

        Vector3 originalLocalPos = playerCamera.transform.localPosition;
        Quaternion originalLocalRot = playerCamera.transform.localRotation;

        Vector3 startWorldPos = playerCamera.transform.position;
        Quaternion startWorldRot = playerCamera.transform.rotation;

        float t = 0f;
        while (t < cameraMoveDuration)
        {
            t += Time.deltaTime;
            float lerp = t / cameraMoveDuration;
            playerCamera.transform.position = Vector3.Lerp(startWorldPos, viewPoint.position, lerp);
            playerCamera.transform.rotation = Quaternion.Slerp(startWorldRot, viewPoint.rotation, lerp);
            yield return null;
        }
        playerCamera.transform.position = viewPoint.position;
        playerCamera.transform.rotation = viewPoint.rotation;

        memoryImage.sprite = currentFrames[0];
        yield return StartCoroutine(FadeCanvas(memoryImageGroup, 0f, 1f, fadeDuration));

        jitterRoutine = StartCoroutine(JitterLoop());
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space));
        if (jitterRoutine != null) StopCoroutine(jitterRoutine);

        yield return StartCoroutine(FadeCanvas(memoryImageGroup, 1f, 0f, fadeDuration));

        Vector3 returnStartPos = playerCamera.transform.localPosition;
        Quaternion returnStartRot = playerCamera.transform.localRotation;

        t = 0f;
        while (t < cameraMoveDuration)
        {
            t += Time.deltaTime;
            float lerp = t / cameraMoveDuration;
            playerCamera.transform.localPosition = Vector3.Lerp(returnStartPos, originalLocalPos, lerp);
            playerCamera.transform.localRotation = Quaternion.Slerp(returnStartRot, originalLocalRot, lerp);
            yield return null;
        }
        playerCamera.transform.localPosition = originalLocalPos;
        playerCamera.transform.localRotation = originalLocalRot;

        firstPersonLookScript.enabled = true;
        playerMovementScript.enabled = true;
        IsActive = false;

        onComplete?.Invoke();
    }

    public IEnumerator RotateCameraTo(Transform target, float duration)
    {
        playerMovementScript.enabled = false;
        firstPersonLookScript.enabled = false;

        Quaternion startRot = playerCamera.transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation((target.position - playerCamera.transform.position).normalized);

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            playerCamera.transform.rotation = Quaternion.Slerp(startRot, targetRot, t / duration);
            yield return null;
        }
        playerCamera.transform.rotation = targetRot;
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