using UnityEngine;

public class StepTracker : MonoBehaviour
{
    public float distancePerStep = 1f; // kept for AddDistance logic, no longer drives audio
    public int currentSteps;
    public AudioSource footstepSource;
    public AudioClip walkingLoopClip;
    Vector3 lastPosition;
    bool isMoving;
    bool hasStartedClip;

    void Start()
    {
        lastPosition = transform.position;
        footstepSource.clip = walkingLoopClip;
        footstepSource.loop = true;
    }

    void Update()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;

        bool currentlyMoving = distanceMoved > 0.001f;

        if (currentlyMoving)
        {
            DayCycleManager.Instance.AddDistance(distanceMoved);
        }

        if (currentlyMoving && !isMoving)
        {
            // just started moving
            if (!hasStartedClip)
            {
                footstepSource.Play();
                hasStartedClip = true;
            }
            else
            {
                footstepSource.UnPause();
            }
        }
        else if (!currentlyMoving && isMoving)
        {
            // just stopped moving
            footstepSource.Pause();
        }

        isMoving = currentlyMoving;
    }

    public void ResetSteps()
    {
        currentSteps = 0;
    }
}