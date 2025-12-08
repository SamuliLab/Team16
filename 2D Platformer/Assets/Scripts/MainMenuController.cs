using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

public void StartLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

public void QuitGame()
    {
        Application.Quit();
    }

}
