using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CinematicIntroController : MonoBehaviour
{
    public Image contentImage;
    public Sprite blurryImage;
    public Sprite clearImage;
    public CinematicParallax parallaxScript;
    public float gateDuration = 4f;
    public string nextSceneName = "HospitalRoom";
    public string nextSpawnPointName = "BedSpawnPoint";

    IEnumerator Start()
    {
        var vignette = SceneTransitionManager.Instance.eyeVignette;
        parallaxScript.enabled = false;

        contentImage.sprite = blurryImage;
        yield return StartCoroutine(vignette.PlayOpen());
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(vignette.PlayClose());

        contentImage.sprite = clearImage;
        yield return StartCoroutine(vignette.PlayOpen());

        parallaxScript.enabled = true;

        yield return new WaitForSeconds(gateDuration);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space));

        parallaxScript.enabled = false;
        SceneTransitionManager.Instance.TransitionTo(nextSceneName, nextSpawnPointName);
    }
}