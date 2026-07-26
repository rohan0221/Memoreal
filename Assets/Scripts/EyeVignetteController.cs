using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EyeVignetteController : MonoBehaviour
{
    public Image vignetteImage;
    public Sprite[] closeFrames; // eye open -> fully closed (last frame should be solid black)
    public Sprite[] openFrames;  // fully closed -> eye open
    public float frameRate = 12f; // frames per second

    public IEnumerator PlayOpen()
    {
        vignetteImage.enabled = true;
        yield return PlaySequence(openFrames);
        vignetteImage.enabled = false; // hide it completely once fully open
    }

    public IEnumerator PlayClose()
    {
        vignetteImage.enabled = true;
        yield return PlaySequence(closeFrames);
        vignetteImage.enabled = false; // hide it completely once fully closed
    }

    IEnumerator PlaySequence(Sprite[] frames)
    {
        float frameDuration = 1f / frameRate;
        foreach (Sprite frame in frames)
        {
            vignetteImage.sprite = frame;
            yield return new WaitForSeconds(frameDuration);
        }
    }
    public void HoldClosed()
    {
        vignetteImage.enabled = true;
        if (closeFrames.Length > 0)
            vignetteImage.sprite = closeFrames[closeFrames.Length - 1];
    }
}