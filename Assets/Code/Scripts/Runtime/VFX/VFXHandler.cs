using System.Collections.Generic;
using UnityEngine;

public class VFXHandler : MonoBehaviour
{
    [SerializeField] VFXChannelSO _channel;
    [SerializeField] List<VFXData> _VFXs;

    Dictionary<string,SpawnedVFX> _spawned;

    private void OnEnable()
    {
        _channel.OnPlayVFX += PlayVFX;
    }

    private void OnDisable()
    {
        _channel.OnPlayVFX -= PlayVFX;
    }

    private void Start()
    {
        SpawnFX();
    }

    void SpawnFX()
    {
        _spawned = new Dictionary<string, SpawnedVFX>();

        foreach (var vfx in _VFXs)
        {
            SpawnedVFX svfx = new SpawnedVFX(vfx.SpawnAmount);

            for (int i = 0; i < vfx.SpawnAmount; i++)
            {
                GameObject go = Instantiate(vfx.Prefab, transform);
                go.name = vfx.Name;

                svfx.SetInstance(i, go);
            }

            _spawned[vfx.Name] = svfx;
        }
    }

    public void PlayVFX(string fxName, Vector3 position)
    {
        if (!_spawned.ContainsKey(fxName))
            return;

        SpawnedVFX svfx = _spawned[fxName];

        GameObject go = svfx.GetNext();
        go.transform.position = position;

        if (!go.TryGetComponent<ParticleSystem>(out var particle))
            return;

        particle.Play(true);
    }

    [System.Serializable]
    public struct VFXData
    {
        public string Name;
        public GameObject Prefab;
        public int SpawnAmount;
    }

    class SpawnedVFX
    {
        GameObject[] _instances;
        int _index;

        public SpawnedVFX(int size)
        {
            _instances= new GameObject[size];
            _index = 0;
        }

        public void SetInstance(int index, GameObject instance) => _instances[index] = instance;

        public GameObject GetNext()
        {
            _index %= _instances.Length;
            return _instances[_index++];
        }
    }
}
