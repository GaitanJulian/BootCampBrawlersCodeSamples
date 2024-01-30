using System.Collections;
using UnityEngine;

public class StartMainMenuManager : MonoBehaviour
{
    [SerializeField] LevelSelectorManager levelSelector;
    [SerializeField] MainMenuManager mainMenuManager;
    void Start()
    {
        StartCoroutine(startManager());
    }

    IEnumerator startManager()
    {
        yield return new WaitForSeconds(0.5f);

        if (levelSelector != null)
            levelSelector.enabled = true;
        else
            mainMenuManager.enabled = true;
    }
}
