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
}