using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; 
using UnityEngine.SceneManagement;

public class DayCycleManager : MonoBehaviour
{
    public static DayCycleManager Instance;

    public float vignetteStartDistance = 20f;
    public float blackoutDistance = 30f;

    public Image vignetteOverlay;
    public TextMeshProUGUI dayText;
    public MonoBehaviour playerMovementScript;
    public MonoBehaviour firstPersonLookScript; 
    public bool IsBusy => isEndingDay || inBlackout;
    bool inBlackout;
    bool isEndingDay;
    const int guiltRevealDay = 6;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Re-sync the visual vignette to match persisted progress after a scene reload
        float t = Mathf.InverseLerp(vignetteStartDistance, blackoutDistance, MemoryManager.Instance.distanceTravelled);
        SetVignetteAlpha(IsDayObjectiveComplete() ? t : 0f);

        if (MemoryManager.Instance.currentDay == 1 && !MemoryManager.Instance.HasShownHint("day1_countdown_intro"))
        {
            MemoryManager.Instance.MarkHintShown("day1_countdown_intro");
            StartCoroutine(ShowCountdownBriefly());
        }
    }

    string GetCountdownLabel(int day)
    {
        int daysLeft = guiltRevealDay - day;
        if (daysLeft < 0) return "You're out of time.";
        if (daysLeft == 1) return "1 DAY LEFT";
        if (daysLeft == 0) return "0 DAYS LEFT";
        return daysLeft + " DAYS LEFT";
    }
    
    IEnumerator ShowCountdownBriefly()
    {
        dayText.text = GetCountdownLabel(1);
        dayText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        dayText.gameObject.SetActive(false);
    }

    public int GetCurrentDay() => MemoryManager.Instance.currentDay;

    public bool IsDayObjectiveComplete()
    {
        var m = MemoryManager.Instance;
        switch (m.currentDay)
        {
            case 1: return m.mirrorCheckedToday;
            case 2: return m.mirrorCheckedToday && m.touchUnlocked;
            case 3: return m.mirrorCheckedToday && m.hearingUnlocked;
            case 4: return m.smellUnlocked;
            case 5: return m.mirrorCheckedToday && m.tasteUnlocked;
            case 6: return m.mirrorCheckedToday && m.guiltRevealed;
            default: return true;
        }
    }

    public void AddDistance(float amount)
    {
        if (inBlackout) return;
        if (!IsDayObjectiveComplete()) return;

        MemoryManager.Instance.distanceTravelled += amount;

        float t = Mathf.InverseLerp(vignetteStartDistance, blackoutDistance, MemoryManager.Instance.distanceTravelled);
        SetVignetteAlpha(t);

        if (t > 0.05f)
        {
            TutorialHintUI.Instance.ShowHint("step_limit_warning", "You have a limited amount of steps per day, use them wisely.");
        }

        if (MemoryManager.Instance.distanceTravelled >= blackoutDistance)
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
        firstPersonLookScript.enabled = false;

        if (!MemoryManager.Instance.HasShownHint("skill_check_explain"))
        {
            MemoryManager.Instance.MarkHintShown("skill_check_explain");
            DialogueManager.Instance.StartDialogue("", new string[] {
                "Press space to land the clock arms on the eye indicators to extend your day's steps.",
                "Some content will only be available in later days."
            }, BeginSkillCheck);
        }
        else
        {
            BeginSkillCheck();
        }
    }
    
    void BeginSkillCheck()
    {
        playerMovementScript.enabled = false;
        firstPersonLookScript.enabled = false;

        float multiplier = 1f + (MemoryManager.Instance.currentDay - 1) * 0.15f + MemoryManager.Instance.attemptsToday * 0.1f;
        MemoryManager.Instance.attemptsToday++;
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
            MemoryManager.Instance.distanceTravelled = 0f;
            SetVignetteAlpha(0f);
            playerMovementScript.enabled = true;
            firstPersonLookScript.enabled = true;
        }
        else
        {
            DialogueManager.Instance.StartDialogue("", new string[] { "You passed out..." }, EndDay);
        }
    }


    public void EndDay()
    {
        if (isEndingDay) return;
        isEndingDay = true;

        playerMovementScript.enabled = false;
        firstPersonLookScript.enabled = false;
        StartCoroutine(EndDaySequence());
    }

    public IEnumerator FadeToBlack(float duration)
    {
        yield return StartCoroutine(FadeVignette(vignetteOverlay.color.a, 1f, duration));
    }

    public void EndGame()
    {
        StartCoroutine(EndGameSequence());
    }

    IEnumerator EndGameSequence()
    {
        yield return StartCoroutine(FadeToBlack(0.6f));

        MemoryManager.Instance.currentDay++;
        dayText.text = GetCountdownLabel(MemoryManager.Instance.currentDay);
        dayText.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator EndDaySequence()
    {
        yield return StartCoroutine(FadeVignette(vignetteOverlay.color.a, 1f, 0.4f));

        MemoryManager.Instance.currentDay++;
        MemoryManager.Instance.attemptsToday = 0;
        MemoryManager.Instance.distanceTravelled = 0f;
        MemoryManager.Instance.mirrorCheckedToday = false;

        dayText.text = GetCountdownLabel(MemoryManager.Instance.currentDay);
        dayText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        dayText.gameObject.SetActive(false);
        isEndingDay = false;
        SceneTransitionManager.Instance.TransitionTo("HospitalRoom", "BedSpawnPoint");
    }
}