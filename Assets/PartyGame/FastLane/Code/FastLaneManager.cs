using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FastLaneManager : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private InGameOverlayScript inGameOverlayScript;

    public int[] actualCheckPointPlayer;

    public int[] remainingLapsPlayers;

    public bool EndGame = false;

    private WinText winText;

    [SerializeField] private RuntimeAnimatorController carAnimator;

    //  private PlayerInputManager inputManager;

    private void OnEnable()
    {
        inGameOverlayScript.startGame.AddListener(StartRace);
        inGameOverlayScript.quitGame.AddListener(FinishGame);
        AudioManager.Instance.StartBackgroundMusic(AudioManager.Instance.fastLaneRaceMusic);
    }

    private void OnDisable()
    {
        inGameOverlayScript.startGame.RemoveListener(StartRace);
        inGameOverlayScript.quitGame.RemoveListener(FinishGame);
    }


    void Start()
    {
        PlayerManager.Instance.UpdateStartingPoints(spawnPoints);
        PlayerManager.Instance.SetPlayersPositions(0);

        winText = GameObject.Find("UIwin").GetComponent<WinText>();

        actualCheckPointPlayer = new int[spawnPoints.Count];
        remainingLapsPlayers = new int[spawnPoints.Count];
    }

    private void StartRace()
    {
        ActivateMovement();

        foreach (Transform t in spawnPoints)
        {
            Animator playerAnimator = t.GetComponentInChildren<Animator>();
            if (playerAnimator != null)
                playerAnimator.runtimeAnimatorController = carAnimator;
        }

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            actualCheckPointPlayer[i] = 0;
            remainingLapsPlayers[i] = 5;
        }
    }


    // All boogie karts are Active at the beggining of the scene
    // We need to disable the karts that dont have a player
    private void ActivateMovement()
    {
        foreach (Transform t in spawnPoints)
        {
           FastLaneMovement movement = t.GetComponentInParent<FastLaneMovement>();
            if (t.GetComponentInChildren<PlayerInput>() != null)
                movement.StartMovementBehavior();
            else
                movement.gameObject.SetActive(false);
                
        }
        
    }

    public void Update1(int index)
    {
        actualCheckPointPlayer[index] = 1;
    }

    public void Update2(int index)
    {
        actualCheckPointPlayer[index] = 2;
    }

    public void LapUpdate(int index)
    {
        actualCheckPointPlayer[index] = 0;
        remainingLapsPlayers[index]--;
        if (remainingLapsPlayers[index] == 0)
        {
            EndGame = true;
            winText.ShowWinner(index);
        }
    }

    public void FinishGame()
    {
        foreach (Transform t in spawnPoints)
        {
            FastLaneMovement movement = t.GetComponentInParent<FastLaneMovement>();
            if (t.GetComponentInChildren<PlayerInput>() != null)
                movement.StopMovementBehavior();
   

        }

        PlayerManager.Instance.PreparePlayersForSceneChange();
        SceneManager.LoadScene("MinigameSelect", LoadSceneMode.Single);
    }
}
