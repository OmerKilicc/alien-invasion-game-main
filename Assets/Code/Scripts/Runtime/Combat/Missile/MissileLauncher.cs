using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    [SerializeField] GameObject _prefab;

    public void Launch()
    {
        GameObject go = Instantiate(_prefab);
        go.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }
}
