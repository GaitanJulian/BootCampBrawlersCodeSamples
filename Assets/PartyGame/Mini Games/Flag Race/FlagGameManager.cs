using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FlagGameManager : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private GameObject flag;
    [SerializeField] private RuntimeAnimatorController flagRaceAnimator;
    [SerializeField] private StartRaceScript startRaceScript;
    [SerializeField] private InGameOverlayScript inGameOverlayScript;

    public bool hasWinner = false;
    private bool isFinished = false;

    private void OnEnable()
    {
        startRaceScript.startRace.AddListener(StartRace);
        inGameOverlayScript.quitGame.AddListener(FinishGame);
    }

    private void OnDisable()
    {
        startRaceScript.startRace.RemoveListener(StartRace);
        inGameOverlayScript.quitGame.RemoveListener(FinishGame);
    }

    private void OnDestroy()
    {
        startRaceScript.startRace.RemoveListener(StartRace);
        inGameOverlayScript.quitGame.RemoveListener(FinishGame);
    }

    private void Start()
    {
        hasWinner = false;
        PlayerManager.Instance.UpdateStartingPoints(spawnPoints);
        PlayerManager.Instance.SetPlayersPositions(0);
        AudioManager.Instance.StartBackgroundMusic(AudioManager.Instance.flagRaceMusic);
        foreach (Transform t in spawnPoints)
        {
            t.LookAt(flag.transform.position);
            Animator playerAnimator = t.GetComponentInChildren<Animator>();
            if (playerAnimator != null)
            {
                playerAnimator.runtimeAnimatorController = flagRaceAnimator;
                playerAnimator.Play("idle");
            }
                
        }
    }

    public void StartRace()
    {
        foreach (Transform t in spawnPoints)
        {
            PlayerInput player = t.gameObject.GetComponentInChildren<PlayerInput>();
            if (player != null)
            {
                player.GetComponentInChildren<FlagRaceController>().enabled = true;
            }
        }
    }

    public void FinishGame()
    {
        if (!isFinished)
        {
            isFinished = true;
            foreach (Transform t in spawnPoints)
            {
                PlayerInput player = t.gameObject.GetComponentInChildren<PlayerInput>();
                if (player != null)
                {
                    FlagRaceController playerController = player.GetComponentInChildren<FlagRaceController>();
                    playerController.FinishGame();
                    playerController.ResetPosition();
                    playerController.enabled = false;
                }
            }
            PlayerManager.Instance.PreparePlayersForSceneChange();
            SceneManager.LoadScene("MinigameSelect", LoadSceneMode.Single);
        }
        
    }
}
