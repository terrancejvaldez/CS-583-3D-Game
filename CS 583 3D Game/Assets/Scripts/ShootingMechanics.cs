using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // Added to use UI components like Slider and Image

public class ShootingMechanics : MonoBehaviour
{
    public Camera playerCamera;
    public float pickupRange = 1f; // Range to pick up a basketball
    public Transform holdPoint;   // The position where the basketball is held
    public float maxShootForce = 10f; // Maximum force to apply when fully charged
    public float minShootForce = 3f;  // Minimum force to apply
    public float maxChargeTime = 1f;  // Time to reach maximum charge
    public TextMeshProUGUI interactionText; // TextMeshPro UI for "Press E to Pick Up"
    public Material highlightMaterial; // Material to highlight the basketball
    public TextMeshProUGUI chargeText; // Optional text for showing charge percentage

    public Slider powerMeter; // Added: Reference to the power meter slider
    public Image powerMeterFill; // Added: Reference to the slider's fill image
    public Gradient powerMeterColor; // Added: Gradient to define color changes for the slider

    public HoopTrigger hoopTrigger; // Reference to the HoopTrigger script

    private GameObject heldBall = null; // The basketball currently held by the player
    private GameObject highlightedBall = null; // The basketball currently highlighted
    private Material originalMaterial; // To store the original material of the basketball
    private float chargeTime = 0f; // Time the shoot button is held
    private bool isCharging = false; // Whether the player is charging the shot
    private bool hasStartedTimer = false; // Tracks if the timer has started

    void Start()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false); // Hide the interaction text initially
        }

        if (chargeText != null)
        {
            chargeText.gameObject.SetActive(false); // Hide the charge text initially
        }

        if (powerMeter != null)
        {
            powerMeter.gameObject.SetActive(false); // Hide the power meter initially
        }

        if (powerMeterFill != null)
        {
            powerMeterFill.color = powerMeterColor.Evaluate(0f); // Set initial color for the fill
        }

        // Update the timer display to show the default game duration
        if (hoopTrigger != null)
        {
            hoopTrigger.UpdateTimerText();
        }
    }

    void Update()
    {
        if (!PauseGame.paused)
        {
            HighlightBasketball();
            HandlePickup();
            HandleChargeAndShoot();
            UpdateHeldBallPosition(); // Ensure the held ball stays at the holdPoint
        }
    }

    private void HighlightBasketball()
    {
        if (heldBall != null)
        {
            RemoveHighlight();
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false); // Hide text
            }
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.CompareTag("Basketball") || hit.collider.CompareTag("Basketball_White") || hit.collider.CompareTag("Basketball_Green"))
            {
                GameObject ball = hit.collider.gameObject;

                if (highlightedBall != ball)
                {
                    RemoveHighlight();

                    highlightedBall = ball;
                    originalMaterial = ball.GetComponent<Renderer>().material;
                    ball.GetComponent<Renderer>().material = highlightMaterial;

                    if (interactionText != null)
                    {
                        interactionText.gameObject.SetActive(true);
                        interactionText.text = "Press E to Pick Up";
                    }
                }

                return;
            }
        }

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
            highlightedBall.GetComponent<Renderer>().material = originalMaterial;
            highlightedBall = null;
        }
    }

    private void HandlePickup()
    {
        if (Input.GetKeyDown(KeyCode.E) && heldBall == null && highlightedBall != null)
        {
            heldBall = highlightedBall;
            Rigidbody ballRigidbody = heldBall.GetComponent<Rigidbody>();
            ballRigidbody.isKinematic = true;
            heldBall.transform.SetParent(null);

            RemoveHighlight();

            // Activate the charge text and power meter when the player picks up the ball
            if (chargeText != null)
            {
                chargeText.gameObject.SetActive(true);
            }

            if (powerMeter != null)
            {
                powerMeter.gameObject.SetActive(true); // Make the power meter visible
                powerMeter.value = 0f; // Reset the power meter slider to 0
            }

            if (powerMeterFill != null)
            {
                powerMeterFill.color = powerMeterColor.Evaluate(0f); // Reset the fill color to the starting color
            }

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false); // Hide the interaction text
            }

            // Start the game timer only when the first ball is picked up
            if (!hasStartedTimer && hoopTrigger != null)
            {
                hoopTrigger.StartGameTimer();
                hasStartedTimer = true;
            }
        }
    }

    private void HandleChargeAndShoot()
    {
        if (heldBall == null) return;

        // Start charging the shot
        if (Input.GetKeyDown(KeyCode.F))
        {
            chargeTime = 0f;
            isCharging = true;

            // Ensure the power meter and charge text are visible when starting to charge
            if (chargeText != null)
            {
                chargeText.gameObject.SetActive(true);
            }

            if (powerMeter != null)
            {
                powerMeter.gameObject.SetActive(true);
                powerMeter.value = 0f; // Reset the power meter slider to 0
            }

            if (powerMeterFill != null)
            {
                powerMeterFill.color = powerMeterColor.Evaluate(0f); // Reset the fill color to the starting color
            }
        }

        // Update the charge while holding the button
        if (Input.GetKey(KeyCode.F) && isCharging)
        {
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);

            if (chargeText != null)
            {
                float chargePercentage = (chargeTime / maxChargeTime) * 100f;
                chargeText.text = $"Charge: {chargePercentage:0}%";
            }

            if (powerMeter != null)
            {
                powerMeter.value = chargeTime / maxChargeTime; // Update slider value
            }

            if (powerMeterFill != null)
            {
                powerMeterFill.color = powerMeterColor.Evaluate(chargeTime / maxChargeTime); // Update slider fill color
            }
        }

        // Cancel the shot when the left mouse button is clicked
        if (Input.GetMouseButtonDown(0) && isCharging)
        {
            isCharging = false;
            chargeTime = 0f;

            if (chargeText != null)
            {
                chargeText.gameObject.SetActive(false);
            }

            if (powerMeter != null)
            {
                powerMeter.gameObject.SetActive(false);
                powerMeter.value = 0f; // Reset the power meter slider to 0
            }

            if (powerMeterFill != null)
            {
                powerMeterFill.color = powerMeterColor.Evaluate(0f); // Reset the fill color to the starting color
            }
        }

        // Shoot the ball when the key is released
        if (Input.GetKeyUp(KeyCode.F) && isCharging)
        {
            float chargeRatio = chargeTime / maxChargeTime;
            float shootForce = Mathf.Lerp(minShootForce, maxShootForce, chargeRatio);

            Rigidbody ballRigidbody = heldBall.GetComponent<Rigidbody>();
            ballRigidbody.isKinematic = false;

            Vector3 shootDirection = playerCamera.transform.forward + Vector3.up * 0.5f;
            ballRigidbody.AddForce(shootDirection.normalized * shootForce, ForceMode.Impulse);

            ballRigidbody.AddTorque(playerCamera.transform.right * 10f, ForceMode.Impulse);

            heldBall = null;
            isCharging = false;

            // Hide the charge text and power meter when the ball is released
            if (chargeText != null)
            {
                chargeText.gameObject.SetActive(false);
            }

            if (powerMeter != null)
            {
                powerMeter.gameObject.SetActive(false); // Hide the power meter
            }
        }
    }

    private void UpdateHeldBallPosition()
    {
        if (heldBall != null)
        {
            heldBall.transform.position = holdPoint.position;
            heldBall.transform.rotation = holdPoint.rotation;
        }
    }
}
