using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; 

public class DayCycleManager : MonoBehaviour
{
    public static DayCycleManager Instance;

    public float vignetteStartDistance = 20f;
    public float blackoutDistance = 30f;

    public Image vignetteOverlay;
    public TextMeshProUGUI dayText;
    public MonoBehaviour playerMovementScript;

    float distanceTravelled;
    int currentDay = 1;
    int attemptsToday;
    bool inBlackout;

    void Awake()
    {
        Instance = this;
    }

    public int GetCurrentDay() => currentDay;

    public bool IsDayObjectiveComplete()
    {
        var m = MemoryManager.Instance;
        switch (currentDay)
        {
            case 1: return m.mirrorSeen;
            case 2: return m.touchUnlocked;
            case 3: return m.hearingUnlocked;
            case 6: return m.guiltRevealed;
            default: return true; // Day 4/5/7 end via their own scripted events, not this gate
        }
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
        if (!IsDayObjectiveComplete())
        {
            distanceTravelled = blackoutDistance - 1f; // stops it retriggering every frame
            DialogueManager.Instance.StartDialogue("", new string[] { "You're not tired yet — there's something you still need to do." });
            return;
        }

        inBlackout = true;
        SetVignetteAlpha(1f);
        playerMovementScript.enabled = false;

        float multiplier = 1f + (currentDay - 1) * 0.15f + attemptsToday * 0.1f;
        attemptsToday++;

        SkillCheckController.Instance.StartCheck(OnSkillCheckResult, multiplier);
    }

    public void OnPartialSuccess()
    {
        StartCoroutine(FlashRelief());
    }

    IEnumerator FlashRelief()
    {
        yield return StartCoroutine(FadeVignette(1f, 0.6f, 0.4f));
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(FadeVignette(0.6f, 1f, 0.4f));
    }

    IEnumerator FadeVignette(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            SetVignetteAlpha(Mathf.Lerp(from, to, t / duration));
            yield return null;
        }
        SetVignetteAlpha(to);
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
        dayText.text = "DAY " + currentDay;
        dayText.gameObject.SetActive(true);
        Invoke(nameof(GoToBedroom), 2f);
    }

    void GoToBedroom()
    {
        dayText.gameObject.SetActive(false);
        SceneTransitionManager.Instance.TransitionTo("HospitalRoom", "BedSpawnPoint");
    }
}