using Euphrates;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] IntSO _health;
    [SerializeField] TriggerChannelSO _playerDeath;

    bool _isDead = false;

    private void OnEnable() => _health.OnChange += HandleChange;

    private void OnDisable() => _health.OnChange -= HandleChange;

    void HandleChange(int amount)
    {
        if (_isDead || amount > 0 || _health.Value > 0) // Don't proceed if already dead or
            return;                                     // the change is positive or health is above 0.

        _isDead = true;
        _playerDeath.Invoke();
    }
}
