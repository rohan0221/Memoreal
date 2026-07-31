using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CinematicIntroController : MonoBehaviour
{
    public Image contentImage;
    public Sprite[] blurryFrames;
    public Sprite[] clearFrames;
    public float jitterInterval = 0.15f;
    public CinematicParallax parallaxScript;
    public float gateDuration = 4f;
    public string nextSceneName = "HospitalRoom";
    public string nextSpawnPointName = "BedSpawnPoint";

    Coroutine jitterRoutine;

    IEnumerator Start()
    {
        var vignette = SceneTransitionManager.Instance.eyeVignette;
        parallaxScript.enabled = false;

        jitterRoutine = StartCoroutine(JitterLoop(blurryFrames));
        yield return StartCoroutine(vignette.PlayOpen());
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(vignette.PlayClose());
        StopCoroutine(jitterRoutine);

        jitterRoutine = StartCoroutine(JitterLoop(clearFrames));
        yield return StartCoroutine(vignette.PlayOpen());

        parallaxScript.enabled = true;

        yield return new WaitForSeconds(gateDuration);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space));

        StopCoroutine(jitterRoutine);
        parallaxScript.enabled = false;
        SceneTransitionManager.Instance.TransitionTo(nextSceneName, nextSpawnPointName);
    }

    IEnumerator JitterLoop(Sprite[] frames)
    {
        int index = 0;
        while (true)
        {
            contentImage.sprite = frames[index % frames.Length];
            index++;
            yield return new WaitForSeconds(jitterInterval);
        }
    }
}