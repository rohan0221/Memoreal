using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugTrigger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            MemoryManager.Instance.Unlock(ref MemoryManager.Instance.touchUnlocked);

        if (Input.GetKeyDown(KeyCode.Y))
            MemoryManager.Instance.Unlock(ref MemoryManager.Instance.tasteUnlocked);

        if (Input.GetKeyDown(KeyCode.U))
            MemoryManager.Instance.Unlock(ref MemoryManager.Instance.hearingUnlocked);

        if (Input.GetKeyDown(KeyCode.I))
            MemoryManager.Instance.Unlock(ref MemoryManager.Instance.smellUnlocked);

        if (Input.GetKeyDown(KeyCode.N))
            SceneManager.LoadScene("TestRoom2");
    }
}