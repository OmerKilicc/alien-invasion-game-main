using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound Channel", menuName = "SO Channels/Sound")]
public class SoundChannelSO : ScriptableObject
{
    public event Action<string> OnPlay;
    public event Action<string> OnStopSound;
    public event Action<int> OnStopChannel;

    public void Play(string soundName) => OnPlay?.Invoke(soundName);
    public void Stop(string soundName) => OnStopSound?.Invoke(soundName);
    public void Stop(int channel) => OnStopChannel?.Invoke(channel);

}
