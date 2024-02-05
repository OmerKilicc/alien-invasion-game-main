using Euphrates;
using UnityEngine;

public class PlayerMessageHandler : MonoBehaviour
{
    [SerializeField] NotifyChannel _channel;

    [Space]
    [SerializeField] IntSO _level;
    [SerializeField] IntSO _stage;

    [HideInInspector] public PlayerMessageData[] Messages;
}
