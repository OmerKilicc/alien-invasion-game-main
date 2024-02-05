using Euphrates;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    bool _exploded = false;

    Transform _transform;

    IExplosive _explosive;
    IEffect _effects;

    [SerializeField] TransformHolderSO _targets;
    [Space]
    [SerializeField] FloatSO _missileSpeed;
    [SerializeField] FloatSO _missileTurnSpeed;
    [SerializeField] FloatSO _targetingRadius;
    [SerializeField] FloatSO _missileLifetime;

    bool _targetAcquired = false;
    Transform _target;

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
        }

        _transform.forward = move;
        _transform.position += _missileSpeed * Time.deltaTime * _transform.forward;
    }

    private void OnTriggerEnter(Collider collider) => Explode();

    float DistSQ(Vector3 position1, Vector3 position2) => Vector3.SqrMagnitude(position1 - position2);

    bool SetTarget()
    {
        float radiusSq = _targetingRadius.Value * _targetingRadius.Value;

        Transform sel = null;
        float minDist = float.MaxValue;
        for(int i = 0; i < _targets.TransformCount; i++)
        {
            Transform target = _targets.GetTransform(i);

            float distSq = DistSQ(target.position, _transform.position);

            if (distSq > radiusSq || distSq > minDist)
                continue;

            sel = target;
            minDist = distSq;
        }

        if (sel == null)
            return false;

        _target = sel;
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

    private void OnDrawGizmos()
    {
        if (_targetingRadius == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _targetingRadius.Value);
    }
}
