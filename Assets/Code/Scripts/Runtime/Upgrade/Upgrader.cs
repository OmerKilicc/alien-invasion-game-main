using Euphrates;
using System;
using UnityEngine;

public class Upgrader : MonoBehaviour
{
    [SerializeField] UpgradeSO _upgrade;
    [SerializeField] IntSO _currentLevel;
    public UpgradeSO UpgradeData => _upgrade;
    public event Action OnUse;
    public TriggerChannelSO OnUpgarde;

    public bool Unlocked => _currentLevel >= _upgrade.LevelToUnlock;

    public bool UseUpgrade()
    {
        if (!Unlocked)
            return false;

        if (!_upgrade.CanUpgrade)
            return false;

        _upgrade.UseUpgrade();

        OnUse?.Invoke();
        OnUpgarde?.Invoke();

        return true;
    }
}
