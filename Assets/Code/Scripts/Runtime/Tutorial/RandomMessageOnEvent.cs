using Euphrates;
using System.Collections.Generic;
using UnityEngine;

public class RandomMessageOnEvent : MonoBehaviour
{
    [SerializeField] NotifyChannel _channel;
    [SerializeField] TriggerChannelSO _onTrigger;
    [SerializeField] IntSO _loadedLevel;
    [SerializeField] IntSO _stage;

    [Space]
    [Header("Message")]
    [SerializeField] List<string> _messages;
    [SerializeField] float _duration = 3f;

    [Space]
    [SerializeField] int[] _skippedStages;
    [SerializeField] int[] _skippedLevels;

    private void OnEnable() => _onTrigger.AddListener(ShowMessage);

    private void OnDisable() => _onTrigger.RemoveListener(ShowMessage);

    void ShowMessage()
    {
        if (_messages == null || _messages.Count == 0)
            return;

        if (_skippedLevels != null && _skippedLevels.Length != 0)
        {
            for (int i = 0; i < _skippedLevels.Length; i++)
            {
                if (_skippedLevels[i] == _loadedLevel)
                    return;
            }
        }

        if (_skippedStages != null && _skippedStages.Length != 0)
        {
            for (int i = 0; i < _skippedStages.Length; i++)
            {
                if (_skippedStages[i] == _stage)
                    return;
            }
        }

        string message = _messages.GetRandomItem();
        _channel.CommanderNotification(message, _duration);
    }
}
