using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f; 
    [SerializeField] private float rotationSpeed = 10f; 
    [SerializeField] private Camera Camera; 
    private Vector2 movementInput;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        var moveAction = InputActionReference.Create(InputSystem.ListEnabledActions()[0]);
        moveAction.action.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        moveAction.action.canceled += ctx => movementInput = Vector2.zero;
    }

    private void FixedUpdate()
    {
        Transform camTransform = Camera.main.transform;
        Vector3 camForward = camTransform.forward;
        camForward.y = 0f;
        if (camForward.sqrMagnitude > 0.001f)
        {
            // Determine the rotation that looks in the camera's forward direction
            Quaternion targetRotation = Quaternion.LookRotation(camForward);
            // Smoothly rotate the player towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        Vector3 localMovement = new Vector3(movementInput.x, 0, movementInput.y);
        // Transform the local movement to world space, based on the player's rotation
        Vector3 movement = transform.TransformDirection(localMovement) * speed;
        // Apply the movement to the Rigidbody's velocity
        rb.linearVelocity = movement;
    }
}
