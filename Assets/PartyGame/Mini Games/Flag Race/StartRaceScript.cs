using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartRaceScript : MonoBehaviour
{
    private InGameOverlayScript tutorialOverlay;

    public ParticleSystem flareShot;
    public UnityEngine.Events.UnityEvent startRace;


    private void Awake()
    {
        tutorialOverlay = FindObjectOfType<InGameOverlayScript>();
    }

    private void OnEnable()
    {
        tutorialOverlay.startGame.AddListener(StartRace);
    }

    private void OnDisable()
    {
        tutorialOverlay.startGame.RemoveListener(StartRace);
    }

    public void StartRace()
    {
        StartCoroutine(StartRaceCoroutine());
    }

    IEnumerator StartRaceCoroutine()
    {

        yield return new WaitForSeconds(Random.Range(5, 7));

        // Play the particle system
        flareShot.Play();
        AudioManager.Instance.playSoundFlare();
        // Trigger the event
        startRace.Invoke();
        
    }
}
