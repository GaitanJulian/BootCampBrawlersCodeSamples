using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class WinnerText : MonoBehaviour
{
    public TextMeshProUGUI winText;

    private SupplyChaseManager supplyChaseManager;

    public Image panel;

    void Start()
    {
        supplyChaseManager = GameObject.Find("SupplyChaseManager").GetComponent<SupplyChaseManager>();

        panel = GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    public void ShowWinner(int index)
    {
        if (supplyChaseManager.endGame)
        {
            int player = index + 1;
            winText.text = "Player " + player + " wins";

            StartCoroutine(ReturnToMainManu());
        }
    }

    private IEnumerator ReturnToMainManu()
    {
        panel.DOFade(1, 6);
        yield return new WaitForSeconds(6);

        supplyChaseManager.FinishGame();
    }
}
