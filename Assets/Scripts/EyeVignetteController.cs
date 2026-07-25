using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EyeVignetteController : MonoBehaviour
{
    public Image vignetteImage;
    public Sprite[] frames; // frame[0] = fully closed (black), last frame = fully open
    float currentProgress = 0f; // 0 = closed, 1 = open

    public IEnumerator AnimateTo(float target, float duration)
    {
        float start = currentProgress;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            SetProgress(Mathf.Lerp(start, target, t / duration));
            yield return null;
        }
        SetProgress(target);
    }

    void SetProgress(float t)
    {
        currentProgress = Mathf.Clamp01(t);
        float scaled = currentProgress * (frames.Length - 1);
        int index = Mathf.RoundToInt(scaled);
        vignetteImage.sprite = frames[index];
    }
}