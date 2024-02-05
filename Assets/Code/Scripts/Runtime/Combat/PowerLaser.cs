using Euphrates;
using UnityEngine;

public class PowerLaser : MonoBehaviour
{
    [SerializeField] TriggerChannelSO _enemyHit;
    [SerializeField] TriggerChannelSO _startPowerLaser;
    [SerializeField] TriggerChannelSO _stopPowerLaser;

    [Space]
    [SerializeField] FloatSO _decreaseAmount;
    [SerializeField] FloatSO _increaseAmount;

    [Space]
    [SerializeField] FloatSO _maxPower;
    [SerializeField] FloatSO _power;
    public float Power => _power;

    bool _activated = false;

    [Space]
    [SerializeField] FloatSO _decreaseDelay;

    private void OnEnable() => _enemyHit.AddListener(OnEnemyHit);

    private void OnDisable() => _enemyHit.RemoveListener(OnEnemyHit);

    float _lastHitTime;
    private void Update()
    {
        if (_lastHitTime + _decreaseDelay > Time.time)
            return;

        _power.Value = Mathf.Clamp(_power.Value - _decreaseAmount * Time.deltaTime, 0f, _maxPower);

        if (!_activated || _power > 0)
            return;

        StopShoot();
    }

    void OnEnemyHit()
    {
        if (_activated)
            return;

        _power.Value = Mathf.Clamp(_power.Value + _increaseAmount, 0f, _maxPower);
        _lastHitTime = Time.time;

        if (_power < _maxPower)
            return;

        StartShoot();
    }

    void StartShoot()
    {
        _activated = true;
        _startPowerLaser.Invoke();
    }

    void StopShoot()
    {
        _activated = false;

        _stopPowerLaser.Invoke();
    }
}
