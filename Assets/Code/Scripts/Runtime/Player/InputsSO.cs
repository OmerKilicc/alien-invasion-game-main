using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "SO Channels/Inputs")]
public class InputsSO : ScriptableObject
{

    public event UnityAction OnChange;

    float _horizontal;
    public float Horizontal
    {
        get => _horizontal;

        set
        {
            _horizontal = value;
            OnChange?.Invoke();
        }
    }

    float _vertical;
    public float Vertical
    {
        get => _vertical;

        set
        {
            _vertical = value;
            OnChange?.Invoke();
        }
    }

    public void Set(float horizontal, float vertical)
    {
        _horizontal = horizontal;
        _vertical = vertical;

        OnChange?.Invoke();
    }
}
