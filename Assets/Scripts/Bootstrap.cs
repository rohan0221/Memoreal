using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    public string sceneToLoad = "TestRoom";

    void Start()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}