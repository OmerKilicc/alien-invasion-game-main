using Euphrates;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CommanderNotification : MonoBehaviour
{
    const string DISPLAY_TIMER = "commander_notification_timer";

    RectTransform _transform;

    [SerializeField] NotifyChannel _channel;

    [Space, Header("Animation")]
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] float _showAnimDuration;
    [SerializeField] float _hideAnimDuration;
    [SerializeField] Ease _easing = Ease.Lerp;

    [Space]
    [SerializeField] int _hiddenY;
    [SerializeField] int _shownY;

    Vector2 HiddenPosition => new Vector2(_transform.anchoredPosition.x, _hiddenY);
    Vector2 ShownPosition => new Vector2(_transform.anchoredPosition.x, _shownY);

    private void Awake()
    {
        _transform = transform as RectTransform;
    }

    private void OnEnable()
    {
        _channel.OnCommanderNotification += CommanderMessage;
        _channel.OnCommanderNotificationConstant += CommanderConstantMessage;
    }

    private void OnDisable()
    {
        _channel.OnCommanderNotification -= CommanderMessage;
        _channel.OnCommanderNotificationConstant -= CommanderConstantMessage;
    }

    void CommanderMessage(string message, float duration)
    {
        ShowNotification(message);
        GameTimer.CancleTimer(DISPLAY_TIMER);

        GameTimer.CreateTimer(DISPLAY_TIMER, duration, HideNotification);
    }

    void CommanderConstantMessage(string message)
    {
        ShowNotification(message);
        GameTimer.CancleTimer(DISPLAY_TIMER);
    }

    bool _isDisplaying = false;
    void ShowNotification(string message)
    {
        if (!_isDisplaying)
        {
            _isDisplaying = true;
            RectTransfromMove(ShownPosition, _showAnimDuration);
        }

        _text.text = message;
    }

    void HideNotification()
    {
        _isDisplaying = false;
        RectTransfromMove(HiddenPosition, _showAnimDuration);
    }

    void RectTransfromMove(Vector2 position, float duration)
    {
        void Step(Vector2 step) => _transform.anchoredPosition = step;
        Tween.DoTween(_transform.anchoredPosition, position, duration, _easing, Step);
    }
}
