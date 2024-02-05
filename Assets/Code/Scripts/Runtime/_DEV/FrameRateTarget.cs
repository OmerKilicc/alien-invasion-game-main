using UnityEngine;

public class FrameRateTarget : MonoBehaviour
{
    [Tooltip("This will only be set when scene starts, changing this value on runtime won't effect the target.")]
    [SerializeField] int _target;
    private void Awake()
    {
        Application.targetFrameRate = _target;
    }
}
