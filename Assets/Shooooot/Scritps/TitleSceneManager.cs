using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
 *
 * This script is used to move from the title scene to the main scene.
 *
 */
public class TitleSceneManager : MonoBehaviour {


    private void Start()
    {
        StartCoroutine(GoToMainScene());
    }


    private IEnumerator GoToMainScene()
    {
        // wait for 2 seconds
        var waitTime = 2f;
        yield return new WaitForSeconds(waitTime);

        // load the next scene (main scene)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}