using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JumpscareFlash : MonoBehaviour
{
    public static JumpscareFlash Instance;
    public Image flashImage;
    public float jitterInterval = 0.08f;
    Coroutine loopRoutine;

    void Awake()
    {
        Instance = this;
        flashImage.enabled = false;
    }

    public void StartFlash(Sprite[] frames)
    {
        Debug.Log("StartFlash called on: " + gameObject.name + " / " + gameObject.GetInstanceID() + " | flashImage: " + (flashImage != null ? flashImage.name + " " + flashImage.GetInstanceID() : "NULL"));
        flashImage.enabled = true;
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