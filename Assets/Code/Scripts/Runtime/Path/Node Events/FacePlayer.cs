using UnityEngine;

[AddComponentMenu("Path/Node Events/Face Player")]
public class FacePlayer : MonoBehaviour, INodeEvent
{
    Transform _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }

    public void Invoke(Transform traveller)
    {
        if (_player == null || !traveller.TryGetComponent<ITraveller>(out var move))
            return;

        move.SetFacingTarget(_player);
    }

}
