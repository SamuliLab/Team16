using UnityEngine;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using System.Collections;
using TMPro;

public class MainController : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TMP_Text playerNameError;
    public GameObject nameMenu;
    public GameObject mainMenu;

    [SerializeField]

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Clear name error text
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
            playerNameError.text = "";
        } else
        {
            playerNameError.text = "Invalid player name!";
        }
    }
    
    // Check if a Player Name has been saved before moving to main menu
    public void CheckPlayerName()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(GameLogic.playerNameKey)))
        {
            nameMenu.SetActive(false);
            mainMenu.SetActive(true);
        } else
        {
            playerNameError.text = "No player name given!";
        }
    }
}



