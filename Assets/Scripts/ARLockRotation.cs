using UnityEngine;

public class ARLockRotation : MonoBehaviour
{
    [Header("Lock Rotation")]
    public bool lockRotation = true;

    [Header("Rotation Values")]
    public Vector3 fixedRotation = new Vector3(0, 0, 0);

    void LateUpdate()
    {
        if (lockRotation)
        {
            transform.localRotation = Quaternion.Euler(fixedRotation);
        }
    }
}