using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject instructionsPanel1; // Instructions panel for Game Mode 1
    public GameObject instructionsPanel2; // Instructions panel for Game Mode 2
    public GameObject instructionsPanel3; // Instructions panel for Game Mode 3

    private void Start()
    {
        // Ensure all instruction panels are inactive when the game starts
        if (instructionsPanel1 != null)
        {
            instructionsPanel1.SetActive(false);
        }
        if (instructionsPanel2 != null)
        {
            instructionsPanel2.SetActive(false);
        }
        if (instructionsPanel3 != null)
        {
            instructionsPanel3.SetActive(false);
        }
    }

    // Play Game Mode 1
    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Play Game Mode 2
    public void PlayGame2()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    // Play Game Mode 3
    public void PlayGame3()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }

    // Quit Game
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    // Show Instructions for Game Mode 1
    public void ShowInstructions1()
    {
        if (instructionsPanel1 != null)
        {
            instructionsPanel1.SetActive(true);
        }
    }

    // Show Instructions for Game Mode 2
    public void ShowInstructions2()
    {
        if (instructionsPanel2 != null)
        {
            instructionsPanel2.SetActive(true);
        }
    }

    // Show Instructions for Game Mode 3
    public void ShowInstructions3()
    {
        if (instructionsPanel3 != null)
        {
            instructionsPanel3.SetActive(true);
        }
    }

    // Hide all instructions panels
    public void HideInstructions()
    {
        if (instructionsPanel1 != null)
        {
            instructionsPanel1.SetActive(false);
        }
        if (instructionsPanel2 != null)
        {
            instructionsPanel2.SetActive(false);
        }
        if (instructionsPanel3 != null)
        {
            instructionsPanel3.SetActive(false);
        }
    }
}
