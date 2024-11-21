using UnityEngine;

public class BasketballCollision : MonoBehaviour
{
    [SerializeField] private AudioClip boundaryHitSound; // Sound for hitting the boundary
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
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip); // Play the audio clip once without interrupting other sounds
        }
    }
}
