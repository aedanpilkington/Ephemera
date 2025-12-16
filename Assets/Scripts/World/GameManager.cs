using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Gold Apple Progress")]
    public int totalGoldApples;
    public int collectedGoldApples;

    [Header("World")]
    public DoorBlocker doorBlocker;
    public GameObject winText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // count gold apples ONCE at start
        GoldApple[] apples = FindObjectsOfType<GoldApple>();
        totalGoldApples = apples.Length;
        collectedGoldApples = 0;

        Debug.Log("Gold apples in scene: " + totalGoldApples);
    }

    public void GoldAppleCollected()
    {
        collectedGoldApples++;
        Debug.Log("Gold apples collected: " + collectedGoldApples + " / " + totalGoldApples);

        if (collectedGoldApples >= totalGoldApples)
        {
            if (doorBlocker != null)
                doorBlocker.Unlock();
        }
    }

    public bool HasAllGoldApples()
    {
        return collectedGoldApples >= totalGoldApples;
        if (cutscene != null)
            cutscene.PlayCutscene();

        if (doorBlocker != null)
            doorBlocker.Unlock();
    }

    public void TriggerWin()
    {
        if (winText != null)
            winText.SetActive(true);
    }

    public RockCutsceneController cutscene;

}
