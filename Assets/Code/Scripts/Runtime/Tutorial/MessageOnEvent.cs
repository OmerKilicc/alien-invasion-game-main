using Euphrates;
using UnityEngine;

public class MessageOnEvent : MonoBehaviour
{
    [SerializeField] NotifyChannel _channel;
    [SerializeField] TriggerChannelSO _onTrigger;
    [SerializeField] IntSO _loadedLevel;

    [Space]
    [Header("Message")]
    [SerializeField] string _message;
    [SerializeField] float _duration = 3f;

    [Space]
    [SerializeField] int[] _skippedLevels;

    private void OnEnable() => _onTrigger.AddListener(ShowMessage);

    private void OnDisable() => _onTrigger.RemoveListener(ShowMessage);

    void ShowMessage()
    {
        if (_skippedLevels != null && _skippedLevels.Length != 0)
        {
            for (int i = 0; i < _skippedLevels.Length; i++)
            {
                if (_skippedLevels[i] == _loadedLevel)
                    return;
            }
        }

        _channel.CommanderNotification(_message, _duration);
    }
}
