using Euphrates;
using UnityEngine;

public class PlayerHitHandler : MonoBehaviour
{
    [SerializeField] IntSO _playerHealth;
    [SerializeField] TriggerChannelSO _playerHasBeenHitTrigger;
    IDamageable[] _hitboxes;

    [SerializeField] float _below50Multiplier;

    private void Awake() => _hitboxes = GetComponentsInChildren<IDamageable>();

    private void OnEnable()
    {
        for (int i = 0; i < _hitboxes.Length; i++)
            _hitboxes[i].OnTakeDamage += OnHit;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _hitboxes.Length; i++)
            _hitboxes[i].OnTakeDamage -= OnHit;
    }

    void OnHit(int amount)
    {
        if (_playerHealth.Value <= 50)
            amount = (int)(amount * _below50Multiplier);

        _playerHealth.Value -= amount;
        _playerHasBeenHitTrigger.Invoke();
    }
}
