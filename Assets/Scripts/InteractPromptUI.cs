using UnityEngine;

public class InteractPromptUI : MonoBehaviour
{
    public static InteractPromptUI Instance;
    public GameObject promptRoot; // the rounded box + "E" text, as one object

    void Awake()
    {
        Instance = this;
        promptRoot.SetActive(false);
    }

    public void Show()
    {
        promptRoot.SetActive(true);
    }

    public void Hide()
    {
        promptRoot.SetActive(false);
    }
}