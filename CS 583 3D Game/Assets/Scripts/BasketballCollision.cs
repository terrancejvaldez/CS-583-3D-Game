using UnityEngine;

public class BasketballCollision : MonoBehaviour
{
    [SerializeField] private AudioClip boundaryHitSound; // Sound for hitting the boundary
    [SerializeField] private AudioClip badMissSound; // Sound for hitting the rim or backboard
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component attached to the basketball
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object the ball collides with has the "Boundary" tag
        if (collision.collider.CompareTag("Boundary"))
        {
            PlaySound(boundaryHitSound);
        }
        // Check if the ball hits the rim or backboard
        else if (collision.collider.CompareTag("Rim") || collision.collider.CompareTag("Backboard"))
        {
            PlaySound(badMissSound);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // Play the audio clip once without interrupting other sounds
        }
    }
}
