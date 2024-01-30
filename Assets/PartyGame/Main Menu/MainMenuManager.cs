using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public bool isOnCustomScreen = false;
    private bool isOnMainMenu = true;

    [Header("Cinemachine animator")]
    [SerializeField] private Animator cineMachineAnimator;

    [Header("Screen panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject customizationPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject settingsPanel;


    [Header("Navigation System")]
    [SerializeField] private InputAction navigate;
    [SerializeField] private InputAction click;
    [SerializeField] private InputAction back;
    private bool transitionEnded = true;

    [SerializeField] GameObject playButton;
    [SerializeField] GameObject musicSlider;
    [SerializeField] GameObject creditsBackButton;
    [SerializeField] GameObject customizationDefaultButton; // We need a button on the customization script that doesnt do anything to prevent null errors

    private GameObject selectedItem;
    private bool canNavigate = true;
    private float navigationDelay = 0.2f; // Set delay time as needed

    private PlayerInputManager playerInputManager; // Reference to the input manager to turn on / off the player joining

    [SerializeField] private List<Transform> playerTransformList;

    private void OnEnable()
    {
        cineMachineAnimator.Play("MainMenu");
        click.Enable();
        navigate.Enable();
        back.Enable();
        back.started += GoToMainMenu;
        click.started += OnClick;
        click.performed += ResetDelay;
        navigate.performed += OnNavigate;
    }

    private void OnDisable()
    {
        back.started -= GoToMainMenu;
        click.started -= OnClick;
        click.performed -= ResetDelay;
        navigate.performed -= OnNavigate;
        click.Disable();
        navigate.Disable();
        back.Disable();
    }

    private void Start()
    {
        AudioManager.Instance.StartBackgroundMusic(AudioManager.Instance.menuMusic);
        PlayerManager.Instance.UpdateStartingPoints(playerTransformList);
        cineMachineAnimator.Play("MainMenu");
        menuPanel.SetActive(true);
        customizationPanel.SetActive(false);
        creditsPanel.SetActive(false);
        selectedItem = this.playButton;
        Button startButton = selectedItem.GetComponent<Button>();
        ChangeColorToSelected(startButton.gameObject);
        playerInputManager = PlayerManager.Instance.gameObject.GetComponent<PlayerInputManager>();
        playerInputManager.DisableJoining();
    }

    public void GoToSelectCharacter()
    {
        if (transitionEnded)
        {
            cineMachineAnimator.Play("CharacterSelection");
            ChangeColorToNormal(selectedItem);
            StartCoroutine(ChangeToCustomizationPanelCoroutine());
            isOnCustomScreen = true;
            selectedItem = customizationDefaultButton;
        }
 
    }

    public void GoToSettingsPanel()
    {
        if (transitionEnded)
        {
            cineMachineAnimator.Play("Configuration");
            ChangeColorToNormal(selectedItem);
            StartCoroutine(ChangeBetweenPanelsCoroutine(menuPanel, settingsPanel));
            selectedItem = musicSlider;
            ChangeColorToSelected(selectedItem);
        }
        
    }

    public void GoToCreditsPanel()
    {
        if (transitionEnded)
        {
            cineMachineAnimator.Play("Credits");
            ChangeColorToNormal(selectedItem);
            StartCoroutine(ChangeBetweenPanelsCoroutine(menuPanel, creditsPanel));
            selectedItem = creditsBackButton;
            ChangeColorToSelected(selectedItem);
        }
    }

    public void GoToMainMenu(InputAction.CallbackContext context)
    {
        if (!isOnMainMenu && playerInputManager.playerCount == 0 && transitionEnded)
        { 
            isOnMainMenu = true;
            cineMachineAnimator.Play("MainMenu");
            creditsPanel.SetActive(false);
            settingsPanel.SetActive(false);
            StartCoroutine(ChangeToMainMenuCoroutine());
            selectedItem = playButton;
            ChangeColorToSelected(selectedItem);
        }
    }

    // Same method but this one will be called using UI buttons
    public void GoToMainMenu()
    {
        if (!isOnMainMenu && playerInputManager.playerCount == 0 && transitionEnded)
        {
            isOnMainMenu = true;
            cineMachineAnimator.Play("MainMenu");
            creditsPanel.SetActive(false);
            settingsPanel.SetActive(false);
            StartCoroutine(ChangeToMainMenuCoroutine());
            selectedItem = playButton;
            ChangeColorToSelected(selectedItem);
        }
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Button currentButton = selectedItem.GetComponent<Button>();
        if (currentButton != null) 
        {
            StartCoroutine(ClickButton(currentButton));
        }
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
            //TODO Aqui ocurre la navegacion entre botones
            AudioManager.Instance.PlaySfxPlayer(AudioManager.Instance.moveUI); //MOVE UI
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

    private void OnDestroy()
    {
        click.started -= OnClick;
        navigate.performed -= OnClick;
        click.Disable();
        navigate.Disable();
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
        AudioManager.Instance.PlaySfxPlayer(AudioManager.Instance.select); // SELECT OR NEXT

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

    IEnumerator ChangeBetweenPanelsCoroutine(GameObject panelToDisable, GameObject panelToActivate)
    {
        transitionEnded = false;
        panelToDisable.SetActive(false);
        yield return new WaitForSeconds(2f);
        isOnMainMenu = false;  // Set this as false only when the other screen is loaded.
        panelToActivate.SetActive(true);
        transitionEnded = true;
    }

    IEnumerator ChangeToCustomizationPanelCoroutine()
    {
        transitionEnded = false;
        menuPanel.SetActive(false);
        yield return new WaitForSeconds(2f);
        isOnMainMenu = false;  // Set this as false only when the other screen is loaded.
        customizationPanel.SetActive(true);
        transitionEnded = true;
        playerInputManager.EnableJoining();

    }

    IEnumerator ChangeToMainMenuCoroutine()
    {
        transitionEnded = false;
        yield return new WaitForSeconds(2f);
        menuPanel.SetActive(true);
        transitionEnded = true;
        playerInputManager.DisableJoining();
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

    public void QuitGame()
    {
        Application.Quit();
    }
}
