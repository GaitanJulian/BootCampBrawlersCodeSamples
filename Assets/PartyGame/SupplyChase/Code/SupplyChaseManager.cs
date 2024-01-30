using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SupplyChaseManager : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private InGameOverlayScript inGameOverlayScript;

    public int[] playerScore;

    public bool endGame = false;
    private WinnerText winText;

    [SerializeField] private RuntimeAnimatorController carAnimator;

    private void OnEnable()
    {
        inGameOverlayScript.startGame.AddListener(StartRace);
        inGameOverlayScript.quitGame.AddListener(FinishGame);
        AudioManager.Instance.StartBackgroundMusic(AudioManager.Instance.supplyChaseMusic);

    }

    private void OnDisable()
    {
        inGameOverlayScript.startGame.RemoveListener(StartRace);
        inGameOverlayScript.quitGame.RemoveListener(FinishGame);
    }

    private void StartRace()
    {
        ActivateMovement();

        StartCoroutine(EndGame());

        foreach (Transform t in spawnPoints)
        {
            Animator playerAnimator = t.GetComponentInChildren<Animator>();
            if (playerAnimator != null)
                playerAnimator.runtimeAnimatorController = carAnimator;
        }

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            playerScore[i] = 0;
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

    void Start()
    {
        PlayerManager.Instance.UpdateStartingPoints(spawnPoints);
        PlayerManager.Instance.SetPlayersPositions(0);

        winText = GameObject.Find("UIwin").GetComponent<WinnerText>();

        playerScore = new int[spawnPoints.Count];
    }

    private void ActivateMovement()
    {
        foreach (Transform t in spawnPoints)
        {
            FastLaneMovement movement = t.GetComponentInParent<FastLaneMovement>();
            if (t.GetComponentInChildren<PlayerInput>() != null)
            {
                movement.StartMovementBehavior();
            }
            else
            {
                movement.gameObject.SetActive(false);
            }
                

        }

    }

      

    public int ChooseWinner()
    {
        int highScore = 0;
        for (int i = 0; i < spawnPoints.Count - 1; i++)
        {
            if (playerScore[i] < playerScore[i + 1])
            {
                highScore = i + 1;
            }
            else
            {
                highScore = i;
            }
        }

        return highScore + 1;
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(40);

        endGame = true;

        int index = ChooseWinner();

        winText.ShowWinner(index);
;    }

    public void ScoreUpdate(int index)
    {
        playerScore[index]++;
    }
}
