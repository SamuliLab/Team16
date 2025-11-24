using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameLogic : MonoBehaviour
{
    public TMPro.TMP_InputField tmpIfTimeElapsed;
    public TMPro.TMP_Text tmpTextEndMessage;

    private PauseMenu pauseMenu;
    private static WaitForSeconds _waitForSeconds1_0 = new(1.0f);
    private bool GameOver;
    private int PlayerScore;
    private string endMessage;

    // Start is called before the first frame update
    void Start()
    {
        GameOver = false;
        tmpTextEndMessage.text = "";
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
                endMessage = "Congratulations!\nYou completed the game. Your score was:\n" + PlayerScore.ToString();
                tmpTextEndMessage.text = endMessage; // Display end message
                GameOver = true;
                Time.timeScale = 0; // Stops the game
                // Load UI here for buttons to restart or go to main menu
            }
                tmpIfTimeElapsed.text = Mathf.FloorToInt(Time.timeSinceLevelLoad).ToString() + "s";
                PlayerScore ++;
                yield return _waitForSeconds1_0;
                Debug.Log("Keys remaining: " + keys.Length.ToString());
        }
    }

}