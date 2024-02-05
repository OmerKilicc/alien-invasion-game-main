using Euphrates;
using UnityEngine;

public class LoadLevelCC : MonoBehaviour, IConsoleCommand
{
    [SerializeField] IntSO _level;

    [SerializeField] TriggerChannelSO _loadLevel;
    [SerializeField] TriggerChannelSO _loadMenu;

    public string CommandString => "load";

    public void Execute(DebugConsole caller, params string[] args)
    {
        if (args.Length == 0)
            return;

        string levelName = args[0];

        if (levelName.ToLower() == "menu")
        {
            _loadMenu.Invoke();
            return;
        }

        if (!int.TryParse(levelName, out int levelIndex)
            || levelIndex < 0)
            return;

        _level.Value = levelIndex;
        _loadLevel.Invoke();
    }
}
