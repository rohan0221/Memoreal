using System;
using System.Collections.Generic;
using UnityEngine;

public class MemoryManager : MonoBehaviour
{
    public static MemoryManager Instance;
    public bool touchUnlocked, hearingUnlocked, smellUnlocked, tasteUnlocked, guiltRevealed;
    public bool wheelchairActive;
    public bool mirrorCheckedToday;
    public event Action OnStateChanged;
    public int currentDay = 1;
    public float distanceTravelled;
    public int attemptsToday;
    HashSet<string> shownHints = new HashSet<string>();

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool HasShownHint(string key) => shownHints.Contains(key);
    public void MarkHintShown(string key) => shownHints.Add(key);

    public void Unlock(ref bool flag)
    {
        flag = true;
        OnStateChanged?.Invoke();
    }

    public void UnlockByFlag(Recolourable.MemoryFlag flag)
    {
        switch (flag)
        {
            case Recolourable.MemoryFlag.Touch: Unlock(ref touchUnlocked); break;
            case Recolourable.MemoryFlag.Hearing: Unlock(ref hearingUnlocked); break;
            case Recolourable.MemoryFlag.Smell: Unlock(ref smellUnlocked); break;
            case Recolourable.MemoryFlag.Taste: Unlock(ref tasteUnlocked); break;
            case Recolourable.MemoryFlag.GuiltTwist: Unlock(ref guiltRevealed); break;
        }
    }
    public bool IsFlagUnlocked(Recolourable.MemoryFlag flag)
    {
        switch (flag)
        {
            case Recolourable.MemoryFlag.Touch: return touchUnlocked;
            case Recolourable.MemoryFlag.Hearing: return hearingUnlocked;
            case Recolourable.MemoryFlag.Smell: return smellUnlocked;
            case Recolourable.MemoryFlag.Taste: return tasteUnlocked;
            case Recolourable.MemoryFlag.GuiltTwist: return guiltRevealed;
            default: return false;
        }
    }
}