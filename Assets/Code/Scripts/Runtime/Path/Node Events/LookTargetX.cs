using Euphrates;
using UnityEngine;

[AddComponentMenu("Path/Node Events/Look Target X")]
public class LookTargetX : MonoBehaviour, INodeEvent
{
    [SerializeField] Transform _target;

    public void Invoke(Transform traveller)
    {
        if (!SetTarget())
            return;

        Vector3 travellerLocal = _target.InverseTransformPoint(traveller.position);

        Vector3 pos = new Vector3(travellerLocal.x, 0f, 0f);
        pos = _target.TransformPoint(pos);

        Quaternion rot = Quaternion.LookRotation((pos - traveller.position).normalized);
        traveller.DoRotation(rot, 1f);
    }

    bool SetTarget()
    {
        if (_target != null)
            return true;

        GameObject go = GameObject.FindWithTag("Player");

        if (go == null)
            return false;

        _target = go.transform;

        return true;
    }
}
