using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ListenPointerDown : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] UnityEvent _onPointerDown;

    public void OnPointerDown(PointerEventData eventData) => _onPointerDown.Invoke();
}
