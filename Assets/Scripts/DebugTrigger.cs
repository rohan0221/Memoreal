using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugTrigger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            MemoryManager.Instance.Unlock(ref MemoryManager.Instance.touchUnlocked);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene("TestRoom2");
        }
    }
}