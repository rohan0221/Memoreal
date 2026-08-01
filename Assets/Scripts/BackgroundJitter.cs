using UnityEngine;
using UnityEngine.UI;

public class BackgroundJitter : MonoBehaviour
{
    public Image backgroundImage;
    public Sprite[] frames;
    public float jitterInterval = 0.15f;

    float timer;
    int frameIndex;

    void Update()
    {
        if (frames == null || frames.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= jitterInterval)
        {
            timer = 0f;
            frameIndex = (frameIndex + 1) % frames.Length;
            backgroundImage.sprite = frames[frameIndex];
        }
    }
}