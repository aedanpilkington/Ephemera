using UnityEngine;

public class Apple : MonoBehaviour
{
    public int value = 1;

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        // check player
        CaterpillarController controller =
            other.GetComponentInParent<CaterpillarController>();

        if (controller == null)
            return;

        collected = true;

        // add apple to counter
        if (AppleManager.Instance != null)
            AppleManager.Instance.AddApple(value);

        // grow caterpillar body
        CaterpillarFollow follow =
            controller.GetComponentInChildren<CaterpillarFollow>();

        if (follow != null)
            follow.AddSegment();

        Destroy(gameObject);
    }
}
