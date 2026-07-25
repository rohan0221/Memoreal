using UnityEngine;
using UnityEngine.UI;

public class MirrorViewController : MonoBehaviour
{
    public GameObject viewCanvas; // parent object containing the image, toggled on/off
    public RectTransform imageRect;
    public Image displayImage;
    public Sprite[] frames; // 2 frames to alternate between
    public float jitterInterval = 0.15f;
    public float maxParallaxOffset = 40f;

    Vector2 basePosition;
    float jitterTimer;
    int frameIndex;
    bool isViewing;

    void Awake()
    {
        basePosition = imageRect.anchoredPosition;
        viewCanvas.SetActive(false);
    }

    public void StartViewing()
    {
        isViewing = true;
        viewCanvas.SetActive(true);
        frameIndex = 0;
        jitterTimer = 0f;
        displayImage.sprite = frames[0];
    }

    public void StopViewing()
    {
        isViewing = false;
        viewCanvas.SetActive(false);
    }

    public bool IsViewing => isViewing;

    void Update()
    {
        if (!isViewing) return;

        jitterTimer += Time.deltaTime;
        if (jitterTimer >= jitterInterval)
        {
            jitterTimer = 0f;
            frameIndex = (frameIndex + 1) % frames.Length;
            displayImage.sprite = frames[frameIndex];
        }

        Vector2 mouseNorm = new Vector2(
            (Input.mousePosition.x / Screen.width) - 0.5f,
            (Input.mousePosition.y / Screen.height) - 0.5f
        );
        imageRect.anchoredPosition = basePosition - mouseNorm * 2f * maxParallaxOffset;
    }
}