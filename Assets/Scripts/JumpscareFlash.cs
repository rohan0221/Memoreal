using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JumpscareFlash : MonoBehaviour
{
    public static JumpscareFlash Instance;
    public Image flashImage;
    public float holdDuration = 0.4f;

    void Awake()
    {
        Instance = this;
        flashImage.enabled = false;
    }

    public void Play(Sprite jumpscareSprite, Action onComplete)
    {
        StartCoroutine(FlashSequence(jumpscareSprite, onComplete));
    }

    IEnumerator FlashSequence(Sprite sprite, Action onComplete)
    {
        flashImage.sprite = sprite;
        flashImage.enabled = true;

        yield return new WaitForSeconds(holdDuration);

        flashImage.enabled = false;
        onComplete?.Invoke();
    }
}