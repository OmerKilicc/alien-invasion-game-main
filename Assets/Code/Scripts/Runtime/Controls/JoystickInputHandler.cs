using UnityEngine;

[RequireComponent(typeof(Joystick))]
public class JoystickInputHandler : MonoBehaviour
{
    Joystick _joystick;
    [SerializeField] InputsSO _inputs;

    private void Awake()
    {
        _joystick = GetComponent<Joystick>();
    }

    private void Update()
    {
        if (_inputs.Vertical == _joystick.Vertical && _inputs.Horizontal == _joystick.Horizontal)
            return;

        _inputs.Set(_joystick.Horizontal, _joystick.Vertical);
    }
}
