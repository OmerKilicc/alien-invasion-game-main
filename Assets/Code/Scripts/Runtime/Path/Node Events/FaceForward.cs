using UnityEngine;

[AddComponentMenu("Path/Node Events/Face Forward")]
public class FaceForward : MonoBehaviour, INodeEvent
{
    [SerializeField] bool _faceForward;
    public void Invoke(Transform traveller)
    {
        if (!traveller.TryGetComponent<ITraveller>(out var move))
            return;

        move.FaceMoveVector = _faceForward;
    }
}
