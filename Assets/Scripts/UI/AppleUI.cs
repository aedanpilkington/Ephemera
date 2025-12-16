using UnityEngine;
using TMPro;

public class AppleUI : MonoBehaviour
{
    [Header("Normal Apples")]
    public TMP_Text appleText;

    [Header("Gold Apples")]
    public TMP_Text goldAppleText;

    private void Update()
    {
        UpdateAppleUI();
        UpdateGoldAppleUI();
    }

    private void UpdateAppleUI()
    {
        if (AppleManager.Instance == null || appleText == null)
            return;

        appleText.text = AppleManager.Instance.appleCount.ToString();
    }

    private void UpdateGoldAppleUI()
    {
        if (GameManager.Instance == null || goldAppleText == null)
            return;

        goldAppleText.text =
            GameManager.Instance.collectedGoldApples +
            " / " +
            GameManager.Instance.totalGoldApples;
    }
}
