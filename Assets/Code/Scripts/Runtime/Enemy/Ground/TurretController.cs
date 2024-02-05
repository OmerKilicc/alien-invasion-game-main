using Euphrates;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    bool _enabled = true;

    Transform _transform;
    Transform _target;

    [SerializeField] string _playerTag = "Player";

    [Space]
    [SerializeField] Transform _hinge;
    [SerializeField] Transform _rotor;

    [Space]
    [SerializeField] float _hingeSpeed = 5f;
    [SerializeField] float _rotorSpeed = 10f;

    [Space]
    [SerializeField] LayerMaskSO _playerLayerMask;
    [SerializeField] float _range = 50f;
    [SerializeField] float _fireRate = .5f;
    [SerializeField] float _shootTreshold = .9f;
    [SerializeField] int _damage = 5;

    //[Space]
    //[SerializeField] Animator _animator;
    //[SerializeField] string _activationTrigger;
    //[SerializeField] string _deactivationTrigger;

    // TurretState _currentState = TurretState.Deactive;

    IRayProvider _rayProvider;
    ShootProjectile[] _blasters;

    IDestruction _destruction;

    float DistanceToTarget
    {
        get
        {
            if (_target == null)
                return float.MaxValue;

            return Vector3.Distance(_transform.position, _target.position);
        }
    }

    private void Awake()
    {
        _transform = transform;

        _rayProvider = GetComponentInChildren<IRayProvider>();
        _blasters = GetComponentsInChildren<ShootProjectile>();

        _destruction = GetComponent<IDestruction>();
    }

    private void OnEnable()
    {
        _destruction.OnDestructed += Disable;
    }

    private void OnDisable()
    {
        _destruction.OnDestructed -= Disable;
    }

    private void Start() => SetTarget();

    float _timePassed = 0f;
    private void Update()
    {
        if (!_enabled)
            return;

        SetTarget();

        if (!_target || !InRange())
            return;

        // _currentState = State();

        MoveRotor();
        MoveHinge();

        if (!CanShoot())
            return;

        _timePassed += Time.deltaTime;

        if (_timePassed < _fireRate)
            return;

        _timePassed = 0f;

        Shoot();
    }

    void SetTarget()
    {
        if (_target)
            return;

        GameObject go = GameObject.FindWithTag(_playerTag);

        if (!go)
            return;

        _target = go.transform;
    }

    //TurretState State()
    //{
    //    float _dist = Vector3.Distance(_transform.position, _target.position);

    //    if (_dist > _range)
    //    {
    //        if (_currentState == TurretState.Active)
    //        {

    //        }
    //    }

    //    return true;
    //}

    void MoveRotor()
    {
        Vector3 targetedRotor = _rotor.InverseTransformPoint(_target.position);
        targetedRotor.y = 0;

        targetedRotor = _rotor.TransformPoint(targetedRotor);

        Quaternion targetRotation = Quaternion.LookRotation(targetedRotor - _rotor.position, _rotor.up);

        _rotor.rotation = Quaternion.Lerp(_rotor.rotation, targetRotation, _rotorSpeed * Time.deltaTime);
    }

    void MoveHinge()
    {
        Vector3 targetedHinge = _hinge.InverseTransformPoint(_target.position);

        Vector3 groundTargeted = targetedHinge;
        groundTargeted.y = 0;

        float zDist = groundTargeted.magnitude;
        float y = targetedHinge.y;

        targetedHinge = new Vector3(0, y, zDist);
        targetedHinge = _hinge.TransformPoint(targetedHinge);

        Quaternion targetRotation = Quaternion.LookRotation(targetedHinge - _hinge.position, _hinge.up);
        _hinge.rotation = Quaternion.Lerp(_hinge.rotation, targetRotation, _hingeSpeed * Time.deltaTime);
    }

    bool InRange() => _range > DistanceToTarget;

    bool CanShoot()
    {
        Vector3 toTarget = (_target.position - _hinge.position).normalized;
        float dot = Vector3.Dot(toTarget, _hinge.forward);

        return dot >= _shootTreshold;
    }

    void SpawnProjectiles()
    {
        for (int i = 0; i < _blasters.Length; i++)
            _blasters[i].SpawnProjectile();
    }

    RaycastHit[] _hits = new RaycastHit[5];
    void Shoot()
    {
        SpawnProjectiles();
        Ray ray = _rayProvider.CreateRay();

        int hCnt = Physics.RaycastNonAlloc(ray, _hits, 200f, _playerLayerMask.Value);

        if (hCnt == 0)
            return;

        var hit = _hits[hCnt - 1];

        if (!hit.collider.TryGetComponent<IDamageable>(out var damageable))
            return;

        damageable.TakeDamage(_damage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _range);
    }

    public void Enable() => _enabled = true;

    public void Disable() => _enabled = false;

    // enum TurretState { Active, Deactive, Deactivating, Activating}
}
