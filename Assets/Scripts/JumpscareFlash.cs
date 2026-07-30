using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JumpscareFlash : MonoBehaviour
{
    public static JumpscareFlash Instance;
    public Image flashImage;
    public float holdDuration = 0.4f;
    public float jitterInterval = 0.08f;

    void Awake()
    {
        Instance = this;
        flashImage.enabled = false;
    }

    public void Play(Sprite[] jumpscareFrames, Action onComplete)
    {
        StartCoroutine(FlashSequence(jumpscareFrames, onComplete));
    }

    IEnumerator FlashSequence(Sprite[] frames, Action onComplete)
    {
        flashImage.enabled = true;

        float elapsed = 0f;
        int index = 0;
        while (elapsed < holdDuration)
        {
            flashImage.sprite = frames[index % frames.Length];
            index++;
            yield return new WaitForSeconds(jitterInterval);
            elapsed += jitterInterval;
        }

        flashImage.enabled = false;
        onComplete?.Invoke();
    }
}