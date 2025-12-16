using UnityEngine;

public class CrouchSizeGate : MonoBehaviour
{
    public GameObject blockingObject;
    public int maxApplesAllowed = 3;

    private bool isUnlocked = false;

    private void OnTriggerStay(Collider other)
    {
        CaterpillarController controller =
            other.GetComponentInParent<CaterpillarController>();

        if (controller == null) return;

        int apples =
            AppleManager.Instance != null
            ? AppleManager.Instance.appleCount
            : 0;

        if (controller.IsCrouching() && apples <= maxApplesAllowed)
        {
            if (!isUnlocked)
            {
                blockingObject.SetActive(false);
                isUnlocked = true;
            }
        }
        else
        {
            // conditions no longer met while inside
            if (isUnlocked)
            {
                blockingObject.SetActive(true);
                isUnlocked = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // reset gate when player leaves
        if (isUnlocked)
        {
            blockingObject.SetActive(true);
            isUnlocked = false;
        }
    }
}
