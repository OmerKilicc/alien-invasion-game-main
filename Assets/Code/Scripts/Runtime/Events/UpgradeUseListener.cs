using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Upgrader))]
public class UpgradeUseListener : MonoBehaviour
{
    Upgrader _upgrader;
    [SerializeField] UnityEvent _onUpgradeUsed;

    private void Awake() => _upgrader = GetComponent<Upgrader>();

    private void OnEnable() => _upgrader.OnUse += _onUpgradeUsed.Invoke;

    private void OnDisable() => _upgrader.OnUse -= _onUpgradeUsed.Invoke;
}
