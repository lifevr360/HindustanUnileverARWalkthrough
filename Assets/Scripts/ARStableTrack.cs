using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARStableTrack : MonoBehaviour
{
    private ARTrackedImage trackedImage;

    [Header("Smoothing")]
    public float positionSmoothSpeed = 10f;

    private Vector3 targetPosition;

    void Awake()
    {
        trackedImage = GetComponent<ARTrackedImage>();
    }

    void Update()
    {
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            // Smooth position
            targetPosition = trackedImage.transform.position;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * positionSmoothSpeed);
        }
    }
}