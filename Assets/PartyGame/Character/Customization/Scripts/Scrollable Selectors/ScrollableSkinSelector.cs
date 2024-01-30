public class ScrollableSkinSelector : ScrollableSelector
{
    public override void ScrollLeft()
    {
        customizationScript.NavigateSkinType(-1);
        OnScroll(leftArrow);
    }

    public override void ScrollRight()
    {
        customizationScript.NavigateSkinType(1);
        OnScroll(rightArrow);
    }

}
