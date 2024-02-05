using Euphrates;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    GameSound EMPTY_SOUND = new GameSound();

    [SerializeField] SoundChannelSO _soundChannel;

    [Space]
    [HideInInspector] public AudioSource[] AudioSources;

    [Space]
    [SerializeField] GameSound[] _sounds;

    private void Awake()
    {
        AudioSources = GetComponentsInChildren<AudioSource>();
    }

    private void OnEnable()
    {
        if (_soundChannel != null)
        {
            _soundChannel.OnPlay += PlaySound;
            _soundChannel.OnStopSound += StopSound;
            _soundChannel.OnStopChannel += StopSound;
        }

        if (_sounds == null || _sounds.Length == 0)
            return;

        for (int i = 0; i < _sounds.Length; i++)
        {
            _sounds[i].Manager = this;

            if (_sounds[i].Triggers == null || _sounds[i].Triggers.Length == 0)
                continue;

            for (int j = 0; j < _sounds[i].Triggers.Length; j++)
                _sounds[i].Triggers[j].AddListener(_sounds[i].Play);

            if (_sounds[i].DisableTriggers == null || _sounds[i].DisableTriggers.Length == 0)
                continue;

            for (int j = 0; j < _sounds[i].DisableTriggers.Length; j++)
                _sounds[i].DisableTriggers[j].AddListener(_sounds[i].Stop);
        }
    }

    private void OnDisable()
    {
        if (_soundChannel != null)
        {
            _soundChannel.OnPlay -= PlaySound;
            _soundChannel.OnStopSound -= StopSound;
            _soundChannel.OnStopChannel -= StopSound;
        }

        if (_sounds == null || _sounds.Length == 0)
            return;

        for (int i = 0; i < _sounds.Length; i++)
        {
            _sounds[i].Manager = this;

            if (_sounds[i].Triggers == null || _sounds[i].Triggers.Length == 0)
                continue;

            for (int j = 0; j < _sounds[i].Triggers.Length; j++)
                _sounds[i].Triggers[j].RemoveListener(_sounds[i].Play);

            if (_sounds[i].DisableTriggers == null || _sounds[i].DisableTriggers.Length == 0)
                continue;

            for (int j = 0; j < _sounds[i].DisableTriggers.Length; j++)
                _sounds[i].DisableTriggers[j].RemoveListener(_sounds[i].Stop);
        }
    }

    bool TryGetSound(out GameSound sound, string soundName)
    {
        sound = EMPTY_SOUND;

        if (_sounds == null || _sounds.Length == 0)
            return false;

        for (int i = 0; i < _sounds.Length; i++)
        {
            if (_sounds[i].Name != soundName)
                continue;

            sound = _sounds[i];
            return true;
        }

        return false;
    }

    bool TryGetSource(out AudioSource channel, int sourceIndex)
    {
        channel = null;

        if (AudioSources == null || AudioSources.Length == 0 || sourceIndex > AudioSources.Length - 1)
            return false;

        for (int i = 0; i < _sounds.Length; i++)
        {
            if (i != sourceIndex)
                continue;

            channel = AudioSources[i];
            return true;
        }

        return false;
    }

    public void PlaySound(string soundName)
    {
        if (!TryGetSound(out var sound, soundName) || !TryGetSource(out var source, sound.Source))
            return;

        source.clip = sound.Audio;
        source.loop = sound.Looping;

        source.volume = sound.Volume;
        source.pitch = sound.Pitch;

        source.Play();
    }

    public void StopSound(int channel)
    {
        if (!TryGetSource(out var source, channel))
            return;

        source.Stop();
    }

    public void StopSound(string soundName)
    {
        if (!TryGetSound(out var sound, soundName) || !TryGetSource(out var source, sound.Source))
            return;

        source.Stop();
    }
}

[System.Serializable]
public struct GameSound
{
    public string Name;
    public AudioClip Audio;
    public int Source;
    public bool Looping;
    [Space]
    public float Volume;
    public float Pitch;
    [Space]
    [Tooltip("Can leave this part empty if you are going to use sound channel.")] public TriggerChannelSO[] Triggers;
    [Tooltip("Can leave this part empty if you are going to use sound channel.")] public TriggerChannelSO[] DisableTriggers;

    [HideInInspector] public SoundManager Manager;

    public void Play() => Manager.PlaySound(Name);
    public void Stop() => Manager.StopSound(Name);
}
