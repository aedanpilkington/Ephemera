using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitToButterflyScene : MonoBehaviour
{
    public string sceneToLoad = "EphemeraButterfly";

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<CaterpillarController>() != null)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
