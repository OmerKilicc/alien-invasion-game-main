using Euphrates;
using System;
using UnityEngine;
using Euphrates.Path;

[RequireComponent(typeof(IHealth))]
public class ShipEnemy : MonoBehaviour, IEnemy, IStageObject
{
    bool _enabled = true;

    static string VFX_NAME = "Enemy Ship Explosion";
    readonly string INIT_TIMER_NAME = "Init Timer Enemy [{0}]";
    string TimerName => string.Format(INIT_TIMER_NAME, gameObject.GetInstanceID());

    ITraveller _movement;
    IAttackPattern _attack;

    [Header("Stage")]
    [SerializeField] int _startStage;
    public int StartStage => _startStage;
    [SerializeField] float _stageStartDelay = 0;
    public float Delay => _stageStartDelay;

    [Space]
    [SerializeField] EnemySO _enemyData;
    public EnemySO EnemyData => _enemyData;
    [SerializeField] Path _startPath;
    IHealth _health;

    IDamageable[] _hitboxes;

    [Space]
    [Header("Death")]
    [SerializeField] VFXChannelSO _vfxChannel;
    [SerializeField] EnemyEventSO _defeatEvent;

    [Space]
    [Header("Targeting")]
    [SerializeField] TransformHolderSO _targetHolder;
    [SerializeField] TransformHolderSO _enemyHolder;

    public event Action OnDefeated;
    public event Action<int> OnTookHit;

    public event Action<IStageObject> OnLeaveStage;
    public event Action<IStageObject> OnInitialized;

    private void Awake()
    {
        _movement = GetComponent<ITraveller>();
        _attack = GetComponent<IAttackPattern>();
        _health = GetComponent<IHealth>();

        _hitboxes = GetComponentsInChildren<IDamageable>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < _hitboxes.Length; i++)
            _hitboxes[i].OnTakeDamage += TookHit;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _hitboxes.Length; i++)
            _hitboxes[i].OnTakeDamage -= TookHit;

        _enemyHolder.RemoveTransform(transform);
    }

    private void Start()
    {
        _health.Init(_enemyData.StartHealth);
        transform.localScale = Vector3.zero;
    }

    public void Init()
    {
        if (_stageStartDelay == 0)
        {
            InitInternal();

            return;
        }

        GameTimer.CreateTimer(TimerName, _stageStartDelay, InitInternal);
    }

    void InitInternal()
    {
        transform.localScale = Vector3.one;
        AddTargets();
        _enemyHolder.AddTransform(transform);

        OnInitialized?.Invoke(this);

        if (_startPath == null)
            return;

        _movement.SetPath(_startPath);
    }

    public void StartPath(Path path) => _movement?.SetPath(path);

    void TookHit(int amount)
    {
        _health.Health -= amount;

        OnTookHit?.Invoke(amount);

        if (_health.Health > 0) // Retun if health is above zero.
            return;

        Death(); // If health is 0 kill self.
    }

    void Death()
    {
        _enabled = false;

        _movement.Stop();
        RemoveTargets();
        _enemyHolder.RemoveTransform(transform);

        _defeatEvent.Invoke(this);
        _vfxChannel.PlayVFX(VFX_NAME, transform.position);
        transform.localScale = Vector3.zero;

        OnLeaveStage?.Invoke(this);
        OnDefeated?.Invoke();
    }

    public void Attack(Transform player) => _attack.StartAttack(player);

    void AddTargets()
    {
        foreach (var hb in _hitboxes)
        {
            MonoBehaviour mb = hb as MonoBehaviour;

            if (mb != null)
                _targetHolder.AddTransform(mb.transform);
        }
    }

    void RemoveTargets()
    {
        foreach (var hb in _hitboxes)
        {
            MonoBehaviour mb = hb as MonoBehaviour;

            if (mb != null)
                _targetHolder.RemoveTransform(mb.transform);
        }
    }

    public void Enable() => _enabled = true;

    public void Disable()
    {
        _enabled = false;
        _movement?.Stop();
        _attack?.StopAttack();
    }
}
