using UnityEngine;
using UnityEngine.Events;

public class AllEnemiesDefeated : MonoBehaviour
{
    bool _enabled = true;

    [SerializeField] TransformHolderSO _targets;
    [SerializeField] UnityEvent _onAllDefeated;

    private void OnEnable()
    {
        _targets.OnTransformRemoved += OnTargetDestroyed;
    }

    private void OnDisable()
    {
        _targets.OnTransformRemoved -= OnTargetDestroyed;
    }

    void OnTargetDestroyed(Transform _)
    {
        if (!_enabled || _targets.TransformCount != 0)
            return;

        _onAllDefeated.Invoke();
        _enabled = false;
    }
}
