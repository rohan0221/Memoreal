using System;
using System.Collections;
using UnityEngine;

public class SkillCheckController : MonoBehaviour
{
    public static SkillCheckController Instance;

    public GameObject skillCheckUI;
    public RectTransform handOuter;
    public RectTransform handInner;
    public RectTransform indicatorPivot;   // rotates around centre
    public RectTransform indicatorMarker;  // the visible dot, child of indicatorPivot
    public GameObject eyeCloseOverlay;

    public float baseSpeed = 90f;
    public float tolerance = 6f;

    Action<bool> onComplete;
    bool active;
    int phase;
    float currentAngle;
    float targetAngleA, targetAngleB;
    float speed;

    void Awake()
    {
        Instance = this;
        skillCheckUI.SetActive(false);
    }

    public void StartCheck(Action<bool> callback, float speedMultiplier)
    {
        onComplete = callback;
        speed = baseSpeed * speedMultiplier;

        targetAngleA = UnityEngine.Random.Range(0f, 360f);
        targetAngleB = (targetAngleA + 180f) % 360f;

        phase = 0;
        currentAngle = 0f;
        active = true;

        skillCheckUI.SetActive(true);
        handOuter.gameObject.SetActive(true);
        handInner.gameObject.SetActive(false);
        eyeCloseOverlay.SetActive(false);

        PositionIndicator(targetAngleA, handOuter.rect.height / 2f);
    }

    void PositionIndicator(float angle, float radius)
    {
        indicatorPivot.localEulerAngles = new Vector3(0, 0, -angle);
        indicatorMarker.anchoredPosition = new Vector2(0, radius);
    }

    void Update()
    {
        if (!active) return;

        currentAngle = (currentAngle + speed * Time.deltaTime) % 360f;
        var hand = phase == 0 ? handOuter : handInner;
        hand.localEulerAngles = new Vector3(0, 0, -currentAngle);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float target = phase == 0 ? targetAngleA : targetAngleB;
            float diff = Mathf.Abs(Mathf.DeltaAngle(currentAngle, target));

            if (diff <= tolerance) HandleHit();
            else HandleFail();
        }
    }

    void HandleHit()
    {
        active = false;

        if (phase == 0)
        {
            DayCycleManager.Instance.OnPartialSuccess();
            phase = 1;
            currentAngle = 0f;
            handInner.gameObject.SetActive(true);
            PositionIndicator(targetAngleB, handInner.rect.height / 2f);
            Invoke(nameof(ResumeSecondPhase), 0.6f);
        }
        else
        {
            StartCoroutine(SuccessSequence());
        }
    }

    void ResumeSecondPhase() => active = true;

    void HandleFail()
    {
        active = false;
        skillCheckUI.SetActive(false);
        onComplete?.Invoke(false);
    }

    IEnumerator SuccessSequence()
    {
        float t = 0f, duration = 0.5f;
        float startOuter = handOuter.localEulerAngles.z;
        float startInner = handInner.localEulerAngles.z;

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;
            handOuter.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(startOuter, 90f, lerp));
            handInner.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(startInner, 270f, lerp));
            yield return null;
        }

        eyeCloseOverlay.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        skillCheckUI.SetActive(false);
        onComplete?.Invoke(true);
    }
}