using System;
using UnityEngine;

[RequireComponent(typeof(Targetable), typeof(IExplosive), typeof(IHealth))]
public class ExplosiveBarrel : MonoBehaviour, IDamageable
{
    IExplosive _explosive;
    IHealth _health;
    Targetable _targetable;

    public event Action<int> OnTakeDamage;
    bool _exploded = false;

    [SerializeField] VFXChannelSO _vfxChannel;
    [SerializeField] string _vfxName;

    [Space]
    [SerializeField] SoundChannelSO _soundChannel;
    [SerializeField] string _soundName;

    private void Awake()
    {
        _explosive = GetComponent<IExplosive>();
        _health = GetComponent<IHealth>();

        _targetable = GetComponent<Targetable>();
    }

    private void Start()
    {
        _targetable.AddAsTarget();
    }

    private void OnDisable()
    {
        _targetable.RemoveAsTarget();
    }

    public void TakeDamage(int amount)
    {
        if (_exploded)
            return;

        _health.Health -= amount;

        if (_health.Health > 0)
            return;

        _exploded = true;

        _vfxChannel.PlayVFX(_vfxName, transform.position);
        _soundChannel.Play(_soundName);

        _explosive.Explode();

        gameObject.SetActive(false);
    }
}
