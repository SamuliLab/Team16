using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    
    
    // Start button calls this
    public void StartLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
    // Exit button calls this
    public void QuitGame()
    {
        Application.Quit();
    }
}



