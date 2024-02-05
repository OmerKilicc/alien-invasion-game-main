using UnityEditor;
using UnityEngine;
using Euphrates.Path;

[AddComponentMenu("Path/Node")]
public class PathNode : MonoBehaviour
{
    INodeEvent[] _events;

    private void Start() => _events = GetComponents<INodeEvent>();

    public void Reached(Transform enemy)
    {
        for (int i = 0; i < _events.Length; i++)
            _events[i].Invoke(enemy);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, .24f);
    }

    [MenuItem("GameObject/Path/Node")]
    static PathNode CreateNode()
    {
        if (Selection.activeGameObject == null || !Selection.activeGameObject.TryGetComponent<Path>(out var path))
            path = Path.CreatePath();

        GameObject go = new GameObject("Path Node");
        go.transform.SetParent(path.transform);
        go.transform.localPosition = Vector3.zero;

        if (path.NodeCount != 0)
        {
            PathNode lastNode = path.GetNode(path.NodeCount - 1);
            go.transform.position = lastNode.transform.position + lastNode.transform.forward * 2;
        }

        PathNode node = go.AddComponent<PathNode>();
        path.AddNode(node);
        return node;
    }

    public static PathNode CreateNode(Vector3 pos)
    {
        if (Selection.activeGameObject == null || !Selection.activeGameObject.TryGetComponent<Path>(out var path))
            path = Path.CreatePath();

        GameObject go = new GameObject("Path Node");
        go.transform.SetParent(path.transform);
        go.transform.position = pos;

        PathNode node = go.AddComponent<PathNode>();
        path.AddNode(node);
        return node;
    }
#endif
}
