using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;

public class ButtonQuit : MonoBehaviour
{
    private PlayerInput player;
    private CustomizationCanvas customizationCanvas;
    public Transform PositionContainer;
    public GameObject canvasGO;

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

    public void PlayerQuit()
    {
        AudioManager.Instance.PlaySfxPlayer(AudioManager.Instance.unselect); //UNSELECT OR BACK
        ColorBlock currColorBlock = button.colors;
        currColorBlock.normalColor = Color.white;
        button.colors = currColorBlock;
        PlayerManager.Instance.RemovePlayer(player);
        customizationCanvas.TurnOffCanvas();
    }
}
