using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
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
        UpdateHeldBallPosition(); // Ensure the held ball stays at the holdPoint
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
            walkSpeed = 3f;
            runSpeed = 6f;
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
                    Rigidbody ballRigidbody = heldBall.GetComponent<Rigidbody>();
                    ballRigidbody.isKinematic = true; // Disable physics
                    heldBall.transform.SetParent(null); // Unparent in case it's parented elsewhere
                }
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
