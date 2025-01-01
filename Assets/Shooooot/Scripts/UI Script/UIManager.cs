using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void PlayGame() {
        SceneManager.LoadSceneAsync(2);
    }
    public void QuitGame() {
        Application.Quit();
    }

    public void TutorialShoot() {
        SceneManager.LoadSceneAsync(3);
    }
}
