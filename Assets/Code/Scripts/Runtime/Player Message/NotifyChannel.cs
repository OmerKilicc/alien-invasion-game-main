using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SO Channels/Notify")]
public class NotifyChannel : ScriptableObject
{
    public event Action<string, float> OnCommanderNotification;
    public void CommanderNotification(string message, float duration) => OnCommanderNotification?.Invoke(message, duration);

    public event Action<string> OnCommanderNotificationConstant;
    public void CommanderNotificationConstant(string message) => OnCommanderNotificationConstant?.Invoke(message);

    public event Action OnHideCommanderNotification;
    public void HideCommanderNotification() => OnHideCommanderNotification?.Invoke();

    public event Action<string, float> OnUpgradeNotification;
    public void UpgradeNotification(string message, float duration) => OnUpgradeNotification?.Invoke(message, duration);
}
