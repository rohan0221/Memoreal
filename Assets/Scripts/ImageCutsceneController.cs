using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageCutsceneController : MonoBehaviour
{
    public static ImageCutsceneController Instance;
    public bool IsActive { get; private set; }

    public Image contentImage;
    public CinematicParallax parallaxScript;
    public MonoBehaviour playerMovementScript;
    public MonoBehaviour firstPersonLookScript;

    Action onComplete;
    Sprite[] currentFrames;
    float jitterInterval;
    Coroutine jitterRoutine;

    void Awake() { Instance = this; }

    public void PlayCutscene(Sprite[] frames, float jitterInt, Action onCompleteCallback)
    {
        if (IsActive) return;
        IsActive = true;
        currentFrames = frames;
        jitterInterval = jitterInt;
        onComplete = onCompleteCallback;
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        playerMovementScript.enabled = false;
        firstPersonLookScript.enabled = false;

        var vignette = SceneTransitionManager.Instance.eyeVignette;

        yield return StartCoroutine(vignette.PlayClose());

        contentImage.sprite = currentFrames[0];
        jitterRoutine = StartCoroutine(JitterLoop());
        parallaxScript.enabled = true;

        yield return StartCoroutine(vignette.PlayOpen());

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space));

        yield return StartCoroutine(vignette.PlayClose());

        if (jitterRoutine != null) StopCoroutine(jitterRoutine);
        parallaxScript.enabled = false;

        firstPersonLookScript.enabled = true;
        playerMovementScript.enabled = true;

        yield return StartCoroutine(vignette.PlayOpen());

        IsActive = false;
        onComplete?.Invoke();
    }

    IEnumerator JitterLoop()
    {
        int index = 0;
        while (true)
        {
            index = (index + 1) % currentFrames.Length;
            contentImage.sprite = currentFrames[index];
            yield return new WaitForSeconds(jitterInterval);
        }
    }
}