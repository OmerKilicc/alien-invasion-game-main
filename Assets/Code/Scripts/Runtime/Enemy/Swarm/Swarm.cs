
using Euphrates;
using Euphrates.Path;
using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class Swarm : MonoBehaviour, IEnemy, IStageObject
{
    bool _enabled = false;
    bool _atacking = false;
    ITraveller _movement;

    [SerializeField] EnemySO _enemyData;
    public EnemySO EnemyData => _enemyData;

    [Space]
    [SerializeField] EnemyEventSO _deathEvent;
    [SerializeField] Path _startPath;

    [Space]
    [Header("Minion")]
    [SerializeField] string _minionPoolName;
    [SerializeField] float _minionHitDistance = 2f;
    [SerializeField] string _explosiveMinionPoolName;
    [SerializeField] int _explosiveMinionCount;

    List<Minion> _minions;

    [Space]
    [Header("VFX")]
    [SerializeField] VFXChannelSO _vfxChannel;
    [SerializeField] string _minionDeathVFX;

    [Space]
    [Header("Boid Settings")]
    [SerializeField] int _swarmSize = 20;
    [SerializeField] float _boidSpeed = 10f;
    [SerializeField] float _boidTurnSpeed = 2f;
    [SerializeField] float _boidAvoidDistance = 2f;
    [SerializeField] float _boidTargetBias = 5;

    [Space]
    [Header("Stage")]
    [SerializeField] int _startStage;
    public int StartStage => _startStage;
    [SerializeField] int _stageStartDelay;
    public float Delay => _stageStartDelay;

    [Space]
    [Header("Targeting")]
    [SerializeField] TransformHolderSO _targetHolder;
    [SerializeField] TransformHolderSO _enemyHolder;

    public event Action OnDefeated;
    public event Action<IStageObject> OnLeaveStage;
    public event Action<IStageObject> OnInitialized;

    IDamageable _playerHitbox;

    private void Awake()
    {
        _movement = GetComponent<ITraveller>();
    }

    private void OnDisable()
    {
        _enemyHolder.RemoveTransform(transform);
    }

    private void Update()
    {
        if (!_enabled)
            return;

        int cnt = _minions.Count;

        NativeArray<float3> minionPositions = new NativeArray<float3>(cnt, Allocator.TempJob);
        NativeArray<float3> minionDirections = new NativeArray<float3>(cnt, Allocator.TempJob);

        PopulateArrays(minionPositions, minionDirections);

        int processorCount = System.Environment.ProcessorCount;

        var dirJob = new SwarmDirJob()
        {
            Positions = minionPositions,
            Directions = minionDirections,

            Target = transform.position,
            AvoidDistanceSQ = _boidAvoidDistance* _boidAvoidDistance,
            TargetBias = _boidTargetBias,

            TurnSpeed = _boidTurnSpeed,

            DeltaTime = Time.deltaTime
        };

        dirJob.Schedule(cnt, processorCount).Complete();

        var moveJob = new SwarmMoveJob()
        {
            Positions = minionPositions,
            Directions = minionDirections,

            Speed = _boidSpeed,
            DeltaTime = Time.deltaTime
        };

        moveJob.Schedule(cnt, processorCount).Complete();

        UpdateSwarm(minionPositions, minionDirections);

        if (_atacking)
        {
            NativeArray<bool> minionhits = new NativeArray<bool>(cnt, Allocator.TempJob);

            var hitJob = new SwarmHitJob()
            {
                Target = transform.position,

                Positions = minionPositions,
                Hits = minionhits,

                HitDistanceSQ = _minionHitDistance * _minionHitDistance
            };

            hitJob.Schedule(cnt, processorCount).Complete();

            UpdateHits(minionhits);

            minionhits.Dispose();
        }

        minionPositions.Dispose();
        minionDirections.Dispose();
    }

    public void Enable() => _enabled = true;

    public void Disable() => _enabled = false;

    public void Init()
    {
        if (_stageStartDelay == 0)
        {
            InitInternal();
            return;
        }

        GameTimer.CreateTimer("Init Delay " + gameObject.name, _stageStartDelay, InitInternal);
    }

    void InitInternal()
    {
        _enabled = true;
        SpawnMinions();
        AddTargets();
        _enemyHolder.AddTransform(transform);

        OnInitialized?.Invoke(this);

        if (_startPath != null)
            _movement.SetPath(_startPath);
    }

    void PopulateArrays(NativeArray<float3> positions, NativeArray<float3> directions)
    {
        for (int i = 0; i < _minions.Count; i++)
        {
            positions[i] = _minions[i].transform.position;
            directions[i] = _minions[i].transform.forward;
        }
    }

    void UpdateSwarm(NativeArray<float3> positions, NativeArray<float3> directions)
    {
        for (int i = 0; i < _minions.Count; i++)
        {
            Quaternion rot = Quaternion.LookRotation(directions[i]);
            _minions[i].SetPositionRotation(positions[i], rot);
        }
    }

    List<Minion> _hitMinions = new List<Minion>();
    void UpdateHits(NativeArray<bool> hits)
    {
        _hitMinions.Clear();

        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i])
                continue;

            _hitMinions.Add(_minions[i]);
        }

        foreach (var minion in _hitMinions)
            PlayerHit(minion);
    }

    public void SetPath(Path path) => _movement.SetPath(path);

    void SpawnMinions()
    {
        if (_minions != null && _minions.Count != 0)
            return;

        _minions = new List<Minion>();

        for (int i = 0; i < _swarmSize - _explosiveMinionCount; i++)
        {
            GameObject go = Pooler.Spawn(_minionPoolName, null, transform.position);
            Minion spawned = go.GetComponent<Minion>();
            spawned.SetPositionRotation(transform.position + UnityEngine.Random.insideUnitSphere * 20f, Quaternion.identity);

            _minions.Add(spawned);

            spawned.OnShotDown += MinionShot;
        }

        for (int i = 0; i < _explosiveMinionCount; i++)
        {
            GameObject go = Pooler.Spawn(_explosiveMinionPoolName, null, transform.position);
            Minion spawned = go.GetComponent<Minion>();
            spawned.SetPositionRotation(transform.position + UnityEngine.Random.insideUnitSphere * 20f, Quaternion.identity);

            _minions.Add(spawned);

            spawned.OnShotDown += MinionShot;
        }
    }

    void DestroyMinion(Minion minion)
    {
        // Unsub from events since it's down.
        minion.OnShotDown -= MinionShot;

        _targetHolder.RemoveTransform(minion.transform);

        // Play the VFX for the minion destruction.
        _vfxChannel.PlayVFX(_minionDeathVFX, minion.transform.position);

        // Remove the dead minion from the minions list.
        _minions.Remove(minion);

        // Tell pooler to release(Despawn) this object.
        Pooler.Release(_minionPoolName, minion.gameObject);
    }

    void MinionShot(Minion minion)
    {
        if (!_minions.Contains(minion))
            return;

        DestroyMinion(minion);

        if (_minions.Count != 0) // If there are minions left. Dont proceed.
            return;

        // If all the minions are shot down

        Death(); // Trigger death since this swarm is defeated.
    }

    void PlayerHit(Minion minion)
    {
        DestroyMinion(minion);
        _playerHitbox.TakeDamage(minion.Damage);

        if (_minions.Count > 0)
            return;

        Death();
    }

    public void Attack(Transform player)
    {
        _playerHitbox = player.GetComponentInChildren<IDamageable>();

        _movement.Stop();

        transform.position = player.position;

        _atacking = true;
    }

    void Death()
    {
        if (!_enabled)
            return;

        _enabled = false;

        _movement.Stop();

        _deathEvent.Invoke(this);

        OnLeaveStage?.Invoke(this);

        _enemyHolder.RemoveTransform(transform);

        gameObject.SetActive(false);
    }

    void AddTargets()
    {
        foreach (var minion in _minions)
            _targetHolder.AddTransform(minion.transform);
    }

    [BurstCompile]
    struct SwarmDirJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float3> Positions;
        public NativeArray<float3> Directions;

        public float3 Target;
        public float AvoidDistanceSQ;
        public float TargetBias;

        public float TurnSpeed;

        public float DeltaTime;

        public void Execute(int index)
        {
            float3 pos = Positions[index];
            float3 dir = Directions[index];

            float3 toTarget = Target - pos;
            float3 avoid = float3.zero;

            for (int i = 0; i < Positions.Length; i++)
            {
                if (i == index)
                    continue;

                float3 avoidDir = pos - Positions[i];
                float distSq = avoidDir.x * avoidDir.x + avoidDir.y * avoidDir.y + avoidDir.z * avoidDir.z;

                if (distSq > AvoidDistanceSQ)
                    continue;

                avoid += avoidDir;
            }

            float3 steer = toTarget / TargetBias;
            steer += avoid;

            dir += steer * DeltaTime * TurnSpeed;
            Directions[index] = math.normalize(dir);
        }
    }

    [BurstCompile]
    struct SwarmMoveJob : IJobParallelFor
    {
        public NativeArray<float3> Positions;
        [ReadOnly] public NativeArray<float3> Directions;

        public float Speed;
        public float DeltaTime;

        public void Execute(int index)
        {
            float3 dir = Directions[index];
            Positions[index] += Speed * DeltaTime * dir;
        }
    }

    [BurstCompile]
    struct SwarmHitJob : IJobParallelFor
    {
        public float3 Target;

        [ReadOnly] public NativeArray<float3> Positions;
        public NativeArray<bool> Hits;

        public float HitDistanceSQ;

        public void Execute(int index)
        {
            float3 pos = Positions[index];

            if (math.distancesq(Target, pos) > HitDistanceSQ)
                return;

            Hits[index] = true;
        }
    }
}