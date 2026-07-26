using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MirrorViewController : MonoBehaviour
{
    public GameObject viewCanvas;
    public RectTransform imageRect;
    public Image displayImage;

    public Sprite[] eyesOnlyFrames;  // default, nothing unlocked yet
    public Sprite[] touchFrames;     // hands revealed
    public Sprite[] hearingFrames;   // ears revealed
    public Sprite[] smellFrames;     // nose revealed
    public Sprite[] tasteFrames;     // mouth revealed

    public float jitterInterval = 0.15f;
    public float maxParallaxOffset = 40f;
    public GameObject captionRoot;
    public TextMeshProUGUI captionText;

    Sprite[] activeFrames;
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

    Sprite[] GetCurrentStageFrames()
    {
        var m = MemoryManager.Instance;
        if (m.tasteUnlocked) return tasteFrames;
        if (m.smellUnlocked) return smellFrames;
        if (m.hearingUnlocked) return hearingFrames;
        if (m.touchUnlocked) return touchFrames;
        return eyesOnlyFrames;
    }

    public void StartViewing()
    {
        isViewing = true;
        viewCanvas.SetActive(true);
        activeFrames = GetCurrentStageFrames();
        frameIndex = 0;
        jitterTimer = 0f;
        accumulatedOffset = Vector2.zero;
        displayImage.sprite = activeFrames[0];
    }

    public bool IsViewing => isViewing;

    void Update()
    {
        if (!isViewing) return;

        jitterTimer += Time.deltaTime;
        if (jitterTimer >= jitterInterval)
        {
            jitterTimer = 0f;
            frameIndex = (frameIndex + 1) % activeFrames.Length;
            displayImage.sprite = activeFrames[frameIndex];
        }

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        accumulatedOffset += new Vector2(-mouseX, -mouseY) * 10f;
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