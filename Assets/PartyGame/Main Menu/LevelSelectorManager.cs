
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelSelectorManager : MonoBehaviour
{
    [Header("Navigation System")]
    [SerializeField] private InputAction navigate;
    [SerializeField] private InputAction click;
    private bool transitionEnded = true;

    [SerializeField] GameObject startingMinigame;

    private GameObject selectedItem;
    private bool canNavigate = true;
    private float navigationDelay = 0.2f; // Set delay time as needed

    private void OnEnable()
    {
        click.Enable();
        navigate.Enable();
        click.started += OnClick;
        click.performed += ResetDelay;
        navigate.performed += OnNavigate;
    }

    private void OnDisable()
    {
        click.started -= OnClick;
        click.performed -= ResetDelay;
        navigate.performed -= OnNavigate;
        click.Disable();
        navigate.Disable();
    }

    private void Start()
    {
        selectedItem = this.startingMinigame;
        Button startButton = selectedItem.GetComponent<Button>();
        ChangeColorToSelected(startButton.gameObject);
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (!canNavigate && !transitionEnded)
            return;
        Vector2 inputDirection = context.ReadValue<Vector2>();
        Selectable currentSelectable = selectedItem.GetComponent<Selectable>();
        Selectable nextSelectable = null;

        if (inputDirection.y > 0)
        {
            nextSelectable = currentSelectable.FindSelectableOnUp();
        }
        else if (inputDirection.y < 0)
        {
            nextSelectable = currentSelectable.FindSelectableOnDown();
        }
        else if (inputDirection.x > 0)
        {
            nextSelectable = currentSelectable.FindSelectableOnRight();
        }
        else if (inputDirection.x < 0)
        {
            nextSelectable = currentSelectable.FindSelectableOnLeft();
        }

        if (nextSelectable != null)
        {
            AudioManager.Instance.PlaySfxPlayer(AudioManager.Instance.moveUI); //MOVE UI
            //TODO Aqui ocurre la navegacion entre botones
            ChangeColorToNormal(currentSelectable.gameObject);
            currentSelectable = nextSelectable;
            selectedItem = nextSelectable.gameObject;
            ChangeColorToSelected(selectedItem);

            StartCoroutine(NavigationDelay());
        }

        if (currentSelectable is Slider slider)
        {
            float newValue = slider.value;
            if (inputDirection.x > 0)
                newValue += 1; // adjust the value as needed
            else if (inputDirection.x < 0)
                newValue -= 1; // adjust the value as needed
            slider.value = Mathf.Clamp(newValue, slider.minValue, slider.maxValue);
        }

    }

    private IEnumerator NavigationDelay()
    {
        canNavigate = false;
        yield return new WaitForSeconds(navigationDelay);
        canNavigate = true;
    }

    private void ResetDelay(InputAction.CallbackContext context)
    {
        canNavigate = true;
    }

    private IEnumerator ClickButton(Button button)
    {
        // TODO --> SONIDO DE BOTON MENU PRINCIPAL

        // Change the button's color to the pressed color
        ColorBlock colors = button.colors;
        Color originalColor = colors.normalColor;
        colors.normalColor = colors.pressedColor;
        button.colors = colors;

        // Invoke the onClick event
        button.onClick.Invoke();

        // Wait for a short delay
        yield return new WaitForSeconds(0.1f);

        // Change the button's color back to the original color
        colors.normalColor = Color.white;
        button.colors = colors;
    }

    private void ChangeColorToSelected(GameObject selectedObject)
    {
        Selectable currentSelectable = selectedObject.GetComponent<Selectable>();
        ColorBlock nextColorBlock = currentSelectable.colors;
        nextColorBlock.normalColor = nextColorBlock.selectedColor; // Set to highlighted color
        currentSelectable.colors = nextColorBlock;

        // Scale Up Animation with DOTween
        currentSelectable.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f);
    }

    private void ChangeColorToNormal(GameObject selectedObject)
    {
        Selectable currentSelectable = selectedObject.GetComponent<Selectable>();
        ColorBlock nextColorBlock = currentSelectable.colors;
        nextColorBlock.normalColor = Color.white;
        currentSelectable.colors = nextColorBlock;

        // Scale Down Animation with DOTween
        currentSelectable.transform.DOScale(Vector3.one, 0.1f);
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Button currentButton = selectedItem.GetComponent<Button>();
        if (currentButton != null)
        {
            StartCoroutine(ClickButton(currentButton));
        }
    }
}
