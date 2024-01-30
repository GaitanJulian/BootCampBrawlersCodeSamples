using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartPartyListener : MonoBehaviour
{
    [SerializeField] private List<string> scenes; // A list of scene names
    [SerializeField] private int sceneIndex; // Index of the scene to load in the list
    [SerializeField] private Animator cinemachineAnimator;

    void Start()
    {
        PlayerManager.Instance.OnPlayersReady += ChangeScene;
    }

    private void OnDisable()
    {
        PlayerManager.Instance.OnPlayersReady -= ChangeScene;
    }

    private void ChangeScene()
    {
        cinemachineAnimator.Play("MainMenu");

        // Ensure we have a valid scene index
        if (sceneIndex >= 0 && sceneIndex < scenes.Count)
        {
            // Load the scene at the current index
            SceneManager.LoadScene(scenes[sceneIndex], LoadSceneMode.Single);
        }
        PlayerManager.Instance.PreparePlayersForSceneChange();
        PlayerManager.Instance.GetComponent<PlayerInputManager>().DisableJoining();
    }
}