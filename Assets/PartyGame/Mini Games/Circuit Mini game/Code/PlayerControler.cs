using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{

    //private PlayerControlls playerActions;

    private InputActionAsset inputActions;
    private InputActionMap player;
    private InputAction move;

    private Rigidbody rb;

    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float maxSpeed = 5f;


    private Vector3 forceDirection = Vector3.zero;


    [SerializeField] Camera playerCamera;


    public event Action<PlayerInput> onPlayerJoined;
    public event Action<PlayerInput> onPlayerLeft;



    private bool canJump = false;
    private float coyoteTime = 0.4f; 
    private float coyoteTimer = 0f;


    private void Awake()
    {
        inputActions = GetComponent<PlayerInput>().actions;
        player = inputActions.FindActionMap("Player");
        rb = GetComponent<Rigidbody>();
        //playerActions = new PlayerControlls();
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

        if (IsGrounded())
        {
            coyoteTimer = coyoteTime;
            canJump = true;
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
            if (coyoteTimer <= 0)
            {
                canJump = false;
            }
        }
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
        player.FindAction("Jump").started += DoJump;
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
        if (canJump)
        {
            forceDirection += Vector3.up * jumpForce;
            canJump = false;
        }
    }

    private bool IsGrounded()
    {
        Vector3 capsuleStart = transform.position + Vector3.up * 0.1f;
        float capsuleRadius = 0.3f;
        RaycastHit hit;
        float capsuleLength = 0.3f; // Longitud del CapsuleCast

        if (Physics.CapsuleCast(capsuleStart, capsuleStart - Vector3.up * capsuleLength, capsuleRadius, Vector3.down, out hit))
        {
            return true;
        }

        return false;
    }
}
