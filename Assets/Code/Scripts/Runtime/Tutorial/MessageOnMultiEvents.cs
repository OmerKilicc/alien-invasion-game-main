using Euphrates;
using System.Collections.Generic;
using UnityEngine;

public class MessageOnMultiEvents : MonoBehaviour
{
    [SerializeField] NotifyChannel _channel;
    [SerializeField] List<TriggerChannelSO> _onTrigger = new List<TriggerChannelSO>();

    [Space]
    [SerializeField] List<int> _onLevels = new List<int>();
    [SerializeField] List<int> _onStages = new List<int>();

    [Space]
    [SerializeField] IntSO _stage;
    [SerializeField] IntSO _loadedLevel;

    [Space]
    [Header("Message")]
    [SerializeField] string _message;
    [SerializeField] float _duration = 3f;
    [SerializeField] PlayerMessageType _messageType;

    private void OnEnable()
    {
        foreach(var channel in _onTrigger)
            channel.AddListener(ShowMessage);

    }

    private void OnDisable()
    {
        foreach (var channel in _onTrigger)
            channel.RemoveListener(ShowMessage);
    }

    void ShowMessage()
    {
        if (_onLevels.Count != 0 && !_onLevels.Exists(level => level == _loadedLevel))
            return;

        if (_onStages.Count != 0 && !_onStages.Exists(stage => stage == _stage))
            return;

        switch (_messageType)
        {
            case PlayerMessageType.Commander:
                _channel.CommanderNotification(_message, _duration);
                break;
            case PlayerMessageType.CommanderConstant:
                _channel.CommanderNotificationConstant(_message);
                break;
            default:
                break;
        }
        
    }

    public enum PlayerMessageType { Commander, CommanderConstant}
}
