using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Target Holder", menuName = "SO Channels/Transfrom Holder")]
public class TransformHolderSO : ScriptableObject
{
    [SerializeReference] List<Transform> _transforms = new List<Transform>();
    public int TransformCount => _transforms.Count;

    public event Action<Transform> OnTransformAdded;
    public event Action<Transform> OnTransformRemoved;

    public void AddTransform(Transform transform)
    {
        _transforms.Add(transform);
        OnTransformAdded?.Invoke(transform);
    }

    public void RemoveTransform(Transform transform)
    {
        _transforms.Remove(transform);
        OnTransformRemoved?.Invoke(transform);
    }

    public Transform GetTransform(int index)
    {
        if (index > TransformCount - 1 || index < 0)
            return null;

        return _transforms[index];
    }

    public void Clear() => _transforms.Clear();
}
