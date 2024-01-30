
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, AxisState.IInputAxisProvider
{
    private InputActionAsset inputActionAsset;
    private InputActionMap actionMap;
    [HideInInspector]
    public InputAction horizontal;
    [HideInInspector]
    public InputAction vertical;

    InGameOverlayScript pauseScript;

    /*
    public Transform target;
    public float verticalOffset = 0f;
    public float distance = 5f;
    public float sensitivity = 100f;

    private InputAction cameraMoveAction;
    

    private bool isStarted = false;
    private float yRot = 0f;
    private float xRot = 20f;

    */


    private void Start()
    {
        StartCharacterController();
        pauseScript = FindObjectOfType<InGameOverlayScript>();
    }


    public void StartCharacterController()
    {
        inputActionAsset = GetComponentInParent<PlayerInput>().actions;
        actionMap = inputActionAsset.FindActionMap("Player");
        horizontal = actionMap.FindAction("Look");
        actionMap.Enable();
        //isStarted = true;
/*
#if UNITY_EDITOR
        sensitivity *= 10f;
#endif
*/
    }
    public float GetAxisValue(int axis)
    {
        Vector2 input = horizontal.ReadValue<Vector2>();
        input = Vector2.ClampMagnitude(input, 1);
        switch (axis)
        {
            case 0: return input.x;
            case 1: return input.y;
            case 2: return vertical.ReadValue<float>();
        }

        return 0;
    }
    /*
    private void LateUpdate()
    {
        if (isStarted)
        {
            Vector2 mouseDelta = cameraMoveAction.ReadValue<Vector2>();
            yRot += mouseDelta.x * sensitivity * Time.deltaTime;
            xRot -= mouseDelta.y * sensitivity * Time.deltaTime;
            xRot = Mathf.Clamp(xRot, 0f, 75f);

            Quaternion worldRotation = transform.parent != null ? transform.parent.rotation : Quaternion.FromToRotation(Vector3.up, target.up);
            Quaternion cameraRotation = worldRotation * Quaternion.Euler(xRot, yRot, 0f);
            Vector3 targetToCamera = cameraRotation * new Vector3(0f, 0f, -distance);

            transform.SetPositionAndRotation(target.TransformPoint(0f, verticalOffset, 0f) + targetToCamera, cameraRotation);
        }
    }*/
}
