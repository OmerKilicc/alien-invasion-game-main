using Euphrates;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Add Percent")]
public class AddPercentUpgradeSO : UpgradeSO
{
    public FloatSO BaseValue;
    public FloatSO Upgraded;

    [Space]
    public float UpgradeMultiplier;

    public override void Set()
    {
        float rval = BaseValue.Value;
        float percent = UpgradeMultiplier;

        for (int i = 0; i < UpgradedAmount; i++)
            rval = rval * percent;

        Upgraded.Value = rval;
    }

    public override void UseUpgrade()
    {
        UpgradedAmount++;

        Set();

        InvokeUpgradeEvent();
    }
}
