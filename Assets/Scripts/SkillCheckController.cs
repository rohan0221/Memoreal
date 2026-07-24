using System;
using System.Collections;
using UnityEngine;

public class SkillCheckController : MonoBehaviour
{
    public static SkillCheckController Instance;

    public GameObject skillCheckUI;       // parent object containing everything below
    public RectTransform handOuter;
    public RectTransform handInner;
    public RectTransform indicatorMarker; // reused for both phases, repositioned by rotation
    public GameObject eyeCloseOverlay;

    public float baseSpeed = 90f;  // degrees per second
    public float tolerance = 6f;   // degrees of forgiveness

    Action<bool> onComplete;
    bool active;
    int phase; // 0 = outer hand, 1 = inner hand
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
        PositionIndicator(targetAngleA);
    }

    void PositionIndicator(float angle)
    {
        indicatorMarker.localEulerAngles = new Vector3(0, 0, -angle);
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
            PositionIndicator(targetAngleB);
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
            handOuter.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(startOuter, 0f, lerp));
            handInner.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(startInner, 180f, lerp));
            yield return null;
        }

        eyeCloseOverlay.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        skillCheckUI.SetActive(false);
        onComplete?.Invoke(true);
    }
}