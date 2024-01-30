using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCircuit3d : MonoBehaviour
{
    private InputActionAsset inputActions;
    private InputActionMap player;
    private InputAction move;

    private Rigidbody rb;

    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float maxSpeed = 5f;

    [SerializeField] private float coyoteTimeDuration = 0.02f;
    private float timeSinceGrounded;

    private Vector3 forceDirection = Vector3.zero;

    private Vector3 playerRoot;

    [SerializeField] Camera playerCamera;


    
    [SerializeField] private float waterSpeedReduction = 0.5f;
    private bool canJump = true;
    private bool isInWater = false;
    private bool isInitialized = false;

 

    public void EnableRuning()
    {
        inputActions = GetComponentInParent<PlayerInput>().actions;
        player = inputActions.FindActionMap("Player");
        rb = GetComponentInParent<Rigidbody>();
        player.FindAction("Jump").started += DoJump;
        move = player.FindAction("Movement");
        player.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isInitialized = true;
    }

    private void FixedUpdate()
    {
        if (isInitialized)
        {
            forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
            forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

            rb.AddForce(forceDirection, ForceMode.Impulse);
            forceDirection = Vector3.zero;

            if (rb.velocity.y < 0f)
                rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

            Vector3 horizontalVelocity = rb.velocity;
            horizontalVelocity.y = 0;
            if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
                rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;

            LookAt();
            /*
            if (IsGrounded())
            {
                canJump = true;
            }
            else
            {
                timeSinceGrounded += Time.fixedDeltaTime;
            }
            */
        }   
    }
  
    private void OnDestroy()
    {
        player.FindAction("Jump").started -= DoJump;
        player.Disable();
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }
    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }


    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void DoJump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            forceDirection += Vector3.up * jumpForce;
            canJump = false;
            timeSinceGrounded = coyoteTimeDuration;
        }
    }

    private bool IsGrounded()
    {
        Vector3 capsuleStart = transform.position + Vector3.up * 0.01f;
        float capsuleRadius = 0.20f;
        float capsuleLength = 0.2f;

        // Realiza un CapsuleCast en lugar de un Ray
        bool grounded = Physics.CapsuleCast(playerRoot, capsuleStart + Vector3.down * capsuleLength, capsuleRadius, Vector3.down, 0.3f);
        if (grounded)
        {
            timeSinceGrounded = 0f;
        }
        else
        {
            timeSinceGrounded += Time.fixedDeltaTime;
        }

        return grounded;
    }
    private void OnDrawGizmos()
    {
        // Establece el color de los Gizmos para el CapsuleCast
        Gizmos.color = Color.blue;

        Vector3 capsuleStart = transform.position + Vector3.up * 0.01f;
        float capsuleRadius = 0.20f;
        float capsuleLength = 0.2f;

        // Dibuja el CapsuleCast en la escena
        Gizmos.DrawWireSphere(capsuleStart, capsuleRadius);
        Gizmos.DrawWireSphere(capsuleStart + Vector3.down * capsuleLength, capsuleRadius);
        Gizmos.DrawLine(capsuleStart + Vector3.left * capsuleRadius, capsuleStart + Vector3.right * capsuleRadius);
        Gizmos.DrawLine(capsuleStart + Vector3.down * capsuleLength + Vector3.left * capsuleRadius, capsuleStart + Vector3.down * capsuleLength + Vector3.right * capsuleRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
            changeVelocity();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
            changeVelocity();
        }
    }
    private void changeVelocity()
    {
        if (isInWater) { rb.velocity *= waterSpeedReduction; }
        if (!isInWater) { rb.velocity = rb.velocity / waterSpeedReduction; }
    }
}

