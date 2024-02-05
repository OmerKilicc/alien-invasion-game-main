using UnityEngine;

public class ExplosiveMinion : Minion, IPoolable
{
    IExplosive _explosive;

    [SerializeField] VFXChannelSO _vfxChannel;
    [SerializeField] string _particleName;

    bool _exploded = false;

    protected override void Awake()
    {
        base.Awake();
        _explosive= GetComponent<IExplosive>();
    }

    public override void TakeDamage(int _)
    {
        if (_exploded)
            return;

        _exploded = true;

        _explosive.Explode();
        _vfxChannel.PlayVFX(_particleName, transform.position);

        base.TakeDamage(_);
    }

    public void OnGet()
    {
        _exploded = false;
    }

    public void OnReleased()
    {
    }

    public void OnDestroyed()
    {
    }
}
