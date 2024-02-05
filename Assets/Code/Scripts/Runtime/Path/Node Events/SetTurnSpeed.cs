using UnityEngine;

[AddComponentMenu("Path/Node Events/Set Turn Speed")]
public class SetTurnSpeed : MonoBehaviour, INodeEvent
{
    [SerializeField] float _turnSpeed;
    public void Invoke(Transform traveller)
    {
        if (!traveller.TryGetComponent<ITraveller>(out var move))
            return;

        move.TurnSpeed = _turnSpeed;
    }
}
