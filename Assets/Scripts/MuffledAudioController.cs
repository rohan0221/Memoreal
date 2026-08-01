using System.Collections;
using UnityEngine;

public class MuffledAudioController : MonoBehaviour
{
    public AudioLowPassFilter lowPassFilter;
    public float muffledCutoff = 800f;
    public float normalCutoff = 22000f;
    public float transitionDelay = 0.5f;

    bool hasUnmuffled;

    void Start()
    {
        if (lowPassFilter == null) return;

        if (MemoryManager.Instance.hearingUnlocked)
        {
            lowPassFilter.cutoffFrequency = normalCutoff;
            hasUnmuffled = true;
        }
        else
        {
            lowPassFilter.cutoffFrequency = muffledCutoff;
        }

        MemoryManager.Instance.OnStateChanged += HandleStateChanged;
    }

    void OnDestroy()
    {
        if (MemoryManager.Instance != null)
            MemoryManager.Instance.OnStateChanged -= HandleStateChanged;
    }

    void HandleStateChanged()
    {
        if (hasUnmuffled) return;
        if (!MemoryManager.Instance.hearingUnlocked) return;

        hasUnmuffled = true;
        StartCoroutine(UnmuffleAfterDelay());
    }

    IEnumerator UnmuffleAfterDelay()
    {
        yield return new WaitForSeconds(transitionDelay);
        if (lowPassFilter != null) lowPassFilter.cutoffFrequency = normalCutoff;
    }
}