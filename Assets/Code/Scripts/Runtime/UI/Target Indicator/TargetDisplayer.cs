using System.Collections.Generic;
using UnityEngine;

public class TargetDisplayer : MonoBehaviour
{
    [SerializeField] TransformHolderSO _enemies;
    [SerializeField] RectTransform _canvasRect;

    [SerializeField] float _outOfSightOffset = 20f;

    [SerializeField] List<TargetIndicator> _indicators = new List<TargetIndicator>();
    Dictionary<Transform, TargetIndicator> _actives = new Dictionary<Transform, TargetIndicator>();

    private void Awake()
    {
        foreach (var ind in _indicators)
            ind.Stop();
    }

    private void OnEnable()
    {
        ResetActives();
        SetEnemies();

        _enemies.OnTransformAdded += TargetAdded;
        _enemies.OnTransformRemoved += TargetRemoved;
    }

    private void OnDisable()
    {
        _enemies.OnTransformAdded -= TargetAdded;
        _enemies.OnTransformRemoved -= TargetRemoved;

        ResetActives();
    }

    void SetEnemies()
    {
        for (int i = 0; i < _enemies.TransformCount; i++)
            TargetAdded(_enemies.GetTransform(i));
    }

    void TargetAdded(Transform target)
    {
        var indicator = _indicators.FindLast(x => !_actives.ContainsValue(x));

        if (indicator == null)
            return;

        indicator.Init(target, _outOfSightOffset, _canvasRect);
        _actives.Add(target, indicator);
    }

    void TargetRemoved(Transform target)
    {
        if (!_actives.ContainsKey(target))
            return;

        var indicator = _actives[target];

        indicator.Stop();
        _actives.Remove(target);
    }

    void ResetActives()
    {
        if (_actives == null || _actives.Count == 0)
            return;

        foreach (var item in _actives)
            item.Value.Stop();

        _actives.Clear();
    }
}
