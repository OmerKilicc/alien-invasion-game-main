using Euphrates;
using UnityEngine;
using UnityEngine.Events;

public class EventTriggerCC : MonoBehaviour, IConsoleCommand
{
    public string CommandString => "trigger";

    [SerializeField] string[] _parameters;
    [SerializeField] UnityEvent _event;

    public void Execute(DebugConsole caller, params string[] args)
    {
        if (_parameters.Length != args.Length)
            return;

        for (int i = 0; i < _parameters.Length; i++)
        {
            string prm = _parameters[i].ToLower();
            string arg = args[i].ToLower();

            if (prm != arg)
                return;
        }

        _event.Invoke();
    }
}
