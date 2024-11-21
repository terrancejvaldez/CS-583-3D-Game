using UnityEngine;

public class HoopTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource hoopAudioSource; // Audio source for the hoop
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is tagged as "Basketball"
        if (other.CompareTag("Basketball"))
        {
            // Destroy the basketball after 5 seconds
            Destroy(other.gameObject, 5f);
            if (hoopAudioSource != null)
            {
                hoopAudioSource.Play();
            }

            // Optional: Log or trigger an event
            Debug.Log("Basketball went through the hoop!");
        }
    }
}
