using Euphrates;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BuyUpgrade))]
public class UpgradeButtonEnabler : MonoBehaviour
{
    Upgrader _upgrader;
    BuyUpgrade _buyUpgrade;

    [SerializeField] Button _button;
    [SerializeField] IntSO _currentLevel;

    private void Awake()
    {
        _upgrader = GetComponent<Upgrader>();
        _buyUpgrade = GetComponent<BuyUpgrade>();
    }

    public void Check() => _button.interactable = _buyUpgrade.CanBuy && _currentLevel >= _upgrader.UpgradeData.LevelToUnlock;
}
