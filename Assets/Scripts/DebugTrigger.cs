using UnityEngine;

public class DebugTrigger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            MemoryManager.Instance.Unlock(ref MemoryManager.Instance.touchUnlocked);
        }
    }
}