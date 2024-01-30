using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FlagScript : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent finishRace;
    [SerializeField] TextMeshProUGUI winnerText;
    [SerializeField] GameObject winnerCanvasGO;
    private int player;
    private bool finished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (finished)
            return;
        finished = true;

        finishRace?.Invoke();
        player = other.GetComponentInParent<PlayerInput>().playerIndex;

        // Display the winner player on the UI Text
        winnerText.text = "Player " + (player + 1).ToString() + " wins!"; // Assuming playerIndex starts from 0

        winnerCanvasGO.SetActive(true);

        // Reset the scale to 0 before the scaling animation
        winnerText.transform.localScale = Vector3.zero;

        // Animate the text scale from 0 to 1 over 1 second
        winnerText.transform.DOScale(1, 1);

  
    }


}
