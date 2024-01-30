using Cinemachine;
using MenteBacata.ScivoloCharacterControllerDemo;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Race3DManager : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private GameObject prefabPlayer;
    [SerializeField] private RuntimeAnimatorController controller;
    [SerializeField] private InGameOverlayScript inGameOverlayScript;
    private void Start()
    {
        PlayerManager.Instance.UpdateStartingPoints(spawnPoints);
        PlayerManager.Instance.SetPlayersPositions(180);
        AudioManager.Instance.StartBackgroundMusic(AudioManager.Instance.circuitMusic);

    }

    private void OnEnable()
    {
        inGameOverlayScript.startGame.AddListener(StartRace);
        inGameOverlayScript.quitGame.AddListener(FinishGame);
    }

    private void OnDisable()
    {
        inGameOverlayScript.startGame.RemoveListener(StartRace);
        inGameOverlayScript.quitGame.RemoveListener(FinishGame);
    }

    private void StartRace()
    {
        SetMovement();

        PlayerManager.Instance.SetCameraLayers();
        SetCameras();
    }

    public void FinishGame()
    {
        TurnOffMovement();
        PlayerManager.Instance.PreparePlayersForSceneChange();
        SceneManager.LoadScene("MinigameSelect", LoadSceneMode.Single);
    }

    private void SetMovement()
    {
        foreach (Transform t in spawnPoints)
        {
            PlayerInput player = t.gameObject.GetComponentInChildren<PlayerInput>();
            if (player != null) 
            {
                player.GetComponentInChildren<SimpleCharacterController>().enabled = true;
                player.GetComponentInParent<Animator>().runtimeAnimatorController = controller;
            }

        }
    }

    private void SetCameras()
    {
        foreach (Transform t in spawnPoints)
        {
            PlayerInput player = t.gameObject.GetComponentInChildren<PlayerInput>();
            if (player != null)
            {
                GameObject characterControll = player.GetComponentInChildren<SimpleCharacterController>().gameObject;
                CinemachineFreeLook camera = player.GetComponentInChildren<CinemachineFreeLook>();
                camera.LookAt = characterControll.transform;
                camera.Follow = characterControll.transform;

            }

        }
    }

    private void TurnOffMovement()
    {
        foreach (Transform t in spawnPoints)
        {
            PlayerInput player = t.gameObject.GetComponentInChildren<PlayerInput>();
            if (player != null)
            {
                SimpleCharacterController playerController = player.GetComponentInChildren<SimpleCharacterController>();
                playerController.ResetPosition();
                playerController.enabled = false;
            }

        }
    }

}
