using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private CinemachineCamera Camera;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float airControlFactor = 0.2f; // Adjust air control level

    private Vector2 moveInput;
    private Vector3 lastGroundedVelocity = Vector3.zero;
    private Rigidbody rb;
    private bool isGrounded = true;
    private int jumpCount;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        RotationUpdate();
    }

    private void FixedUpdate()
    {
        MovementUpdate();
    }

    private void RotationUpdate()
    {
        Vector3 camForward = Camera.transform.forward;
        camForward.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(camForward);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));
    }

    private void MovementUpdate()
    {
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

        if (isGrounded)
        {
            Vector3 groundedPosition = rb.position + moveDirection * speed * Time.fixedDeltaTime;
            rb.MovePosition(new Vector3(groundedPosition.x, rb.position.y, groundedPosition.z));
            lastGroundedVelocity = new Vector3(moveDirection.x * speed, 0, moveDirection.z * speed);
        }
        else
        {
            // Maintain momentum with limited air control
            Vector3 airVelocity = lastGroundedVelocity * (1 - airControlFactor) + moveDirection * (speed * airControlFactor);
            rb.linearVelocity = new Vector3(airVelocity.x, rb.linearVelocity.y, airVelocity.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
        // if collision is againt tag wall, move player in opposite direction
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Wall hit");
            // set player transform to possition before collision
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Allow the player to slide down slightly instead of sticking
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -0.5f, rb.linearVelocity.z);

            // Optional: Small push-off effect to prevent hard sticking
            rb.AddForce(transform.forward * 1.5f, ForceMode.Impulse);
        }
    }

    public void OnJump()
    {
        if (jumpCount < 2)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpHeight, rb.linearVelocity.z);
            isGrounded = false;
            jumpCount++;
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
