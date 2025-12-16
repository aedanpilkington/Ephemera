using UnityEngine;
using System.Collections;

public class RespawningApple : MonoBehaviour
{
    public int value = 1;
    public float respawnTime = 8f;

    private bool collected;

    private Collider appleCollider;
    private Renderer appleRenderer;

    void Awake()
    {
        appleCollider = GetComponent<Collider>();
        appleRenderer = GetComponentInChildren<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        if (other.GetComponentInParent<CaterpillarController>() == null)
            return;

        collected = true;

        if (AppleManager.Instance != null)
            AppleManager.Instance.AddApple(value);

        // hide apple instead of destroying it
        appleCollider.enabled = false;
        appleRenderer.enabled = false;

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);

        collected = false;
        appleCollider.enabled = true;
        appleRenderer.enabled = true;
    }
}
