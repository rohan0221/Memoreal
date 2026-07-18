using UnityEngine;

public class Recolourable : MonoBehaviour
{
    public enum MemoryFlag { Touch, Hearing, Smell, Taste, GuiltTwist }
    public MemoryFlag governingFlag;
    public Material greyMaterial;
    public Material colourMaterial;
    Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        MemoryManager.Instance.OnStateChanged += Refresh;
        Refresh();
    }

    void OnDestroy()
    {
        if (MemoryManager.Instance != null)
            MemoryManager.Instance.OnStateChanged -= Refresh;
    }

    void Refresh()
    {
        rend.material = IsUnlocked() ? colourMaterial : greyMaterial;
    }

    bool IsUnlocked()
    {
        var m = MemoryManager.Instance;
        return governingFlag switch
        {
            MemoryFlag.Touch => m.touchUnlocked,
            MemoryFlag.Hearing => m.hearingUnlocked,
            MemoryFlag.Smell => m.smellUnlocked,
            MemoryFlag.Taste => m.tasteUnlocked,
            MemoryFlag.GuiltTwist => m.guiltRevealed,
            _ => false
        };
    }
}