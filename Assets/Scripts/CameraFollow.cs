using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;           // The player
    public Vector3 offset = new Vector3(0f, 1.5f, -10f);  // Camera offset
    public float smoothSpeed = 5f;     // How snappy the follow is

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }
}
