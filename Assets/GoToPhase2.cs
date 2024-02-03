using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToPhase2 : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneToLoad; // Name of the scene to preload

    void Start()
    {
        // Start the preload process
        StartCoroutine(LoadSceneAfterDelay());
    }

    IEnumerator LoadSceneAfterDelay()
    {
        // Preload the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false; // Prevent automatic scene activation

        // Wait until the preload operation is almost done
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
                break;

            yield return null;
        }

        // Wait for 15 seconds
        yield return new WaitForSeconds(14.45f);

        // Activate the preloaded scene
        asyncLoad.allowSceneActivation = true;
    }

}
