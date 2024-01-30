using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TherdController : MonoBehaviour
{

    //private PlayerControlls playerActions;

    private InputActionAsset inputActions;
    private InputActionMap player;
    private InputAction move;

    private Rigidbody rb;

    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float maxSpeed = 5f;

    [SerializeField] private float coyoteTimeDuration = 0.2f;
    private float timeSinceGrounded;

    private Vector3 forceDirection = Vector3.zero;


    [SerializeField] Camera playerCamera;


    public event Action<PlayerInput> onPlayerJoined;
    public event Action<PlayerInput> onPlayerLeft;


    [SerializeField] private float waterSpeedReduction = 0.5f;

    private bool isInWater = false;

    private void Awake()
    {
        inputActions = GetComponent<PlayerInput>().actions;
        player = inputActions.FindActionMap("Player"); 
        rb = GetComponent<Rigidbody>();
        //playerActions = new PlayerControlls();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void FixedUpdate()
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
    }
    private void OnEnable()
    {
        //playerActions.Player.Jump.started += DoJump;
        //move = playerActions.Player.Movement;
        //playerActions.Player.Enable();

        player.FindAction("Jump").started += DoJump;
        move = player.FindAction("Movement");
        player.Enable();
    }
    private void OnDisable()
    {
        //playerActions.Player.Jump.started -= DoJump;
        //playerActions.Player.Disable();
        player.FindAction("Jump").started -= DoJump;
        player.Enable();
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
        }
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        bool grounded = Physics.Raycast(ray, out RaycastHit hit, 0.3f);

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
        if (isInWater) { rb.velocity *= waterSpeedReduction;}
        if (!isInWater) { rb.velocity = rb.velocity / waterSpeedReduction;}
    }
}
