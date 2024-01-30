using UnityEngine;

public class ButtonChangeSkin : MonoBehaviour
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

    public void ChangeSkin()
    {
        customizationScript.NavigateSkinType(1);
    }
}
