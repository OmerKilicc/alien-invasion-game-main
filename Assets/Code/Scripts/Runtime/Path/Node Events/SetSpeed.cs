using UnityEngine;

[AddComponentMenu("Path/Node Events/Set Speed")]
public class SetSpeed : MonoBehaviour, INodeEvent
{
    [SerializeField] float _speed;
    public void Invoke(Transform traveller)
    {
        if (!traveller.TryGetComponent<ITraveller>(out var move))
            return;

        move.Speed = _speed;
    }
}
