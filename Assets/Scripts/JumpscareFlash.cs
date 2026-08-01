using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JumpscareFlash : MonoBehaviour
{
    public static JumpscareFlash Instance;
    public Image flashImage;
    public float jitterInterval = 0.08f;
    public AudioSource stingerSource;
    public AudioClip stingerClip;
    Coroutine loopRoutine;

    void Awake()
    {
        Instance = this;
        flashImage.enabled = false;
    }

    public void StartFlash(Sprite[] frames)
    {
        flashImage.enabled = true;
        if (stingerSource != null && stingerClip != null) stingerSource.PlayOneShot(stingerClip);
        loopRoutine = StartCoroutine(JitterLoop(frames));
    }

    public void StopFlash()
    {
        if (loopRoutine != null) StopCoroutine(loopRoutine);
        flashImage.enabled = false;
    }

    IEnumerator JitterLoop(Sprite[] frames)
    {
        int index = 0;
        while (true)
        {
            flashImage.sprite = frames[index % frames.Length];
            index++;
            yield return new WaitForSeconds(jitterInterval);
        }
    }
}