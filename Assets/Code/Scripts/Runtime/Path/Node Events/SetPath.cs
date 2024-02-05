using Euphrates.Path;
using UnityEngine;

[AddComponentMenu("Path/Node Events/Set Path")]
public class SetPath : MonoBehaviour, INodeEvent
{
    [SerializeField] Path _path;

    public void Invoke(Transform traveller)
    {
        if (_path == null || !traveller.TryGetComponent<ITraveller>(out var move))
            return;

        move.SetPath(_path);
    }

    private void OnDrawGizmosSelected()
    {
        if (!_path)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, _path.GetNode(0).transform.position);
    }
}
