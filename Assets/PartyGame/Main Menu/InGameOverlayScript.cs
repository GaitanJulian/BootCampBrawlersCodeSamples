using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InGameOverlayScript : MonoBehaviour
{
    [Header("Tutorial system")]
    [SerializeField] private GameObject currentTutorialPanel;

    [Header("Pause System")]
    [SerializeField] private InputAction startButton;
    [SerializeField] private InputAction navigate;
    [SerializeField] private InputAction click;
    private float navigationDelay = 0.2f;
    private bool canNavigate = true;
    private Button currentButton;
    public UnityEvent startGame;
    public UnityEvent quitGame;
    public GameObject pauseMenuPanel;
    public GameObject resumeButton;
    public GameObject quitButton;

    private bool isStarted = false;
    public bool isPaused = false;

    private void Start()
    {
        currentButton = resumeButton.GetComponent<Button>();
        SetSelectedColor(currentButton);
    }

    private void OnEnable()
    {
        startButton.Enable();
        navigate.Enable();
        click.Enable();
        startButton.started += onStartButtonPressed;
        navigate.performed += OnNavigate;
        click.performed += OnClick;
    }

    private void OnDestroy()
    {
        startButton.started -= onStartButtonPressed;
        navigate.performed -= OnNavigate;
        click.performed -= OnClick;
        startButton.Disable();
        navigate.Disable();
        click.Disable();
    }

    public void onStartButtonPressed(InputAction.CallbackContext context)
    {
        if (!isStarted)
        {
            startGame?.Invoke();
            currentTutorialPanel.SetActive(false);
            isStarted = true;
            return;
        }

        if (!isPaused)
        {
            Pause();
            currentButton = resumeButton.GetComponent<Button>();
            SetSelectedColor(currentButton);
            SetNormalColor(quitButton.GetComponent<Button>());
        }
        else
        {
            Unpause();
        }

    }

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
    }

    public void Unpause()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }


    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        quitGame?.Invoke();
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        if (currentButton != null && isPaused)
        {
            StartCoroutine(ClickButton(currentButton));
        }
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (!isPaused && !canNavigate)
            return;

        Vector2 inputDirection = context.ReadValue<Vector2>();
        Selectable nextSelectable = null;

        if (inputDirection.y > 0)
        {
            nextSelectable = currentButton.FindSelectableOnUp();
        }
        else if (inputDirection.y < 0)
        {
            nextSelectable = currentButton.FindSelectableOnDown();
        }
        else if (inputDirection.x > 0)
        {
            nextSelectable = currentButton.FindSelectableOnRight();
        }
        else if (inputDirection.x < 0)
        {
            nextSelectable = currentButton.FindSelectableOnLeft();
        }

        if (nextSelectable != null)
        {
            // TODO se cambio de boton
            SetNormalColor(currentButton);
            currentButton = nextSelectable.GetComponent<Button>();
            SetSelectedColor(currentButton);
            StartCoroutine(NavigationDelay());
        }


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

    private IEnumerator NavigationDelay()
    {
        canNavigate = false;
        yield return new WaitForSeconds(navigationDelay);
        canNavigate = true;
    }

    private void SetSelectedColor(Button button)
    {
        ColorBlock nextColorBlock = button.colors;
        nextColorBlock.normalColor = nextColorBlock.selectedColor; // Set to highlighted color
        button.colors = nextColorBlock;

        // Scale Up Animation with DOTween
        button.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f);
    }

    private void SetNormalColor(Button button)
    {
        ColorBlock colorBlock = button.colors;
        colorBlock.normalColor = Color.white;
        button.colors = colorBlock;

        // Scale Down Animation with DOTween
        button.transform.DOScale(Vector3.one, 0.1f);
    }
}
