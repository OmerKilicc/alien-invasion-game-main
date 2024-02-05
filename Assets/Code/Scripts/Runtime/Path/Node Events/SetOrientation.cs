using Euphrates;
using UnityEngine;

[AddComponentMenu("Path/Node Events/Set Orientation")]
public class SetOrientation : MonoBehaviour, INodeEvent
{
    [SerializeField] Ease _ease = Ease.Lerp;
    [SerializeField] float _easeDuration = .5f;

    public void Invoke(Transform traveller)
    {
        Quaternion rotation = transform.rotation;

        traveller.DoRotation(rotation, _easeDuration, _ease);

        if (!traveller.TryGetComponent<PlayerControls>(out var control))
            return;

        control.SetTargetRotaion(rotation);
    }
}
