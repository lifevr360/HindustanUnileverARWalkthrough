using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (cam == null) return;

        Vector3 direction = cam.transform.position - transform.position;

        // Remove vertical tilt (keeps video upright)
        direction.y = 0.2f;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}