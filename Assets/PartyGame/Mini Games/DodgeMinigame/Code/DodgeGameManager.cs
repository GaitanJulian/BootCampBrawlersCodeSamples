using MenteBacata.ScivoloCharacterControllerDemo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DodgeGameManager : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private RuntimeAnimatorController controller;


    private void Start()
    {
        PlayerManager.Instance.UpdateStartingPoints(spawnPoints);
        PlayerManager.Instance.SetPlayersPositions(0);
        AudioManager.Instance.StartBackgroundMusic(AudioManager.Instance.dodgeMusic);
        SetMovement();
    }

    private void SetMovement()
    {
        foreach (Transform t in spawnPoints)
        {
            PlayerInput player = t.gameObject.GetComponentInChildren<PlayerInput>();
            if (player != null)
            {
                player.GetComponentInChildren<DodgeMovementPlayerScript>().enabled = true;
                player.GetComponentInParent<Animator>().runtimeAnimatorController = controller;
            }

        }
    }

}
