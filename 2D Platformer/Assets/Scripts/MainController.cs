using JetBrains.Annotations;
using System.Collections;
using TMPro;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public TMP_InputField playerNameInput;

    [SerializeField]

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
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
            Debug.Log("Invalid player name submitted.");
        }
    }
    public void StartLevel(int levelIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
