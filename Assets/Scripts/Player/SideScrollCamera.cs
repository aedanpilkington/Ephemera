using UnityEngine;

public class SideScrollCamera : MonoBehaviour
{
    public Transform target;

    // how far the camera sits from the caterpillar
    public Vector3 offset = new Vector3(0f, 2f, -10f);

    // smoothing so it doesn’t feel rigid
    public float followSpeed = 5f;

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            offset.z                // lock depth
        );

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );
    }
}
