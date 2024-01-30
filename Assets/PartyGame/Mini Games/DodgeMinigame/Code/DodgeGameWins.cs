using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DodgeGameWins : MonoBehaviour
{
    public float timeOfFinalAnimation = 1.0f;
    public GameObject[] players;
    public int playersActive;

    private void Start()
    {
        StartCoroutine(InvokePlayersOnSceneAfterDelay(4f));
        
    }

    private void LateUpdate()
    {
        
    }

    private IEnumerator InvokePlayersOnSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayersOnScene();
        StartCoroutine(CheckPlayersPeriodically(3f));
    }

    public void PlayersOnScene()
    {
        playersActive = 0;
        foreach (GameObject player in players)
        {
            Transform playerTransform = player.transform;
            if (player != null && playerTransform.childCount <= 0)
            {
                Debug.Log("Player Vacío");
            }
            if (player != null && playerTransform.childCount >= 1)
            {
                Debug.Log("Player Activo");
                playersActive++;
            }
        }
        ValidIfWin();
    }
    private IEnumerator CheckPlayersPeriodically(float checkInterval)
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);
            PlayersOnScene();
        }
    }

    public void Victory()
    {
        Debug.Log("Finalizando");
    }

    public void ValidIfWin()
    {
        if (playersActive == 0)
        {
            StartCoroutine(ChangeScene());
        }
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(timeOfFinalAnimation);
        SceneManager.LoadScene("MinigameSelect");
    }
}
