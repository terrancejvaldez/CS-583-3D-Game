using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShootingMechanics : MonoBehaviour
{
    public Camera playerCamera;
    public float pickupRange = 1f; // Range to pick up a basketball
    public Transform holdPoint;   // The position where the basketball is held
    public float shootForce = 10f; // Force to apply when shooting the ball
    public TextMeshProUGUI interactionText; // TextMeshPro UI for "Press E to Pick Up"
    public Material highlightMaterial; // Material to highlight the basketball

    private GameObject heldBall = null; // The basketball currently held by the player
    private GameObject highlightedBall = null; // The basketball currently highlighted
    private Material originalMaterial; // To store the original material of the basketball

    void Start()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false); // Hide the text initially
        }
    }

    void Update()
    {
        HighlightBasketball();
        HandlePickupAndShoot();
        UpdateHeldBallPosition(); // Ensure the held ball stays at the holdPoint
    }

    private void HighlightBasketball()
    {
        // Do not highlight or show text if holding a ball
        if (heldBall != null)
        {
            RemoveHighlight();
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false); // Hide text
            }
            return; // Exit early
        }

        // Raycast to detect a basketball
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.CompareTag("Basketball"))
            {
                GameObject ball = hit.collider.gameObject;

                // Highlight the basketball if it's not already highlighted
                if (highlightedBall != ball)
                {
                    RemoveHighlight(); // Remove highlight from the previous ball

                    highlightedBall = ball;
                    originalMaterial = ball.GetComponent<Renderer>().material; // Store the original material
                    ball.GetComponent<Renderer>().material = highlightMaterial; // Apply the outline material

                    // Show the interaction text
                    if (interactionText != null)
                    {
                        interactionText.gameObject.SetActive(true);
                        interactionText.text = "Press E to Pick Up";
                    }
                }

                return; // Exit early if a ball is detected
            }
        }

        // No basketball detected, remove highlight and hide text
        RemoveHighlight();
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    private void RemoveHighlight()
    {
        if (highlightedBall != null)
        {
            highlightedBall.GetComponent<Renderer>().material = originalMaterial; // Restore original material
            highlightedBall = null;
        }
    }

    private void HandlePickupAndShoot()
    {
        // Check for pickup
        if (Input.GetKeyDown(KeyCode.E) && heldBall == null && highlightedBall != null)
        {
            heldBall = highlightedBall;
            Rigidbody ballRigidbody = heldBall.GetComponent<Rigidbody>();
            ballRigidbody.isKinematic = true; // Disable physics
            heldBall.transform.SetParent(null); // Unparent in case it's parented elsewhere

            RemoveHighlight(); // Remove highlight after picking up
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false); // Hide the interaction text
            }
        }

        // Check for shooting
        if (Input.GetKeyDown(KeyCode.Space) && heldBall != null)
        {
            Rigidbody ballRigidbody = heldBall.GetComponent<Rigidbody>();
            ballRigidbody.isKinematic = false; // Enable physics

            // Add arc to the shot
            Vector3 shootDirection = playerCamera.transform.forward + Vector3.up * 0.5f; // Add upward force for arc
            ballRigidbody.AddForce(shootDirection.normalized * shootForce, ForceMode.Impulse); // Shoot the ball

            // Apply spin for realism
            ballRigidbody.AddTorque(playerCamera.transform.right * 10f, ForceMode.Impulse);

            heldBall = null; // Clear the held ball reference
        }
    }

    private void UpdateHeldBallPosition()
    {
        if (heldBall != null)
        {
            // Make the ball follow the holdPoint
            heldBall.transform.position = holdPoint.position;
            heldBall.transform.rotation = holdPoint.rotation;
        }
    }
}
