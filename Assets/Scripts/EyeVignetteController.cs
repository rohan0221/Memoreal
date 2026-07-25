using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EyeVignetteController : MonoBehaviour
{
    public Image vignetteImage;
    public Sprite[] closeFrames; // eye open -> fully closed (last frame should be solid black)
    public Sprite[] openFrames;  // fully closed -> eye open
    public float frameRate = 12f; // frames per second

    public IEnumerator PlayClose()
    {
        yield return PlaySequence(closeFrames);
    }

    public IEnumerator PlayOpen()
    {
        yield return PlaySequence(openFrames);
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
}