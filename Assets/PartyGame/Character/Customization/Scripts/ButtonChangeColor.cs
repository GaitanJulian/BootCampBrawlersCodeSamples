using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangeColor : MonoBehaviour
{
    private CharacterCustomizationScript customizationScript;
    public Transform PositionContainer;
    private void OnEnable()
    {
        customizationScript = PositionContainer.GetComponentInChildren<CharacterCustomizationScript>();
    }

    private void OnDisable()
    {
        customizationScript = null;
    }

    public void ChangeColor()
    {
        customizationScript.NavigateMaterialType(true);
    }
}
