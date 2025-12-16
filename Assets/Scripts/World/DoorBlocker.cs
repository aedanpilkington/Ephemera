using UnityEngine;

public class DoorBlocker : MonoBehaviour
{
    [Header("Fall Settings")]
    public float fallDuration = 1.5f;

    [Header("Pivot (local offset to bottom of rock)")]
    public Vector3 pivotOffset = new Vector3(0f, -0.5f, 0f);

    [Header("End Rotation")]
    public Vector3 endRotationEuler;

    private Quaternion startRotation;
    private Quaternion endRotation;

    private Vector3 worldPivot;
    private bool shouldFall;
    private float timer;

    private void Start()
    {
        // capture start state from scene
        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(endRotationEuler);

        // FIXED pivot point in world space
        worldPivot = transform.TransformPoint(pivotOffset);
    }

    private void Update()
    {
        if (!shouldFall) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / fallDuration);

        // ease-in (slow → fast)
        float easedT = t * t;

        Quaternion targetRotation =
            Quaternion.Slerp(startRotation, endRotation, easedT);

        float angleDelta =
            Quaternion.Angle(transform.rotation, targetRotation);

        // rotate around fixed pivot only
        transform.RotateAround(
            worldPivot,
            transform.right,
            angleDelta
        );

        transform.rotation = targetRotation;
    }

    public void Unlock()
    {
        shouldFall = true;
        timer = 0f;
    }
}
