using Euphrates;
using System;
using UnityEngine;

public class BasicObjectHealth : MonoBehaviour, IHealth
{
    [SerializeField] IntSO _baseHealthSO;
    [SerializeField] int _health;
    [SerializeField] int _baseHealth;

    public event Action<int> OnHealthChange;

    public int Health
    {
        get => _health;

        set
        {
            int change = value - _health;
            _health = value;
            _health = Mathf.Clamp(_health, 0, _baseHealth);
            OnHealthChange?.Invoke(change);
        }
    }

    public int BaseHealth => _baseHealth;

    void Start()
    {
        _health = _baseHealth;

        if (_baseHealthSO != null)
            Init(_baseHealthSO.Value);
    }

    public void Init(int baseHealth)
    {
        _baseHealth = baseHealth;
        _health = _baseHealth;
    }
}
