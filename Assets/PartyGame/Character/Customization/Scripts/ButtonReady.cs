using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonReady : MonoBehaviour
{
    private PlayerInput player;
    private CustomizationCanvas customizationCanvas;
    public Transform PositionContainer;

    private Color readyColor = new Color(0.007f, 1f, 0.012f); // 01FF03
    private Color notReadyColor = Color.white; // FFFFFF
    private Button button;


    private void Start()
    {
        customizationCanvas = GetComponentInParent<CustomizationCanvas>();
        button = GetComponent<Button>();   
    }

    private void OnEnable()
    {
        player = PositionContainer.GetComponentInChildren<PlayerInput>();

    }

    private void OnDisable()
    {
        player = null;
    }

    public void ChangePlayerStatus()
    {
        AudioManager.Instance.PlaySfxPlayer(AudioManager.Instance.select); // SELECT OR NEXT
        PlayerManager.Instance.ChangePlayerReadyStatus(player);
        customizationCanvas.ChangePlayerStatus();

        // Change the button color based on the player's ready status
        ColorBlock colorBlock = button.colors;
        if (PlayerManager.Instance.GetPlayerReadyStatus(player))
        {
            colorBlock.normalColor = readyColor;
        }
        else
        {
            colorBlock.normalColor = notReadyColor;
        }
        button.colors = colorBlock;

        PlayerManager.Instance.AreAllPlayersReady();
    }
}
