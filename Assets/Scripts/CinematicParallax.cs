using UnityEngine;

public class CinematicParallax : MonoBehaviour
{
    public RectTransform imageRect; // should be sized larger than the screen
    public float maxOffset = 40f;
    Vector2 basePos;

    void Awake()
    {
        basePos = imageRect.anchoredPosition;
    }

    void Update()
    {
        Vector2 mouseNorm = new Vector2(
            (Input.mousePosition.x / Screen.width) - 0.5f,
            (Input.mousePosition.y / Screen.height) - 0.5f
        );
        imageRect.anchoredPosition = basePos - mouseNorm * 2f * maxOffset;
    }
}