using Euphrates;
using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Comet : MonoBehaviour, IDamageable, IDestruction
{
    bool _enabled = false;
    Collider _collider;

    [SerializeField] IntSO _cometHitAmount;

    public event Action OnDestructed;
    public event Action<int> OnTakeDamage;

    int _hitAmount = 0;

    private void Awake() => _collider = GetComponent<Collider>();

    public void Init()
    {
        _enabled = true;
        _hitAmount = 0;
    }

    public void Destruct()
    {
        _enabled = false;
        _collider.enabled = false;
        OnDestructed?.Invoke();
    }

    public void TakeDamage(int amount)
    {
        if (!_enabled || ++_hitAmount < _cometHitAmount)
            return;

        Destruct();
    }
}
