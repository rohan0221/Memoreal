using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public EyeVignetteController eyeVignette;
    public float fadeDuration = 0.5f;

    string pendingSpawnPointName;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void TransitionTo(string sceneName, string spawnPointName)
    {
        pendingSpawnPointName = spawnPointName;
        StartCoroutine(DoTransition(sceneName));
    }

    IEnumerator DoTransition(string sceneName)
    {
        yield return StartCoroutine(eyeVignette.PlayClose());

        SceneManager.LoadScene(sceneName);
        eyeVignette.HoldClosed(); // reassert immediately after load, before anything else runs
        yield return null;
        yield return null; // extra frame buffer so the new scene's rendering settles before we open

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject spawnPoint = GameObject.Find(pendingSpawnPointName);

        if (player != null && spawnPoint != null)
        {
            var cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            player.transform.position = spawnPoint.transform.position;
            player.transform.rotation = spawnPoint.transform.rotation;
            if (cc != null) cc.enabled = true;
        }

        yield return StartCoroutine(eyeVignette.PlayOpen());
    }
}