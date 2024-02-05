using Euphrates;
using UnityEngine;

public class PlayerHealthAlarm : MonoBehaviour
{
    [SerializeField] IntSO _health;
    [SerializeField] SoundChannelSO _soundChannel;
    [SerializeField] string _playerHealthLowSound;
    [SerializeField] int _lowHealth = 20;

    [Space]
    [Header("Events")]
    [SerializeField] TriggerChannelSO _alarmStart;
    [SerializeField] TriggerChannelSO _alarmEnd;

    bool _wasHealthLow = false;

    private void OnEnable()
    {
        _health.OnChange += OnHealthChange;
        StopAlarm();
    }

    private void OnDisable()
    {
        _health.OnChange -= OnHealthChange;
    }

    void OnHealthChange(int change)
    {
        //Önceki framede caný az ve þimdi caný azlýk sýnýrýnýn üstündeyse
        if (_health.Value > _lowHealth && _wasHealthLow)
        {
            StopAlarm();
            return;
        }

        //Önceki framede can yüksek bu framede düþükse
        if (_health.Value <= _lowHealth && !_wasHealthLow)
        {
            StartAlarm();
            return;
        }
    }

    void StartAlarm()
    {
        _alarmStart?.Invoke();
        _soundChannel.Play(_playerHealthLowSound);
        _wasHealthLow = true;
    }

    void StopAlarm()
    {
        _alarmEnd?.Invoke();
        _soundChannel.Stop(_playerHealthLowSound);
        _wasHealthLow = false;
    }

}
