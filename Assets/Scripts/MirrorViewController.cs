using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MirrorViewController : MonoBehaviour
{
    public GameObject viewCanvas; // parent object containing the image, toggled on/off
    public RectTransform imageRect;
    public Image displayImage;
    public Sprite[] frames; // 2 frames to alternate between
    public float jitterInterval = 0.15f;
    public float maxParallaxOffset = 40f;
    public GameObject captionRoot; // small text object inside MirrorViewCanvas, start disabled
    public TextMeshProUGUI captionText;
    Vector2 basePosition;
    Vector2 accumulatedOffset;
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
        accumulatedOffset = Vector2.zero;
        displayImage.sprite = frames[0];
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

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        accumulatedOffset += new Vector2(-mouseX, -mouseY) * 1f;
        accumulatedOffset = Vector2.ClampMagnitude(accumulatedOffset, maxParallaxOffset);

        imageRect.anchoredPosition = basePosition + accumulatedOffset;
    }

    public void ShowCaptionOnce()
    {
        if (MemoryManager.Instance.HasShownHint("mirror_caption")) return;
        MemoryManager.Instance.MarkHintShown("mirror_caption");
        captionText.text = "Recover your senses as you search for context.";
        captionRoot.SetActive(true);
    }

    public void StopViewing()
    {
        isViewing = false;
        viewCanvas.SetActive(false);
        captionRoot.SetActive(false);
    }
}