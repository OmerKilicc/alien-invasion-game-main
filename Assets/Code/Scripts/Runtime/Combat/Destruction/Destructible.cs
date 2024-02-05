using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Destructible : MonoBehaviour, IDamageable, IDestruction
{
    bool _destructed = false;

    Collider _collider;
    IHealth _health;
    Targetable _targetable;

    int Health
    {
        get => _health.Health;
        set => _health.Health = value;
    }

    public event Action<int> OnTakeDamage;
    public event Action OnDestructed;

    private void Awake()
    {
        _health =GetComponent<IHealth>();

        _targetable = GetComponent<Targetable>();

        _collider = GetComponent<Collider>();
    }

    private void OnDisable() => _targetable.RemoveAsTarget();

    private void Start() => _targetable.AddAsTarget();

    public void TakeDamage(int amount)
    {
        if (_destructed)
            return;

        Health -= amount;
        OnTakeDamage?.Invoke(amount);

        if (Health > 0)
            return;

        Destruct();
    }

    public void Destruct()
    {
        _destructed = true;
        _targetable.RemoveAsTarget();
        _collider.enabled = false;
        OnDestructed?.Invoke();
    }
}
