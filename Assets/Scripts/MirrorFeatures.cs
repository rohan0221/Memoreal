using UnityEngine;

public class MirrorFeatures : MonoBehaviour
{
    public GameObject eyes;   // always active from the start
    public GameObject mouth;  // Taste
    public GameObject nose;   // pick a flag — e.g. Smell
    public GameObject ears;   // Hearing

    void Awake()
    {
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
        var m = MemoryManager.Instance;
        eyes.SetActive(true);
        mouth.SetActive(m.tasteUnlocked);
        nose.SetActive(m.smellUnlocked);
        ears.SetActive(m.hearingUnlocked);
    }
}