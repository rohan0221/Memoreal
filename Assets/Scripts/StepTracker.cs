using UnityEngine;

public class StepTracker : MonoBehaviour
{
    public float distancePerStep = 1f;
    public int currentSteps;
    public AudioSource footstepSource;
    public AudioClip[] footstepClips;
    Vector3 lastPosition;
    float distanceSinceLastFootstep;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;

        if (distanceMoved > 0.001f)
        {
            DayCycleManager.Instance.AddDistance(distanceMoved);

            distanceSinceLastFootstep += distanceMoved;
            if (distanceSinceLastFootstep >= distancePerStep)
            {
                distanceSinceLastFootstep = 0f;
                currentSteps++;
                PlayFootstep();
            }
        }
    }

    public void ResetSteps()
    {
        currentSteps = 0;
    }

    void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;
        footstepSource.clip = footstepClips[Random.Range(0, footstepClips.Length)];
        footstepSource.Play();
    }
}