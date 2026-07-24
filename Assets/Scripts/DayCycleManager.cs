using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayCycleManager : MonoBehaviour
{
    public static DayCycleManager Instance;

    public float vignetteStartDistance = 20f; // start darkening after this much movement
    public float blackoutDistance = 30f;      // full black + skill check triggers here

    public Image vignetteOverlay;   // full-screen black Image, CanvasGroup or Image alpha
    public GameObject skillCheckUI;
    public TextMeshProUGUI dayText;
    public MonoBehaviour playerMovementScript;

    float distanceTravelled;
    int currentDay = 1;
    bool inBlackout;

    void Awake()
    {
        Instance = this;
    }

    public void AddDistance(float amount)
    {
        if (inBlackout) return; // don't keep accumulating while skill check is active

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
        skillCheckUI.SetActive(true);
        SkillCheckController.Instance.StartCheck(OnSkillCheckResult);
    }

    void OnSkillCheckResult(bool passed)
    {
        skillCheckUI.SetActive(false);
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

    void EndDay()
    {
        currentDay++;
        distanceTravelled = 0f;
        SetVignetteAlpha(0f);
        dayText.text = "DAY " + currentDay;
        dayText.gameObject.SetActive(true);
        Invoke(nameof(HideDayTextAndResume), 2f);
    }

    void HideDayTextAndResume()
    {
        dayText.gameObject.SetActive(false);
        playerMovementScript.enabled = true;
    }
}