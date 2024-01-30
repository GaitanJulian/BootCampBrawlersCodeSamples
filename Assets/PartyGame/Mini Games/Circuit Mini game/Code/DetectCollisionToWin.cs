using Cinemachine;
using MenteBacata.ScivoloCharacterControllerDemo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DetectCollisionToWin : MonoBehaviour
{
    public GameObject player;
    public bool collisionOccurred = false;
    public GameObject sirPlane;
    public Transform positionOfPlayerWins;
    public bool playerWin;
    public GameObject cameraOrbit;
    private bool stopFollowing = false;

    public float timeAnimationFinal = 7.0f;
    private void LateUpdate()
    {
        if (collisionOccurred && playerWin)
        {
            collisionOccurred = false;
            sirPlane.GetComponent<Animator>().enabled = true;
            StartCoroutine(ChangeScene());
        }
        if (playerWin && !stopFollowing)
        {
            player.transform.position = positionOfPlayerWins.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.gameObject;
        collisionOccurred = true; 
        

        SimpleCharacterController playerController = player.GetComponent<SimpleCharacterController>();
        CinemachineFreeLook camera = player.GetComponentInParent<Transform>().parent.GetComponentInChildren<CinemachineFreeLook>();

        //GetComponent<Collider>().enabled = false;
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        Debug.Log(camera.name);
        camera.Follow = cameraOrbit.transform;
        camera.LookAt = cameraOrbit.transform;

        playerWin = true;
        //gameObject.SetActive(false);
    }
    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(7.0f);
        stopFollowing = true;
        sirPlane.GetComponent<Animator>().enabled = false;
        FindAnyObjectByType<Race3DManager>().FinishGame();
    }
}
