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
    bool inBlackout;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // TEMPORARY debug stand-in for the skill check — remove once real skill check exists
        if (inBlackout)
        {
            if (Input.GetKeyDown(KeyCode.P)) OnSkillCheckResult(true);
            if (Input.GetKeyDown(KeyCode.F)) OnSkillCheckResult(false);
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
        inBlackout = true;
        SetVignetteAlpha(1f);
        playerMovementScript.enabled = false;
        Debug.Log("Blackout! Press P to pass, F to fail (debug stand-in for skill check)");
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

    public void EndDay() // changed from private to public
    {
        playerMovementScript.enabled = false;
        currentDay++;
        distanceTravelled = 0f;
        SetVignetteAlpha(0f); // or vignette.SetProgress(0f) once you swap in the new system
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
            if (cc != null) cc.enabled = false; // must disable CC before moving its transform directly
            playerMovementScript.transform.position = bedSpawnPoint.position;
            playerMovementScript.transform.rotation = bedSpawnPoint.rotation;
            if (cc != null) cc.enabled = true;
        }

        playerMovementScript.enabled = true;
    }
}
