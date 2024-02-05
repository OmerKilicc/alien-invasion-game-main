using UnityEngine;

public class RayDisplayer : MonoBehaviour
{
    IRayProvider _rayProvider;

    private void Awake() => _rayProvider = GetComponent<IRayProvider>();

    private void OnDrawGizmos()
    {
        if (_rayProvider == null)
        {
            _rayProvider = GetComponent<IRayProvider>();
            return;
        }

        Ray ray = _rayProvider.CreateRay();

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(ray.origin, ray.direction * 100f);
    }
}
