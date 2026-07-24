using System;
using UnityEngine;

public class MemoryManager : MonoBehaviour
{
    public static MemoryManager Instance;
    public bool touchUnlocked, hearingUnlocked, smellUnlocked, tasteUnlocked, guiltRevealed;
    public bool wheelchairActive;

    public event Action OnStateChanged;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

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
}