using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private CinemachineCamera Camera;
    [SerializeField] private float jumpHeight = 5f;
    private Vector2 moveInput;
    private Rigidbody rb;
    private Boolean isGrounded = true;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        RotationUpdate();
        MovementUpdate();
    }
    private void FixedUpdate()
    {
        
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
        Vector3 targetPosition = rb.position + moveDirection * speed * Time.deltaTime;
        rb.MovePosition(targetPosition);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    public void OnJump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            isGrounded = false;
        }
    }
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
