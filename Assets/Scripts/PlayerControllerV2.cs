using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerV2 : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 9f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private CinemachineCamera Camera;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float airControlFactor = 0.2f;
    private Vector2 moveInput;
    private bool isGrounded = true;
    private bool hasDash = true;
    private int jumpCount;
    private Rigidbody rb;
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
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log("Move Input: " + moveInput);
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
        Vector3 moveDirection = (transform.right * moveInput.x) + (transform.forward * moveInput.y);
        moveDirection.Normalize();

        // Adjust acceleration based on whether the player is grounded
        float currentAcceleration = isGrounded ? acceleration : acceleration * airControlFactor;

        rb.AddForce(moveDirection * currentAcceleration, ForceMode.Acceleration);
        // limit acceleration to max speed
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            hasDash=true;
            jumpCount = 0;
        }
    }
    public void OnJump()
    {
        if (jumpCount < 2)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
            isGrounded = false;
            jumpCount++;
        }
    }
    public void OnDash()
    {
        if (hasDash)
        {
            rb.AddForce(transform.forward * dashSpeed, ForceMode.VelocityChange);
            hasDash = false;
        }
    }

}
