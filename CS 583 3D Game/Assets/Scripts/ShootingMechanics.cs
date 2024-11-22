using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private GameObject heldBall = null; // The basketball currently held by the player
    private GameObject highlightedBall = null; // The basketball currently highlighted
    private Material originalMaterial; // To store the original material of the basketball
    private float chargeTime = 0f; // Time the shoot button is held
    private bool isCharging = false; // Whether the player is charging the shot

    void Start()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false); // Hide the text initially
            interactionText.gameObject.SetActive(false); // Hide the text initially
        }

        if (chargeText != null)
        {
            chargeText.gameObject.SetActive(false); // Hide the charge text initially
        }
    }

    void Update()
    {
        HighlightBasketball();
        HandlePickup();
        HandleChargeAndShoot();
        UpdateHeldBallPosition(); // Ensure the held ball stays at the holdPoint
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
            if (hit.collider.CompareTag("Basketball"))
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
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }

    private void HandleChargeAndShoot()
    {
        if (heldBall == null) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            chargeTime = 0f;
            isCharging = true;
            if (chargeText != null)
            {
                chargeText.gameObject.SetActive(true);
            }
        }

        if (Input.GetKey(KeyCode.F) && isCharging)
        {
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);

            if (chargeText != null)
            {
                float chargePercentage = (chargeTime / maxChargeTime) * 100f;
                chargeText.text = $"Charge: {chargePercentage:0}%";
            }
        }

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

            if (chargeText != null)
            {
                chargeText.gameObject.SetActive(false);
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
