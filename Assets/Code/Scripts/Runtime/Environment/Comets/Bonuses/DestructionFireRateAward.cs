using Euphrates;
using TMPro;
using UnityEngine;

public class DestructionFireRateAward : DestructionBonus
{
    const string NOTIFICATION_STRING = "Fire Rate";
    const float NOTIFICATION_DURATION = 1f;

    [SerializeField] FloatSO _fireRate;
    [SerializeField] FloatSO _bonusPercent;

    [Space]
    [Header("Displaying")]
    [SerializeField] TextMeshPro _text;
    [SerializeField] NotifyChannel _notifyChannel;

    private void Start() => UpdateText();

    protected override void OnDestruction()
    {
        GiveBonus();
        ShowNotification();
    }

    void UpdateText()
    {
        if (_text == null)
            return;

        _text.text = '+' + (_bonusPercent.Value * 100f).ToString("#");
    }

    void GiveBonus()
    {
        float bonusRate = _fireRate * _bonusPercent;
        _fireRate.Value -= bonusRate;
    }

    void ShowNotification() => _notifyChannel.UpgradeNotification(NOTIFICATION_STRING, NOTIFICATION_DURATION);
}
