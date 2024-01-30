using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlagRaceController : MonoBehaviour
{
    private InputActionAsset inputActions;
    private InputActionMap player;

    private Animator animator;
    private FlagScript flagScript;

    [SerializeField] private float baseTimeToReachFlag = 10f;
    [SerializeField] private float minTimeToReachFlag = 5f;
    public Transform flag;

    private float distanceToFlag;
    private float currentSpeed;
    private float maxSpeed;
    private float speedDecay = 0.1f; // Adjust this value for desired speed decay
    private float buttonPressWeight = 0.5f;
    private Rigidbody rb;
    private bool isButtonPressed;
    private bool isInitialized;
    private bool isDiving = false;
    private bool isGameFinished = false;

    private IEnumerator diveCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        player.FindAction("Jump").started -= DoJump;
        player.FindAction("Jump").canceled -= StopJump;
        player.FindAction("Run").started -= DoDive;
        player.Disable();
        flagScript.finishRace.RemoveListener(FinishGame);
    }
/*
    private void OnDestroy()
    {
        if (player != null)
        {
            player.FindAction("Jump").started -= DoJump;
            player.FindAction("Jump").canceled -= StopJump;
            player.FindAction("Run").started -= DoDive;
            player.Disable();
        }
        
        if (flagScript != null)
        flagScript.finishRace.RemoveListener(FinishGame);
    }
*/
    private void OnEnable()
    {
        diveCoroutine = MoveDuringDive();
        flag = FindObjectOfType<FlagScript>().transform;
        flagScript = FindObjectOfType<FlagScript>();
        flagScript.finishRace.AddListener(FinishGame);
        // POR SI NO FUNCIONA
        distanceToFlag = Vector3.Distance(transform.position, flag.position);
        currentSpeed = 0f;
        maxSpeed = distanceToFlag / minTimeToReachFlag;
        animator = GetComponentInParent<Animator>();
        isGameFinished = false;
        animator.SetFloat("Speed", 0);
        EnableRuning();

    }

    public void EnableRuning()
    {
        inputActions = GetComponentInParent<PlayerInput>().actions;
        player = inputActions.FindActionMap("Player");
        player.FindAction("Jump").started += DoJump;
        player.FindAction("Jump").canceled += StopJump;
        player.FindAction("Run").started += DoDive;
        player.Enable();


        isInitialized = true;
    }

    private void Update()
    {
        if (isInitialized && !isDiving && !isGameFinished) 
        {
            if (isButtonPressed && currentSpeed < maxSpeed)
            {
                currentSpeed += buttonPressWeight;
            }
            else if (!isButtonPressed && currentSpeed > distanceToFlag / baseTimeToReachFlag)
            {
                currentSpeed -= speedDecay;
            }

            float normalizedSpeed = Mathf.InverseLerp(0f, maxSpeed * 0.8f, currentSpeed);
            animator.SetFloat("Speed", normalizedSpeed);
        }
    }

    private void FixedUpdate()
    { 
        if (isInitialized && !isDiving && !isGameFinished) 
        {
            Vector3 direction = (flag.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * (currentSpeed * Time.fixedDeltaTime));
        }
    }

    public void DoJump(InputAction.CallbackContext context)
    {
        isButtonPressed = true;

    }

    public void StopJump(InputAction.CallbackContext context)
    {
        isButtonPressed = false;
    }

    public void DoDive(InputAction.CallbackContext context)
    {
        if (!isDiving && !isGameFinished)
        {
            isDiving = true;
            animator.SetTrigger("Dive");
            StartCoroutine(diveCoroutine);
            AudioManager.Instance.PlayRandomJumpSound(gameObject.transform);
        }
    }

    IEnumerator MoveDuringDive()
    {
        print("corutina");
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + (flag.position - transform.position).normalized * (currentSpeed * 1.5f);

        float elapsedTime = 0;
        while (elapsedTime < 1.15f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / 1.15f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }

    public void FinishGame()
    {
        if(diveCoroutine != null)
        {
            StopCoroutine(diveCoroutine);
        }
        isGameFinished = true;
        isDiving = false;
        isButtonPressed = false;

        if (animator != null)
        animator.SetFloat("Speed", 0);
    }

    public void ResetPosition()
    {
        transform.localPosition = Vector3.zero;
    }
}