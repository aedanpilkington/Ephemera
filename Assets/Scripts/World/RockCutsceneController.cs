using System.Collections;
using UnityEngine;

public class RockCutsceneController : MonoBehaviour
{
    public Camera cutsceneCamera;
    public Camera playerCamera;

    public float cutsceneDuration = 2.5f;

    private void Start()
    {
        cutsceneCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
    }


    public void PlayCutscene()
    {
        StartCoroutine(CutsceneRoutine());
    }

    private IEnumerator CutsceneRoutine()
    {
        // switch to cutscene camera
        playerCamera.gameObject.SetActive(false);
        cutsceneCamera.gameObject.SetActive(true);

        yield return new WaitForSeconds(cutsceneDuration);

        // back to player
        cutsceneCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
    }
}
