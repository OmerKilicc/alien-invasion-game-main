using Euphrates;
using UnityEngine;

public class DragInputs : MonoBehaviour
{
    [SerializeField] InputReaderSO _inputReader;
    [SerializeField] InputsSO _inputs;

    [Space]
    [Header("Controller Stats")]
    [SerializeField] FloatSO _deadZone;
    [SerializeField] FloatSO _maxDistance;


    private void OnEnable()
    {
        _inputReader.OnTouchDown += OnTouchDown;
        _inputReader.OnTouchMove += OnTouchMove;
        _inputReader.OnTouchUp += OnTouchUp;
    }

    private void OnDisable()
    {
        _inputReader.OnTouchDown -= OnTouchDown;
        _inputReader.OnTouchMove -= OnTouchMove;
        _inputReader.OnTouchUp -= OnTouchUp;
    }

    Vector2 _initialTouch;
    void OnTouchDown(Vector2 pos) => _initialTouch = pos;

    void OnTouchMove(Vector2 pos)
    {
        Vector2 dif = pos - _initialTouch;
        _initialTouch = pos;

        _inputs.Set(dif.x * .25f, dif.y * .25f);
    }

    void OnTouchUp(Vector2 _) => _inputs.Set(0, 0);
}
