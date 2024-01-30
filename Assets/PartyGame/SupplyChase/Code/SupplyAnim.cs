using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class SupplyAnim : MonoBehaviour
{
    private GameObject parachute;

    private SupplyChaseManager chaseManager;

    private PlayerInput index;

    [SerializeField] public GameObject spawnPos;

    private void Start()
    {
        parachute = transform.Find("Parachute").gameObject;
        chaseManager = GameObject.Find("SupplyChaseManager").GetComponent<SupplyChaseManager>();
    }

    private void OnEnable()
    {
        transform.DOMove(transform.position + Vector3.down * 17, 3).OnComplete(Parachute).SetEase(Ease.OutQuad);

        StartCoroutine(NotTaken());
    }

    private IEnumerator NotTaken()
    {
        yield return new WaitForSeconds(8.0f);

        FuelPooler.SharedInstance.ReturnFuel(gameObject);

        parachute.SetActive(true);
    }

    private void Parachute()
    {
        parachute.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        index = other.GetComponentInChildren<PlayerInput>();

        chaseManager.ScoreUpdate(index.playerIndex);
        FuelPooler.SharedInstance.ReturnFuel(gameObject);
        parachute.SetActive(true);
    }
}
