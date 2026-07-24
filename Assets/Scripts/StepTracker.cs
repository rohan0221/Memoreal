using UnityEngine;

public class StepTracker : MonoBehaviour
{
    public float distancePerStep = 1f;
    public int currentSteps;
    Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;

        if (distanceMoved > 0.001f) // ignore floating point noise while standing still
        {
            DayCycleManager.Instance.AddDistance(distanceMoved);
        }
    }

    public void ResetSteps()
    {
        currentSteps = 0;
    }
}