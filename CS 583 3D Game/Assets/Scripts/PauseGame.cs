using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool paused;

    void Start()
    {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor initially
        Cursor.visible = false; // Hide the cursor initially
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (paused)
            {
                ResumeGame();
            }
            else
            {
                PausedGame();
            }
        }
    }

    public void PausedGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        paused = true;

        // Show the cursor and unlock it
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        paused = false;

        // Hide the cursor and lock it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GoToMainMenu()
    {
        paused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");

        // Show the cursor when returning to the main menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
