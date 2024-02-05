using UnityEngine;
using UnityEngine.Events;

public class EventTriggerNode : MonoBehaviour, INodeEvent
{
    public UnityEvent OnTrigger;

    public void Invoke(Transform traveller) => OnTrigger?.Invoke();
}
