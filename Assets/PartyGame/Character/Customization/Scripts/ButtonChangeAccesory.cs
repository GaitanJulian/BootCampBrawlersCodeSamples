using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangeAccesory : MonoBehaviour
{
    private CharacterCustomizationScript customizationScript;

    public CharacterCustomizationScript.ENUM_CustomizationType customizationType;
    public int direction;
    public Transform PositionContainer;
    private void OnEnable()
    {
        customizationScript = PositionContainer.GetComponentInChildren<CharacterCustomizationScript>();
    }

    private void OnDisable()
    {
        customizationScript = null;
    }

    public void ChangeAccesory()
    {
        customizationScript.NavigateCustomization(customizationType, direction);
    }
}
