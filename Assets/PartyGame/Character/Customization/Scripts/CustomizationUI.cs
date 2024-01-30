using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationUI : MonoBehaviour
{
    private CharacterCustomizationScript customizationScript; // Reference to your customization script

    private void Start()
    {
        customizationScript = FindObjectOfType<CharacterCustomizationScript>();
    }

    public void OnNextNavigationClick(CharacterCustomizationScript.ENUM_CustomizationType customizationType, int direction)
    {
        customizationScript.NavigateCustomization(customizationType, direction);
    }

    public void OnNextCharacterClick(int direction)
    {
        customizationScript.NavigateCustomization(CharacterCustomizationScript.ENUM_CustomizationType.Character, direction);
    }

    public void OnNextArmorClick(int direction)
    {
        customizationScript.NavigateCustomization(CharacterCustomizationScript.ENUM_CustomizationType.Armor, direction);
    }

    public void OnNextBackPackClick(int direction)
    {
        customizationScript.NavigateCustomization(CharacterCustomizationScript.ENUM_CustomizationType.Backpack, direction);
    }
    
    public void OnNextHatClick(int direction) 
    {
        customizationScript.NavigateCustomization(CharacterCustomizationScript.ENUM_CustomizationType.Hat, direction);
    }

    public void OnChangeMaterialClick(bool next)
    {
        customizationScript.NavigateMaterialType(next);
    }

    public void OnChangeSkinClick(int direction)
    {
        customizationScript.NavigateSkinType(direction);
    }
}
