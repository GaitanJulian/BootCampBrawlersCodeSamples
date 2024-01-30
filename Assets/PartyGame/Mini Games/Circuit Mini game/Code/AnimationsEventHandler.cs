
using UnityEngine;
using UnityEngine.Events;

public class AnimationsEventHandler : MonoBehaviour
{
    public UnityEvent onPunchStarted;
    public UnityEvent onPunchFinished;
    public UnityEvent onGetUp;
    public UnityEvent onPunchClipFinished;
    public void OnPunchStarted()
    {
        onPunchStarted?.Invoke();
    }

    public void OnPunchFinished()
    {
        onPunchFinished?.Invoke();
    }

    public void OnGetUp()
    {
        onGetUp?.Invoke();
    }

    public void OnPunchClipFinished()
    {
        onPunchClipFinished?.Invoke();
    }
}
