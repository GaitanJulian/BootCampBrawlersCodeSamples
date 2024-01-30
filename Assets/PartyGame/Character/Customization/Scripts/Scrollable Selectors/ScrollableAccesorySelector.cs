public class ScrollableAccesorySelector : ScrollableSelector
{
    public CharacterCustomizationScript.ENUM_CustomizationType customizationType;

    public override void ScrollLeft()
    {
        customizationScript.NavigateCustomization(customizationType, -1);
        OnScroll(leftArrow);
    }

    public override void ScrollRight()
    {
        customizationScript.NavigateCustomization(customizationType, 1);
        OnScroll(rightArrow);
    }
}
