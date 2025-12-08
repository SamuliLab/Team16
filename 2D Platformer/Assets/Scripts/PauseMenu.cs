using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds0_2 = new WaitForSeconds(0.2f);
    public bool isPaused = false; // Flag for tracking pause state
    // THE FOLLOWING NEED TO BE CREATED AND LINKED IN THE INSPECTOR
    // public GameObject pauseMenu; // Reference to the pause menu UI
    // public GameObject menuMusic; // Reference to the menu music object

    void Update()
    {
        // Toggle pause state with Escape key
        if (Input.GetKeyUp("escape") && !isPaused)
        {
            PauseGame();
        }
        else if (Input.GetKeyUp("escape") && isPaused)
        {
            ResumeGame();
        }
    }
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0; // Pause the game
        // pauseMenu.SetActive(true); // Show pause menu
        // menuMusic.SetActive(true); // Play menu music
    }
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; // Resume the game
        // pauseMenu.SetActive(false); // Hide pause menu
        // menuMusic.SetActive(false); // Stop menu music
    }
}