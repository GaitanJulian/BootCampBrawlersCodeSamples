using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class ScrollableSelector : MonoBehaviour
{
    protected CharacterCustomizationScript customizationScript;
    [SerializeField] protected Transform positionContainer;
    [SerializeField] protected Image leftArrow;
    [SerializeField] protected Image rightArrow;

    protected void OnEnable()
    {
            customizationScript = positionContainer.GetComponentInChildren<CharacterCustomizationScript>();   
    }

    public abstract void ScrollLeft();

    public abstract void ScrollRight();

    public void OnScroll(Image arrowImage)
    {
        // Save the original scale and color
        Vector3 originalScale = arrowImage.transform.localScale;
        Color originalColor = arrowImage.color;

        // Create a new color for the animation
        Color targetColor = new Color(0.5f, 0.2f, 1f, 1f); // Choose the color you prefer

        // Create the animation
        DG.Tweening.Sequence mySequence = DOTween.Sequence();

        // Add a scale up animation
        mySequence.Append(arrowImage.transform.DOScale(originalScale * 1.2f, 0.1f));

        // Add a color change animation
        mySequence.Join(arrowImage.DOColor(targetColor, 0.1f));

        // Add a scale down animation back to the original scale
        mySequence.Append(arrowImage.transform.DOScale(originalScale, 0.1f));

        // Change the color back to the original color
        mySequence.Join(arrowImage.DOColor(originalColor, 0.1f));
    }
}
