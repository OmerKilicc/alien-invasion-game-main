using Euphrates;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Add Float")]
public class AddFloatUpgradeSO : UpgradeSO
{
    public FloatSO BaseValue;
    public FloatSO Upgraded;

    [Space]
    public float UpgradeMultiplier;

    public override void Set()
    {
        float rval = BaseValue.Value;

        for (int i = 0; i < UpgradedAmount; i++)
            rval *= UpgradeMultiplier;

        Upgraded.Value = rval;
    }

    public override void UseUpgrade()
    {
        UpgradedAmount++;

        Set();

        InvokeUpgradeEvent();
    }
}
