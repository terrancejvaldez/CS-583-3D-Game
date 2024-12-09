using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public AudioSource crowdCheer; // Audio source for the crowd cheering
    public static bool paused;

    void Start()
    {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false); // Hide game over menu initially
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

    public void ShowGameOverMenu(int finalScore)
    {
        Time.timeScale = 0f; // Freeze time
        paused = true;

        gameOverMenu.SetActive(true); // Show game over menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Play crowd cheer sound
        if (crowdCheer != null)
        {
            crowdCheer.Play();
        }

        // Update game over menu with final score
        var scoreText = gameOverMenu.transform.Find("FinalScoreText").GetComponent<TMPro.TextMeshProUGUI>();
        if (scoreText != null)
        {
            scoreText.text = $"Final Score: {finalScore}";
        }
    }

    public void RetryGame()
    {
        // Reset Time scale
        Time.timeScale = 1f;

        // Reset Cursor state for player control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Reset the paused state
        paused = false;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
