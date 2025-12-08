using UnityEngine;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using System.Collections;
using TMPro;

public class MainController : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TMP_Text playerNameError;

    [SerializeField]

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerNameError.text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    // Handle saving player name from user input
    public void SavePlayerName()
    {
        string playerName = playerNameInput.text;
        // Check if submitted name is valid, then save it to PlayerPrefs
        if (!string.IsNullOrEmpty(playerName) && playerName.Length > 0)
        {
            Debug.Log("Player name submitted: " + playerName);
            PlayerPrefs.SetString(GameLogic.playerNameKey, playerName);
        } else
        {
            playerNameError.text = "Invalid player name!";
        }
    }
    
    // Start button calls this
    public void StartLevel(int levelIndex)
    {
        // Make sure that a name has been submitted
        string playerName = playerNameInput.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex);
        } else
        {
            playerNameError.text = "Submit a name first!";
        }
    }
    // Exit button calls this
    public void QuitGame()
    {
        Application.Quit();
    }
}



