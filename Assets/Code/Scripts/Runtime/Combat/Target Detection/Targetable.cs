using UnityEngine;

public class Targetable : MonoBehaviour
{
    [SerializeField] TransformHolderSO _targets;

    bool _isTargeted = false;

    public void AddAsTarget()
    {
        if (_isTargeted)
            return;

        _targets.AddTransform(transform);
        _isTargeted = true;
    }

    public void RemoveAsTarget()
    {
        if (!_isTargeted)
            return;

        _targets.RemoveTransform(transform);
        _isTargeted = false;
    }
}
