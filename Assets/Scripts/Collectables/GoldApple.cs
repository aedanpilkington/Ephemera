using UnityEngine;

public class GoldApple : MonoBehaviour
{
    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        if (other.GetComponentInParent<CaterpillarController>() == null)
            return;

        collected = true;

        if (GameManager.Instance != null)
            GameManager.Instance.GoldAppleCollected();

        Destroy(gameObject);
    }
}
