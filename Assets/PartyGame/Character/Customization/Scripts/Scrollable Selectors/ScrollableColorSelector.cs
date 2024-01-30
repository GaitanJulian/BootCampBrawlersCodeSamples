public class ScrollableColorSelector : ScrollableSelector
{
    public override void ScrollLeft()
    {
        customizationScript.NavigateMaterialType(false);
        OnScroll(leftArrow);
    }

    public override void ScrollRight()
    {
        customizationScript.NavigateMaterialType(true);
        OnScroll(rightArrow);
    }
}
