using UnityEngine;

public class TrayVisibility : MonoBehaviour
{
    public int visibleOnDay = 5;

    void Start()
    {
        bool shouldShow = MemoryManager.Instance.currentDay == visibleOnDay;
        gameObject.SetActive(shouldShow);
    }
}