using Euphrates;
using UnityEngine;

[RequireComponent(typeof(Upgrader))]
public class BuyUpgrade : MonoBehaviour
{
    Upgrader _upgrader;

    [SerializeField] IntSO _coins;

    public bool CanBuy => _coins.Value >= _upgrader.UpgradeData.Cost;

    private void Awake() => _upgrader = GetComponent<Upgrader>();

    public void Buy()
    {
        if (!CanBuy)
            return;

        int cost = _upgrader.UpgradeData.Cost;

        if (!_upgrader.UseUpgrade())
            return;

        _coins.Value -= cost;
        return;
    }
}
