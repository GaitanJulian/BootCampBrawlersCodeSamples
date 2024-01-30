using MenteBacata.ScivoloCharacterController;
using MenteBacata.ScivoloCharacterControllerDemo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Otro : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public float jumpSpeed = 8f;
    public float rotationSpeed = 720f;
    public float gravity = -25f;

    [Header("RigidbodyAndGroundDetector")]
    public CharacterCapsule capsule;
    public CharacterMover mover;
    public GroundDetector groundDetector;
    public MeshRenderer groundedIndicator;

    [Header("Input System")]
    private InputActionAsset inputActionsAsset;
    private InputActionMap actionMap;
    private InputAction moveAction;
    private InputAction jumpAction;


    private const float minVerticalSpeed = -12f;
    private const float timeBeforeUngrounded = 0.4f;
    private float verticalSpeed = 0f;
    private float nextUngroundedTime = -1f;
    private Transform cameraTransform;
    private Collider[] overlaps = new Collider[5];
    private int overlapCount;
    private MoveContact[] moveContacts = CharacterMover.NewMoveContactArray;
    private int contactCount;
    private bool isOnMovingPlatform = false;
    private MovingPlatform movingPlatform;


    [Header("Audio")]
    [SerializeField] AudioManager audioManager;
    private bool isMoving = false;

    private void Start()
    {

        cameraTransform = transform.parent.GetComponentInChildren<Camera>().transform;
        mover.canClimbSteepSlope = true;
        StartCharacterController();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void StartCharacterController()
    {
        inputActionsAsset = GetComponentInParent<PlayerInput>().actions;
        actionMap = inputActionsAsset.FindActionMap("Player");

        moveAction = actionMap.FindAction("Movement");
        jumpAction = actionMap.FindAction("Jump");
        jumpAction.started += OnJump;
        actionMap.Enable();
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (Time.time < nextUngroundedTime)
        {
            verticalSpeed = jumpSpeed;
            nextUngroundedTime = -1f;

            audioManager.PlayRandomJumpSound(transform);
        }
    }

    private void OnDestroy()
    {
        if (jumpAction != null)
            jumpAction.started -= OnJump;
        if (moveAction != null)
            moveAction.Disable();

    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        Vector3 movementInput = GetMovementInput();
        Vector3 velocity = moveSpeed * movementInput;
        HandleOverlaps();

        bool groundDetected = DetectGroundAndCheckIfGrounded(out bool isGrounded, out GroundInfo groundInfo);
        SetGroundedIndicatorColor(isGrounded);
        isOnMovingPlatform = false;

        //isMoving = movementInput != Vector3.zero;
        //IsMoving();

        if (isGrounded)
        {
            mover.mode = CharacterMover.Mode.Walk;
            verticalSpeed = 0f;
            if (groundDetected)
                isOnMovingPlatform = groundInfo.collider.TryGetComponent(out movingPlatform);

        }
        else
        {
            mover.mode = CharacterMover.Mode.SimpleSlide;
            BounceDownIfTouchedCeiling();
            verticalSpeed += gravity * deltaTime;
            if (verticalSpeed < minVerticalSpeed)
                verticalSpeed = minVerticalSpeed;
            velocity += verticalSpeed * transform.up;
        }

        RotateTowards(velocity);
        mover.Move(velocity * deltaTime, groundDetected, groundInfo, overlapCount, overlaps, moveContacts, out contactCount);
    }

    private Vector3 GetMovementInput()
    {
        Vector2 inputVec = moveAction.ReadValue<Vector2>();
        float x = inputVec.x;
        float y = inputVec.y;
        Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, transform.up).normalized;
        Vector3 right = Vector3.Cross(transform.up, forward);
        return x * right + y * forward;
    }

    private void LateUpdate()
    {
        if (isOnMovingPlatform)
            ApplyPlatformMovement(movingPlatform);
    }


    private void HandleOverlaps()
    {
        if (capsule.TryResolveOverlap())
        {
            overlapCount = 0;
        }
        else
        {
            overlapCount = capsule.CollectOverlaps(overlaps);
        }
    }

    private bool DetectGroundAndCheckIfGrounded(out bool isGrounded, out GroundInfo groundInfo)
    {
        bool groundDetected = groundDetector.DetectGround(out groundInfo);

        if (groundDetected)
        {
            if (groundInfo.isOnFloor && verticalSpeed < 0.1f)
                nextUngroundedTime = Time.time + timeBeforeUngrounded;
        }
        else
            nextUngroundedTime = -1f;

        isGrounded = Time.time < nextUngroundedTime;
        return groundDetected;
    }

    private void SetGroundedIndicatorColor(bool isGrounded)
    {
        if (groundedIndicator != null)
            groundedIndicator.material.color = isGrounded ? Color.green : Color.blue;
    }

    private void RotateTowards(Vector3 direction)
    {
        Vector3 flatDirection = Vector3.ProjectOnPlane(direction, transform.up);

        if (flatDirection.sqrMagnitude < 1E-06f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(flatDirection, transform.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ApplyPlatformMovement(MovingPlatform movingPlatform)
    {
        GetMovementFromMovingPlatform(movingPlatform, out Vector3 movement, out float upRotation);

        transform.Translate(movement, Space.World);
        transform.Rotate(0f, upRotation, 0f, Space.Self);
    }

    private void GetMovementFromMovingPlatform(MovingPlatform movingPlatform, out Vector3 movement, out float deltaAngleUp)
    {
        movingPlatform.GetDeltaPositionAndRotation(out Vector3 platformDeltaPosition, out Quaternion platformDeltaRotation);
        Vector3 localPosition = transform.position - movingPlatform.transform.position;
        movement = platformDeltaPosition + platformDeltaRotation * localPosition - localPosition;

        platformDeltaRotation.ToAngleAxis(out float platformDeltaAngle, out Vector3 axis);
        float axisDotUp = Vector3.Dot(axis, transform.up);

        if (-0.1f < axisDotUp && axisDotUp < 0.1f)
            deltaAngleUp = 0f;
        else
            deltaAngleUp = platformDeltaAngle * Mathf.Sign(axisDotUp);
    }

    private void BounceDownIfTouchedCeiling()
    {
        for (int i = 0; i < contactCount; i++)
        {
            if (Vector3.Dot(moveContacts[i].normal, transform.up) < -0.7f)
            {
                verticalSpeed = -0.25f * verticalSpeed;
                break;
            }
        }
    }

    private void IsMoving()
    {
        if (isMoving)
            audioManager.StartWalkingOnSand(transform);
        else
            audioManager.StopWalkingOnSand();
    }
}

