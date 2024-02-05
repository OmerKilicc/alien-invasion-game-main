using System;
using UnityEngine;

public class Hitbox : MonoBehaviour, IDamageable
{

    [SerializeField] float _damageMultiplier = 1f;

    public event Action<int> OnTakeDamage;

    public void TakeDamage(int amount)
    {
        int amt = (int)(amount * _damageMultiplier);
        OnTakeDamage?.Invoke(amt);
    }
}
