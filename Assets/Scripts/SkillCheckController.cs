using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheckController : MonoBehaviour
{
    public static SkillCheckController Instance;

    public RectTransform movingBar;
    public RectTransform targetZone;
    public float speed = 400f;
    public float trackWidth = 300f; // total horizontal distance the bar travels

    Action<bool> onComplete;
    bool active;
    float pos;
    int direction = 1;

    void Awake()
    {
        Instance = this;
    }

    public void StartCheck(Action<bool> callback)
    {
        onComplete = callback;
        active = true;
        pos = 0f;
        direction = 1;
    }

    void Update()
    {
        if (!active) return;

        pos += speed * direction * Time.deltaTime;
        if (pos >= trackWidth || pos <= 0f)
        {
            direction *= -1;
            pos = Mathf.Clamp(pos, 0f, trackWidth);
        }

        movingBar.anchoredPosition = new Vector2(pos, movingBar.anchoredPosition.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool inZone = pos >= targetZone.anchoredPosition.x - (targetZone.rect.width / 2)
                       && pos <= targetZone.anchoredPosition.x + (targetZone.rect.width / 2);
            active = false;
            onComplete?.Invoke(inZone);
        }
    }
}