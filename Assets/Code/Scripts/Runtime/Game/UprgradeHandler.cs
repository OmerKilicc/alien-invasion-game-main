using Euphrates;
using System.Collections.Generic;
using UnityEngine;

public class UprgradeHandler : MonoBehaviour
{
    [SerializeField] TriggerChannelSO _initialize;
    [SerializeField] List<UprgradeHandler.Upgrade> _upgrades = new List<Upgrade>();

    bool _initialied = false;

    private void OnEnable()
    {
        _initialize.AddListener(Init);

        if (!_initialied)
            return;

        SubEvents();
    }

    private void OnDisable()
    {
        _initialize.RemoveListener(Init);

        if (!_initialied)
            return;

        UnsubEvents();
    }

    void Init()
    {
        if (_initialied)
            return;

        _initialied = true;
        SubEvents();
    }

    void SubEvents()
    {
        foreach (var upgrade in _upgrades)
            upgrade.Event.AddListener(upgrade.UpgradeData.UseUpgrade);
    }

    void UnsubEvents()
    {
        foreach (var upgrade in _upgrades)
            upgrade.Event.RemoveListener(upgrade.UpgradeData.UseUpgrade);
    }

    [System.Serializable]
    struct Upgrade
    {
        public UpgradeSO UpgradeData;
        public TriggerChannelSO Event;
    }
}

