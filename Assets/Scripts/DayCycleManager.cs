using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayCycleManager : MonoBehaviour
{
    public static DayCycleManager Instance;

    public float vignetteStartDistance = 20f;
    public float blackoutDistance = 30f;

    public Image vignetteOverlay;
    public TextMeshProUGUI dayText;
    public MonoBehaviour playerMovementScript;
    public Transform bedSpawnPoint;

    float distanceTravelled;
    int currentDay = 1;
    int attemptsToday;
    bool inBlackout;

    void Awake()
    {
        Instance = this;
    }

    public void AddDistance(float amount)
    {
        if (inBlackout) return;

        distanceTravelled += amount;

        float t = Mathf.InverseLerp(vignetteStartDistance, blackoutDistance, distanceTravelled);
        SetVignetteAlpha(t);

        if (distanceTravelled >= blackoutDistance)
        {
            TriggerBlackout();
        }
    }

    void SetVignetteAlpha(float alpha)
    {
        var c = vignetteOverlay.color;
        c.a = Mathf.Clamp01(alpha);
        vignetteOverlay.color = c;
    }

    void TriggerBlackout()
    {
        inBlackout = true;
        SetVignetteAlpha(1f);
        playerMovementScript.enabled = false;

        float multiplier = 1f + (currentDay - 1) * 0.15f + attemptsToday * 0.1f;
        attemptsToday++;

        SkillCheckController.Instance.StartCheck(OnSkillCheckResult, multiplier);
    }

    public void OnPartialSuccess()
    {
        SetVignetteAlpha(0.6f); // brief relief flash between the two hands
    }

    void OnSkillCheckResult(bool passed)
    {
        inBlackout = false;

        if (passed)
        {
            distanceTravelled = 0f;
            SetVignetteAlpha(0f);
            playerMovementScript.enabled = true;
        }
        else
        {
            EndDay();
        }
    }

    public void EndDay()
    {
        playerMovementScript.enabled = false;
        currentDay++;
        attemptsToday = 0;
        distanceTravelled = 0f;
        SetVignetteAlpha(0f);
        dayText.text = "DAY " + currentDay;
        dayText.gameObject.SetActive(true);
        Invoke(nameof(HideDayTextAndResume), 2f);
    }

    void HideDayTextAndResume()
    {
        dayText.gameObject.SetActive(false);

        if (bedSpawnPoint != null)
        {
            var cc = playerMovementScript.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            playerMovementScript.transform.position = bedSpawnPoint.position;
            playerMovementScript.transform.rotation = bedSpawnPoint.rotation;
            if (cc != null) cc.enabled = true;
        }

        playerMovementScript.enabled = true;
    }
}