using Euphrates;
using UnityEngine;

public class ShootInput : MonoBehaviour
{
    [SerializeField] InputReaderSO _inputs;
    [SerializeField] TriggerChannelSO _shootStart;
    [SerializeField] TriggerChannelSO _shootEnd;

    private void OnEnable()
    {
        _inputs.OnTouchDown += TouchDown;
        _inputs.OnTouchUp += TouchUp;
    }

    private void OnDisable()
    {
        _inputs.OnTouchDown -= TouchDown;
        _inputs.OnTouchUp -= TouchUp;
    }

    void TouchDown(Vector2 _)
    {
        _shootStart.Invoke();
    }

    void TouchUp(Vector2 _)
    {
        _shootEnd.Invoke();
    }
}
