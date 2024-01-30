using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LoadSceneScript : MonoBehaviour
{
    [SerializeField] private List<string> scenes; // A list of scene names
    [SerializeField] private int sceneIndex; // Index of the scene to load in the list


    public void ChangeScene()
    {
        AudioManager.Instance.PlaySfxPlayer(AudioManager.Instance.select); // SELECT OR NEXT
        PlayerManager.Instance.PreparePlayersForSceneChange();

        // Ensure we have a valid scene index
        if (sceneIndex >= 0 && sceneIndex < scenes.Count)
        {
            // Load the scene at the current index
            SceneManager.LoadScene(scenes[sceneIndex], LoadSceneMode.Single);
        }
    }

    public void MainMenu()
    {
        AudioManager.Instance.PlaySfxPlayer(AudioManager.Instance.unselect); //UNSELECT OR BACK
        PlayerManager.Instance.RemoveAllPlayers();
  
        // Ensure we have a valid scene index
        if (sceneIndex >= 0 && sceneIndex < scenes.Count)
        {
            // Load the scene at the current index
            SceneManager.LoadScene(scenes[sceneIndex], LoadSceneMode.Single);
        }
    }
}
