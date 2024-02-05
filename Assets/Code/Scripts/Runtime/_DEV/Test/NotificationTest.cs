using UnityEngine;

public class NotificationTest : MonoBehaviour
{
    [SerializeField] NotifyChannel _channel;

    [Space]
    [Header("Constant Notification")]
    [SerializeField] bool _sendConstant = false;
    [SerializeField] string _constantText;

    [Space]
    [Header("Timed Notfication")]
    [SerializeField] bool _sendTimed = false;
    [SerializeField] string _timedText;
    [SerializeField] float _duration;

    void Update()
    {
        if (_sendConstant)
        {
            _sendConstant = false;
            _channel.CommanderNotificationConstant(_constantText);
        }

        if (_sendTimed)
        {
            _sendTimed = false;
            _channel.CommanderNotification(_timedText, _duration);
        }
    }
}
