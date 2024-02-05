using UnityEngine;

[ExecuteInEditMode]
public class SunPosition : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] Transform _directionalLight;
    [SerializeField] float _distanceFromOrigin = 2000;

    void Update()
    {
        if (!_directionalLight)
            return;

        Vector3 dir = -_directionalLight.forward;
        transform.position = dir * _distanceFromOrigin;
    }
#endif
}
