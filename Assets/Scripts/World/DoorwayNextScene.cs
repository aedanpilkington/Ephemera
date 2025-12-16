using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorwayNextScene : MonoBehaviour
{
    public string sceneToLoad = "TreeInterior_2D";

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<CaterpillarController>() != null)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}

