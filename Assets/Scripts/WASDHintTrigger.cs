using UnityEngine;

public class WASDHintTrigger : MonoBehaviour
{
    void Start()
    {
        TutorialHintUI.Instance.ShowHint("wasd_hint", "Press WASD to move around");
    }
}