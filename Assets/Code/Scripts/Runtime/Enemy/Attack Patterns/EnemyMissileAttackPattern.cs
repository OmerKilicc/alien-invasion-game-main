using Euphrates;
using UnityEngine;

[RequireComponent(typeof(IEnemy))]
public class EnemyMissileAttackPattern : MonoBehaviour, IAttackPattern
{
    readonly string TIMER_NAME = "TIMER - ENEMYMISSILE_ATTACKPATTERN[{0}] - {1}";

    bool _enabled = true;

    IEnemy _parent;

    string _shootTimerName;
    string _idleTimerName;

    [Space]
    MissileLauncher[] _launchers;

    bool _shooting = false;

    private void Awake()
    {
        _parent = GetComponent<IEnemy>();
        _launchers = GetComponentsInChildren<MissileLauncher>();

        int instanceId = gameObject.GetInstanceID();

        _shootTimerName = string.Format(TIMER_NAME, instanceId, "SHOOT");
        _idleTimerName = string.Format(TIMER_NAME, instanceId, "IDLE");
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

    void StartShooting()
    {
        _shooting = true;

        GameTimer.CreateTimer(_shootTimerName, _parent.EnemyData.ShootDuration, StartIdleing);
    }

    void StartIdleing()
    {
        _shooting = false;

        GameTimer.CreateTimer(_idleTimerName, _parent.EnemyData.FirePauseDuration, StartShooting);
    }

    public void StartAttack(Transform target)
    {
        if (_launchers == null || _launchers.Length == 0)
            return;

        _enabled = true;

        StartShooting();
    }

    void Shoot()
    {
        for (int i = 0; i < _launchers.Length; i++)
            _launchers[i].Launch();
    }

    float _timePassed = 0f;
    private void Update()
    {
        if (!_enabled || !_shooting)
            return;

        if ((_timePassed += Time.deltaTime) < _parent.EnemyData.FireRate)
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
