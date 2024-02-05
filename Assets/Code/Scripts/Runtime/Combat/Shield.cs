using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IDamageable
{
    bool _destructed = false;

    IStageObject _boundStageObject;
    IDestruction _destruction;
    IHealth _health;
    Targetable _targetable;

    int Health
    {
        get => _health.Health;
        set => _health.Health = value;
    }

    public event Action<int> OnTakeDamage;

    private void Awake()
    {
        _boundStageObject = GetComponentInParent<IStageObject>();

        _destruction = GetComponent<IDestruction>();
        _health = GetComponent<IHealth>();

        _targetable = GetComponent<Targetable>();    }

    private void OnEnable()
    {
        _boundStageObject.OnInitialized += Init;
    }

    private void OnDisable()
    {
        _boundStageObject.OnInitialized -= Init;
        _targetable.RemoveAsTarget();
    }

    public void Init(IStageObject _)
    {
        _targetable.AddAsTarget();
    }

    public void TakeDamage(int amount)
    {
        if (_destructed)
            return;

        Health -= amount;

        if (Health > 0)
            return;

        _destructed = true;
        _destruction.Destruct();
        _targetable.RemoveAsTarget();
        gameObject.SetActive(false);
    }
}
