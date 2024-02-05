using Euphrates;
using UnityEngine;

public class HealthRegen : MonoBehaviour
{
    [SerializeField] TriggerChannelSO _playerHit;

    [Space]
    [SerializeField] IntSO _health;
    [SerializeField] IntSO _maxHealth;

    [Space]
    [SerializeField] FloatSO _regenDelay;
    [SerializeField] IntSO _regenAmount;
    [SerializeField] FloatSO _regenInterval;

    bool _regen = false;

    private void OnEnable()
    {
        _playerHit.AddListener(OnPlayerHit);   
    }

    private void OnDisable()
    {
        _playerHit.RemoveListener(OnPlayerHit);  
    }

    float _lastHitTime = 0;
    float _timePassed = 0;
    private void Update()
    {
        if (!_regen || Time.time < _lastHitTime + _regenDelay)
            return;

        _timePassed += Time.deltaTime;
        if (_timePassed < _regenInterval)
            return;

        _timePassed = 0f;

        int val = _health.Value + _regenAmount.Value > _maxHealth.Value ? _maxHealth.Value : _health.Value + _regenAmount.Value;
        _health.Value = val;

        if (_health.Value <= 0 || _health.Value >= _maxHealth)
            _regen = false;
    }

    void OnPlayerHit()
    {
        _timePassed = 0f;
        _lastHitTime = Time.time;

        _regen = true;
    }
}
