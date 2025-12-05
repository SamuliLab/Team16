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
        PlayerPrefs.SetString(GameLogic.playerNameKey, playerName);
        Debug.Log("Player name saved: " + playerName);
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
