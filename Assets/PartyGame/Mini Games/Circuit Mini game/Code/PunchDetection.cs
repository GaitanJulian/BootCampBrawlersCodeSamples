using UnityEngine;
using UnityEngine.Events;

public class PunchDetection : MonoBehaviour
{
    public UnityEvent<Transform> punchReceived;

    private void OnTriggerEnter(Collider other)
    {
        punchReceived?.Invoke(other.transform.parent);
    }

}
