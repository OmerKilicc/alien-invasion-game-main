using Euphrates;
using System.Collections.Generic;
using UnityEngine;

public class CometHandler : MonoBehaviour
{
    const string COMET_TIMER_NAME = "comet_life_";
    const float COMET_SPAWN_DELAY = 5f;

    [SerializeField] Comet _cometPrefab;

    [Space]
    [SerializeField] FloatSO _cometLifetime;
    [SerializeField] FloatSO _cometSpeed;
    [SerializeField] FloatSO _cometSpawnDistance;
    [SerializeField] FloatSO _cometSpawnOffset;
    [SerializeField] IntSO _maxPerLevel;

    [Space]
    [SerializeField] CometSpawn[] _spawns;
    [SerializeField] TriggerChannelSO[] _despawns;

    [Space]
    [SerializeField] TransformSO _player;

    List<Comet> _spawnedComets = new List<Comet>();

    int _spawnCount = 0;
    float _lastSpawnTime = 0;

    private void OnEnable()
    {
        for (int i = 0; i < _spawns.Length; i++)
        {
            _spawns[i].Handler = this;
            _spawns[i].Event.AddListener(_spawns[i].Spawn);
        }

        for (int i = 0; i < _despawns.Length; i++)
            _despawns[i].AddListener(HandleDespawningAll);
    }

    private void OnDisable()
    {
        for (int i = 0; i < _spawns.Length; i++)
        {
            _spawns[i].Handler = this;
            _spawns[i].Event.RemoveListener(_spawns[i].Spawn);
        }

        for (int i = 0; i < _despawns.Length; i++)
            _despawns[i].RemoveListener(HandleDespawningAll);
    }

    private void Update()
    {
        foreach (Comet comet in _spawnedComets)
            comet.transform.position += _cometSpeed * Time.deltaTime * comet.transform.forward;
    }

    void HandleDespawningAll()
    {
        for (int i = _spawnedComets.Count - 1; i >= 0; i--)
            DestroyComet(_spawnedComets[i]);

        _spawnCount = 0;
    }

    public void SpawnComet(CometSpawn spawn)
    {
        if (!ShouldSpawn(spawn.Chance))
            return;

        Vector3 spawnOrgin = _player.Value.position + _player.Value.forward * _cometSpawnDistance;
        Vector3 spawnPos = spawnOrgin + Random.insideUnitSphere * _cometSpawnOffset;
        Quaternion spawnRotation = Quaternion.LookRotation((spawnOrgin - spawnPos).normalized);

        Comet comet = CreateComet(spawnPos, spawnRotation);
        SetCometLifetime(comet, _cometLifetime.Value);
        comet.OnDestructed += () => DestroyComet(comet);

        comet.Init();

        _spawnCount++;
        _lastSpawnTime = Time.time;

        _spawnedComets.Add(comet);
    }

    bool ShouldSpawn(float chance)
    {
        if (_spawnCount >= _maxPerLevel)
            return false;

        if (Time.time < _lastSpawnTime + COMET_SPAWN_DELAY)
            return false;

        chance = Mathf.Clamp01(chance);
        float rand = Random.Range(0f, 1f);
        return chance > rand;
    }

    Comet CreateComet(Vector3 position, Quaternion rotation)
    {
        Comet rval = Instantiate(_cometPrefab, transform);
        rval.transform.SetPositionAndRotation(position, rotation);

        return rval;
    }

    void SetCometLifetime(Comet comet, float lifetime)
    {
        GameTimer.CreateTimer(COMET_TIMER_NAME + comet.gameObject.GetInstanceID(), lifetime, () => DestroyComet(comet));
    }

    void DestroyComet(Comet comet)
    {
        if (comet == null)
            return;

        _spawnedComets.Remove(comet);
        Destroy(comet.gameObject);
    }

    [System.Serializable]
    public struct CometSpawn
    {
        [HideInInspector] public CometHandler Handler;
        public string Name;
        public TriggerChannelSO Event;
        public float Chance;

        public void Spawn() => Handler.SpawnComet(this);
    }
}
