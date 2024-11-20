using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;

    public float pickupRange = 2f; // Range to pick up a basketball
    public Transform holdPoint;   // The position where the basketball is held
    public float shootForce = 10f; // Force to apply when shooting the ball

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;
    private GameObject heldBall = null; // The basketball currently held by the player

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandlePickupAndShoot();
    }

    private void HandleMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.R) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    private void HandlePickupAndShoot()
    {
        // Check for pickup
        if (Input.GetKeyDown(KeyCode.E) && heldBall == null)
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
            {
                if (hit.collider.CompareTag("Basketball"))
                {
                    heldBall = hit.collider.gameObject;
                    heldBall.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                    // Attach the ball to the camera (so it moves with the screen)
                    heldBall.transform.SetParent(playerCamera.transform);
                    heldBall.transform.localPosition = new Vector3(0.05f, -0.1f, 0.25f); // Adjusted position (higher on screen)
                    heldBall.transform.localRotation = Quaternion.identity; // Reset rotation
                    heldBall.transform.localScale = new Vector3(5f, 5f, 5f); // Optional: scale down for a better fit
                }
            }
        }

        // Check for shooting
        if (Input.GetKeyDown(KeyCode.Space) && heldBall != null)
        {
            Rigidbody ballRigidbody = heldBall.GetComponent<Rigidbody>();
            heldBall.transform.SetParent(null); // Detach from the camera
            ballRigidbody.isKinematic = false; // Enable physics

            // Add arc to the shot
            Vector3 shootDirection = playerCamera.transform.forward + Vector3.up * 0.5f; // Add upward force for arc
            ballRigidbody.AddForce(shootDirection.normalized * shootForce, ForceMode.Impulse); // Shoot the ball

            // Apply spin for realism
            ballRigidbody.AddTorque(playerCamera.transform.right * 10f, ForceMode.Impulse);

            heldBall = null; // Clear the held ball reference
        }
    }
}
