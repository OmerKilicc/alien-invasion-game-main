using Euphrates;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Add Int")]
public class AddIntUpgradeSO : UpgradeSO
{
    public IntSO BaseValue;
    public IntSO Upgraded;

    [Space]
    public int AddedAmount;

    public override void Set()
    {
        int rval = BaseValue.Value;

        for (int i = 0; i < UpgradedAmount; i++)
            rval += AddedAmount;

        Upgraded.Value = rval;
    }

    public override void UseUpgrade()
    {
        UpgradedAmount++;

        Set();

        InvokeUpgradeEvent();
    }
}
