using System;
using UnityEngine;

public class Minion : MonoBehaviour, IDamageable
{
    public int Damage = 1;

    Transform _transform;

    protected virtual void Awake() => _transform= transform;

    public event Action<Minion> OnShotDown;
    public event Action<int> OnTakeDamage;

    public virtual void TakeDamage(int _)
    {
        OnShotDown?.Invoke(this);
    }

    public void SetPositionRotation(Vector3 position, Quaternion rotation) => _transform.SetPositionAndRotation(position, rotation);
}
