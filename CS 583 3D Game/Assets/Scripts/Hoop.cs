using UnityEngine;
using TMPro;

public class Hoop : MonoBehaviour
{
    [SerializeField] private AudioSource hoopAudioSource; // Audio source for the hoop
    [SerializeField] private ParticleSystem goalEffect; // Assign this in the Inspector
    [SerializeField] private TextMeshProUGUI scoreText; // Reference to TextMeshPro UI to display the score

    private int score = 0; // Variable to hold the score
    private float timeRemaining; // Variable to track the remaining time
    private bool isGameActive = true; // Flag to check if the game is still active

    private void Start()
    {
        // Initialize the timer and score
        UpdateScoreText();
    }


    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is tagged as "Basketball"
        if ((other.CompareTag("Basketball") || other.CompareTag("Basketball_White") || other.CompareTag("Basketball_Green")) && isGameActive)
        {
            // Destroy the basketball after 5 seconds
            Destroy(other.gameObject, 5f);

            // Play the hoop sound and effect
            if (hoopAudioSource != null)
            {
                hoopAudioSource.Play();
            }
            if (goalEffect != null)
            {
                goalEffect.Play();
            }

            // Increase the score by 1
            if (other.CompareTag("Basketball"))
            {
                score++;
            }
            else if (other.CompareTag("Basketball_White"))
            {
                score += 2;
            }
            else if (other.CompareTag("Basketball_Green"))
            {
                score += 3;
            }

            // Update the score text
            UpdateScoreText();

            // Optional: Log or trigger an event
            Debug.Log("Basketball went through the hoop!");
        }
    }

    // Method to update the score display
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString(); // Update the score on the UI
        }
    }

    

    // Method to handle the end of the game
    private void EndGame()
    {
        Debug.Log("Game Over!");
        // Optional: Implement game over logic (e.g., show Game Over screen, stop spawning basketballs, etc.)
    }
}