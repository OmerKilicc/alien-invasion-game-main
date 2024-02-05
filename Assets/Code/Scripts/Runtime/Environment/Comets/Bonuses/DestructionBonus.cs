using UnityEngine;

[RequireComponent(typeof(IDestruction))]
public class DestructionBonus : MonoBehaviour
{
    const string NO_IMPLEMENTATION_WARNING = "[{0}] OnDestruction Method was not overwritten by [{1}]";

    protected IDestruction _destruction;

    protected virtual void Awake() => _destruction = GetComponent<IDestruction>();

    protected virtual void OnEnable() => _destruction.OnDestructed += OnDestruction;

    protected virtual void OnDisable() => _destruction.OnDestructed -= OnDestruction;

    protected virtual void OnDestruction() => Debug.LogWarning(string.Format(NO_IMPLEMENTATION_WARNING, typeof(DestructionAward).Name, this.GetType().Name));
}
