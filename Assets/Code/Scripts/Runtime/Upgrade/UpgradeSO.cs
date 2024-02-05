using System;
using UnityEngine;

public abstract class UpgradeSO : ScriptableObject
{
    public string Name;
    public Sprite Image;

    [Space]
    public int UpgradedAmount;
    public int MaxUpgrades;
    public int LevelToUnlock;
    public virtual bool CanUpgrade => UpgradedAmount < MaxUpgrades;
    public event Action<UpgradeSO> OnUpgrade;
    protected void InvokeUpgradeEvent() => OnUpgrade?.Invoke(this);

    [Space]
    public int BaseCost;
    public float CostMultiplier;
    public virtual int Cost
    {
        get
        {
            int rval = BaseCost;

            for (int i = 0; i < UpgradedAmount; i++)
                rval = (int)((float)rval * CostMultiplier);

            return rval;
        }
    }

    public abstract void Set();
    public abstract void UseUpgrade();
}
