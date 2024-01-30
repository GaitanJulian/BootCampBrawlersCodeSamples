using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// This script manages the UI interactions on the customization canvas including navigation, activation, and handling player inputs
public class CustomizationCanvas : MonoBehaviour
{
    // The World Canvas
    [SerializeField] private GameObject customizationCanvas;

    // References to the different game objects and buttons on the canvas
    [SerializeField] private Button startButton;
    [SerializeField] private Button readyButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button firstButtonCustomization;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject joinCanvas;

    private Button currentButton;

    // Tell if the canvas is active, prevent a player from spawning on an active canvas
    [SerializeField] private bool isActive = false;

    // Tell if the player is on customization panel
    [SerializeField] private bool isOnCustomization = false;

    // A reference to the main menu manager to tell if the players are on Character Selection
    private MainMenuManager mainMenuManager;

    // Tell if the player is on ready status 
    public bool isReady = false;

    // Reference to the input actions
    private InputActionAsset inputActions;
    private InputActionMap UI;

    // Prevent the player from moving in the UI crazy when using a controller
    [SerializeField] private bool canNavigate = true;
    private float navigationDelay = 0.25f; // Set delay time as needed

    private void OnEnable()
    {
        PlayerManager.Instance.OnPlayerAdded += HandlePlayerAdded;
        currentButton = startButton;
        mainMenuManager = FindObjectOfType<MainMenuManager>();
        isReady = false;
    }

    public void TurnOffCanvas()
    {
        joinCanvas.SetActive(true);
        isReady = false;
        isActive = false;
        customizationCanvas.SetActive(false);
        UI.FindAction("Activate").started -= OnActivate;
        UI.FindAction("Activate").performed -= ResetDelay;
        UI.FindAction("Navigate").performed -= OnNavigate;
        UI.FindAction("Cancel").started -= OnPlayerQuit;
        UI.Disable();
        currentButton = startButton;
    }

    private void OnDisable()
    {
        isReady = false;
        isActive = false;
        if (UI != null)
        {
            UI.FindAction("Activate").started -= OnActivate;
            UI.FindAction("Activate").performed -= ResetDelay;
            UI.FindAction("Navigate").performed -= OnNavigate;
            UI.FindAction("Cancel").started -= OnPlayerQuit;
            UI.Disable();
        }
    }

    private void OnDestroy()
    {
        PlayerManager.Instance.OnPlayerAdded -= HandlePlayerAdded;
    }

    private void HandlePlayerAdded(PlayerInput player)
    {
        if (!isActive && GetComponentInChildren<CharacterCustomizationScript>() != null) 
        {
            joinCanvas.SetActive(false);
            customizationCanvas.SetActive(true);
            inputActions = player.actions;
            UI = inputActions.FindActionMap("UI");
            UI.Enable();
            UI.FindAction("Navigate").performed += OnNavigate;
            UI.FindAction("Activate").started += OnActivate;
            UI.FindAction("Activate").performed += ResetDelay;
            UI.FindAction("Cancel").started += OnPlayerQuit;
            UI.Enable();
            isActive = true;

            ColorBlock currColorBlock = currentButton.colors;
            currColorBlock.normalColor = currColorBlock.selectedColor;
            currentButton.colors = currColorBlock;

        }
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (!canNavigate || isReady || !mainMenuManager.isOnCustomScreen)
            return;


            Vector2 inputDirection = context.ReadValue<Vector2>();
            Selectable nextSelectable = null;

            ScrollableSelector optionsSelector = currentButton.gameObject.GetComponent<ScrollableSelector>();

            if (optionsSelector != null)
            {
                if (inputDirection.x > 0)
                {
                    optionsSelector.ScrollRight();
                    StartCoroutine(NavigationDelay());
                    return;
                //TODO aqui se navega a derecha en el scroll
                }
                else if (inputDirection.x < 0)
                {
                    optionsSelector.ScrollLeft();
                    StartCoroutine(NavigationDelay());
                    return;
                    //TODO aqui se navega a izquierda en el scroll
                }
            }


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

    private void OnActivate(InputAction.CallbackContext context)
    {
        if (currentButton != null && mainMenuManager.isOnCustomScreen)
        {
            //TODO Sonido de click
            currentButton.onClick.Invoke();
        }
    }

    public void OnPlayerQuit(InputAction.CallbackContext context)
    {
        ColorBlock colorBlock;
        if (isOnCustomization)
        {
            colorBlock = currentButton.colors;
            colorBlock.normalColor = Color.white;
            currentButton.colors = colorBlock;
            currentButton = backButton;
            SetSelectedColor(currentButton);
            return;
        }


        // Prevent the player from removing the player when ready
        if (isReady)
        {
            readyButton.onClick.Invoke();
            return;
        }

        colorBlock = currentButton.colors;
        colorBlock.normalColor = Color.white;
        currentButton.colors = colorBlock;

        // Set the current button as the quit button
        currentButton = quitButton;

        ColorBlock currColorBlock = currentButton.colors;
        currColorBlock.normalColor = currColorBlock.selectedColor;
        currentButton.colors = currColorBlock;

    }

    public void ChangePlayerStatus()
    {
        isReady = !isReady;
    }

    private void ResetDelay(InputAction.CallbackContext context)
    {
        canNavigate = true;
    }

    private IEnumerator NavigationDelay()
    {
        canNavigate = false;
        yield return new WaitForSeconds(navigationDelay);
        canNavigate = true;
    }

    public void OnCustomizationPanel()
    {
        SetNormalColor(currentButton);
        currentButton = firstButtonCustomization;
        SetSelectedColor(currentButton);
        isOnCustomization = true;
    }

    public void OnReadyPanel()
    {
        SetNormalColor(currentButton);
        currentButton = readyButton;
        SetSelectedColor(currentButton);
        isOnCustomization = false;
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
