using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FuelAnimation : MonoBehaviour
{
    private void Start()
    {
        transform.DORotate(new Vector3(24.424f, -73.016f, -7.197f), 3).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBounce);
        transform.DOMoveY(0.34f, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }

}
