using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<CaterpillarController>() == null)
            return;

        if (GameManager.Instance != null &&
            GameManager.Instance.HasAllGoldApples())
        {
            GameManager.Instance.TriggerWin();
        }
    }
}
