using Euphrates;
using System.Collections.Generic;
using UnityEngine;

public class SaveHandler : MonoBehaviour
{
    [SerializeField] SaveLoadSO _saveChannel;

    [Space]
    [SerializeField] List<TriggerChannelSO> _saveEvents = new List<TriggerChannelSO>();
    [SerializeField] List<TriggerChannelSO> _loadEvents = new List<TriggerChannelSO>();

    [Space]
    [SerializeField] IntSO _currentLevel;
    [SerializeField] IntSO _coins;

    [Space]
    [SerializeField] List<UpgradeSO> _upgrades = new List<UpgradeSO>();


    private void OnEnable()
    {
        foreach (var ev in _saveEvents)
            ev.AddListener(SaveGame);

        foreach (var ev in _loadEvents)
            ev.AddListener(LoadGame);
    }

    private void OnDisable()
    {
        foreach (var ev in _saveEvents)
            ev.RemoveListener(SaveGame);

        foreach (var ev in _loadEvents)
            ev.RemoveListener(LoadGame);
    }

    void SaveGame()
    {
        List<SavedUpgrade> upgradesSave = new List<SavedUpgrade>();
        foreach (var u in _upgrades)
        {
            upgradesSave.Add(new SavedUpgrade()
            {
                Name = u.Name,
                Level = u.UpgradedAmount
            });
        }

        SaveData data = new SaveData()
        {
            Level = _currentLevel,
            Coins = _coins,
            Upgrades = upgradesSave
        };

        _saveChannel.Save(data);
    }

    void LoadGame()
    {
        SaveData data = _saveChannel.Load();

        _currentLevel.Value = data.Level;
        _coins.Value = data.Coins;

        foreach (var u in _upgrades)
        {
            bool set = false;

            foreach (var su in data.Upgrades)
            {
                if (su.Name != u.Name)
                    continue;

                u.UpgradedAmount = su.Level;
                u.Set();
                set = true;

                break;
            }

            if (set)
                continue;

            u.UpgradedAmount = 0;
            u.Set();
        }
    }
}