using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class WinText : MonoBehaviour
{
    public TextMeshProUGUI winText;

    private FastLaneManager fastLaneManager;

    public Image panel;

    void Start()
    {
        fastLaneManager = GameObject.Find("FastLaneManager").GetComponent<FastLaneManager>();

        panel = GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    public void ShowWinner(int index)
    {
        if (fastLaneManager.EndGame)
        {
            int player = index + 1;
            winText.text = "Player " + player + " wins";

            StartCoroutine(ReturnToMainManu());
        }
    }

    private IEnumerator ReturnToMainManu()
    {
        panel.DOFade(1, 3);
        yield return new WaitForSeconds(3);

        fastLaneManager.FinishGame();
    }
}
