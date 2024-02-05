using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "SO Channels/Input Reader")]
public class InputReaderSO : ScriptableObject, InputControls.IGameInputsActions
{
    public event Action<Vector2> OnTouchDown;
    public event Action<Vector2> OnTouchMove;
    public event Action<Vector2> OnTouchUp;

    InputControls _controls = null;

    private void OnEnable()
    {
        if (_controls != null)
            return;

        _controls = new InputControls();

        _controls.GameInputs.SetCallbacks(this);

        _controls.GameInputs.Enable();
    }

    public void OnTouchInput(InputAction.CallbackContext context)
    {
        TouchState state = context.ReadValue<TouchState>();

        switch (state.phase)
        {
            case UnityEngine.InputSystem.TouchPhase.Began:
                OnTouchDown?.Invoke(state.position);
                return;

            case UnityEngine.InputSystem.TouchPhase.Moved:
                OnTouchMove?.Invoke(state.position);
                return;

            case UnityEngine.InputSystem.TouchPhase.Ended:
            case UnityEngine.InputSystem.TouchPhase.Canceled:
                OnTouchUp?.Invoke(state.position);
                return;

            case UnityEngine.InputSystem.TouchPhase.Stationary:
            case UnityEngine.InputSystem.TouchPhase.None:
            default:
                return;
        }
    }
}
