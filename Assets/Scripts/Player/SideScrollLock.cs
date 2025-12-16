using UnityEngine;

public class SideScrollPhysicsLock : MonoBehaviour
{
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints |= RigidbodyConstraints.FreezePositionZ;
    }
}
