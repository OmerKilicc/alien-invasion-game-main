using Euphrates;
using System;
using UnityEngine;

public class EnemyMissileController : MonoBehaviour, IDamageable
{
    bool _exploded = false;

    Transform _transform;

    IExplosive _explosive;
    IEffect _effects;

    [SerializeField] FloatSO _missileSpeed;
    [SerializeField] FloatSO _missileTurnSpeed;
    [SerializeField] FloatSO _missileLifetime;

    bool _targetAcquired = false;
    Transform _target;

    public event Action<int> OnTakeDamage;

    string TimerName => gameObject.GetInstanceID().ToString() + " - MISSILE EXPLODE TIMER";

    private void Awake()
    {
        _transform = transform;

        _explosive = GetComponent<IExplosive>();
        _effects = GetComponent<IEffect>();
    }

    private void OnDisable() => GameTimer.CancleTimer(TimerName);

    private void Start() => GameTimer.CreateTimer(TimerName, _missileLifetime.Value, Explode);

    private void Update()
    {
        if (!_targetAcquired)
            _targetAcquired = SetTarget();

        Vector3 move = _transform.forward;

        if (_targetAcquired)
        {
            Vector3 toTarget = (_target.position - _transform.position).normalized;
            move = (move + _missileTurnSpeed * toTarget).normalized;

            if (DistSQ(_transform.position, _target.position) < 4)
            {
                Explode();
                return;
            }
        }

        _transform.forward = move;
        _transform.position += _missileSpeed * Time.deltaTime * _transform.forward;
    }

    float DistSQ(Vector3 position1, Vector3 position2) => Vector3.SqrMagnitude(position1 - position2);

    bool SetTarget()
    {
        if (_target != null)
            return true;

        GameObject go = GameObject.FindWithTag("Player");

        if (go == null)
            return false;

        _target = go.transform;
        return true;
    }

    void Explode()
    {
        if (_exploded)
            return;

        _exploded = true;

        _explosive?.Explode();
        _effects?.PlayEffect();

        Destroy(gameObject);
    }

    public void TakeDamage(int _)
    {
        if (_exploded)
            return;

        Explode();
    }
}
