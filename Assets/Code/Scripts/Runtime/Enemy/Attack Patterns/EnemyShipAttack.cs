using Euphrates;
using UnityEngine;

public class EnemyShipAttack : MonoBehaviour, IAttackPattern
{
    readonly string TIMER_NAME = "TIMER - ENEMYSHIP_ATTACKPATTERN[{0}] - {1}";
    
    bool _enabled = true;

    IEnemy _parent;

    string _shootTimerName;
    string _idleTimerName;

    ShootProjectile[] _blasters;
    IRayProvider _rayProvider;

    [SerializeField] EnemySO _enemyData;
    [Space]
    [SerializeField] LayerMaskSO _playerLayerMask;
    [SerializeField] float _rayDistance = 1500f;

    [Space]

    bool _shooting = false;

    private void Awake()
    {
        _parent = GetComponent<IEnemy>();
    }

    private void OnEnable()
    {
        if (_parent == null)
            return;

        _parent.OnDefeated += StopAttack;
    }

    private void OnDisable()
    {
        StopAttack();

        if (_parent == null)
            return;

        _parent.OnDefeated -= StopAttack;
    }

    private void Start()
    {
        int instanceId = gameObject.GetInstanceID();

        _shootTimerName = string.Format(TIMER_NAME, instanceId, "SHOOT");
        _idleTimerName = string.Format(TIMER_NAME, instanceId, "IDLE");

        _blasters = transform.GetComponentsInChildren<ShootProjectile>();
        _rayProvider = GetComponent<IRayProvider>();
    }

    void StartShooting()
    {
        _shooting = true;

        GameTimer.CreateTimer(_shootTimerName, _enemyData.ShootDuration, StartIdleing);
    }

    void StartIdleing()
    {
        _shooting = false;

        GameTimer.CreateTimer(_idleTimerName, _enemyData.FirePauseDuration, StartShooting);
    }

    public void StartAttack(Transform target)
    {
        if (_blasters == null || _blasters.Length == 0)
            return;

        _enabled = true;

        StartShooting();
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

        int hCnt = Physics.RaycastNonAlloc(ray, _hits, _rayDistance, _playerLayerMask.Value);

        if (hCnt == 0)
            return;

        var hit = _hits[hCnt - 1];

        if (!hit.collider.TryGetComponent<IDamageable>(out var damageable))
            return;

        damageable.TakeDamage(_enemyData.Damage);
    }

    float _timePassed = 0f;
    private void Update()
    {
        if (!_enabled || !_shooting)
            return;

        if ((_timePassed += Time.deltaTime) < _enemyData.FireRate)
            return;

        _timePassed = 0f;

        Shoot();
    }

    public void StopAttack()
    {
        _enabled = false;

        GameTimer.CancleTimer(_shootTimerName);
        GameTimer.CancleTimer(_idleTimerName);
    }
}
