using Euphrates;
using UnityEngine;

public class DestructionAward : MonoBehaviour
{
    IDestruction _destruction;

    [SerializeField] IntSO _coins;
    [SerializeField] int _awarded;

    private void Awake() => _destruction = GetComponent<IDestruction>();

    private void OnEnable() => _destruction.OnDestructed += OnDestruct;

    private void OnDisable() => _destruction.OnDestructed -= OnDestruct;

    void OnDestruct() => _coins.Value += _awarded;
}
