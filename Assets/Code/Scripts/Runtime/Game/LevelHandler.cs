using Euphrates;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    [SerializeField] TriggerChannelSO _initChannel;

    [Space]
    [SerializeField] IntSO _level;
    [SerializeField] IntSO _loadedLevel;
    [SerializeField] int _levelCount;

    [Space]
    [Header("Scene Triggers")]
    [SerializeField] TriggerChannelSO _loadGameScene;
    [SerializeField] TriggerChannelSO _loadMenu;

    [SerializeField] int[] _randomLevels;
    [SerializeField] int[] _bonusLevels;
    [SerializeField] int _bonusLevelOffset = 4;

    Stack<int> _randomLevelStack;
    Stack<int> _bonusLevelStack;

    private void OnEnable() => _initChannel.AddListener(Initialize);

    private void OnDisable() => _initChannel.RemoveListener(Initialize);

    int LoadedLevelIndex
    {
        get
        {
            if (_level.Value > _levelCount - 1)
            {
                int rndIndex = _level.Value - _levelCount;
                return rndIndex % _bonusLevelOffset == 0 ? RandomFromStack(_bonusLevelStack, _bonusLevels) : RandomFromStack(_randomLevelStack, _randomLevels);
            }

            return _level.Value;
        }
    }

    void Initialize() => _loadedLevel.Value = LoadedLevelIndex;

    public void NextLevel()
    {
        _level.Value++;
        _loadedLevel.Value = LoadedLevelIndex;
        _loadMenu.Invoke();
    }

    public void RestartLevel()
    {
        _loadGameScene.Invoke();
    }

    int RandomFromStack(Stack<int> stack, int[] nums)
    {
        if (stack == null)
            stack = new Stack<int>();

        if (stack.Count != 0)
            return stack.Pop();

        List<int> list = nums.ToList();

        for (int i = 0; i < list.Count; i++)
        {
            int item = list.GetRandomItem();
            stack.Push(item);
            list.Remove(item);
        }

        return stack.Pop();
    }
}
