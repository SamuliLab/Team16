using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameLogic : MonoBehaviour
{

    public TMPro.TMP_InputField tmpIfTimeElapsed;
    public GameObject GameOverScreen;
    public TMPro.TMP_Text tmpTextEndMessage;
    public static string playerScoreKey = "PlayerScoreKey"; // Key for storing player score in PlayerPrefs
    public static string playerNameKey = "PlayerNameKey"; // Key for storing player name in PlayerPrefs
    public static string gameOverFlag = "GameOverFlag"; // Key for storing game over flag in PlayerPrefs

    private PauseMenu pauseMenu;
    private static WaitForSeconds _waitForSeconds1_0 = new(1.0f);
    public bool GameOver;
    private string endMessage;
    public int PlayerScore;
    public string PlayerName;

    // Start is called before the first frame update
    void Start()
    {
        GameOver = false;
        PlayerPrefs.SetString(gameOverFlag, "false"); // Reset game over flag
        tmpTextEndMessage.text = "";
        Time.timeScale = 1;
        PlayerScore = 0;
        StartCoroutine(CheckStatus());
    }

    IEnumerator CheckStatus()
    {
        while (true)
        {
            GameObject[] keys = GameObject.FindGameObjectsWithTag("key"); // Initialize array of all keys in scene
            if (keys.Length == 0 && !GameOver) // If no keys remain and game is not over
            {
                // Save player score to PlayerPrefs
                PlayerPrefs.SetInt(playerScoreKey, PlayerScore);
                // Set game over flag in PlayerPrefs
                PlayerPrefs.SetString(gameOverFlag, "true");
                // Update game over message and enable the game over screen
                endMessage = "Congratulations!\nYou completed the game. Your score was:\n" + PlayerScore.ToString();
                tmpTextEndMessage.text = endMessage;
                GameOverScreen.SetActive(true);
                GameOver = true;
                Time.timeScale = 0; // Stops the game
            }
                tmpIfTimeElapsed.text = Mathf.FloorToInt(Time.timeSinceLevelLoad).ToString() + "s";
                PlayerScore ++;
                yield return _waitForSeconds1_0;
                Debug.Log("Keys remaining: " + keys.Length.ToString());
        }
    }
    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(0); // Assuming main menu is at index 0
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}