using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    // Start button calls this
    public void StartLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    // Quit button calls this
    public void QuitGame()
    {
        Application.Quit();
    }
}



