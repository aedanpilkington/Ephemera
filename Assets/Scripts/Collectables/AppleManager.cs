using UnityEngine;

public class AppleManager : MonoBehaviour
{
    public static AppleManager Instance;

    public int appleCount;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddApple(int amount)
    {
        appleCount += amount;
    }

    public bool SpendApples(int amount)
    {
        if (appleCount < amount)
            return false;

        appleCount -= amount;
        return true;
    }
}
