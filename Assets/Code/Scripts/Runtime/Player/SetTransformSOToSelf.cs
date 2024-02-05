using Euphrates;
using UnityEngine;

public class SetTransformSOToSelf : MonoBehaviour
{
    [SerializeField] TransformSO _transform;

    private void Awake()
    {
        Set();
    }

    public void Set() => _transform.Value = transform;
}
