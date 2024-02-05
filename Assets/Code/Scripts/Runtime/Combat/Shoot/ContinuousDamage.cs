using Euphrates;
using UnityEngine;

[RequireComponent(typeof(IRayProvider))]
public class ContinuousDamage : MonoBehaviour
{
    [SerializeField] FloatSO _damageInterval;
    [SerializeField] IntSO _damageAmount;

    [SerializeField] LayerMask _enemyLayer;

    IRayProvider _rayProvider;

    private void Awake()
    {
        _rayProvider = GetComponent<IRayProvider>();
    }

    RaycastHit[] _hits = new RaycastHit[50];
    float _timePassed = 0f;
    private void Update()
    {
        _timePassed += Time.deltaTime;

        if (_timePassed < _damageInterval)
            return;

        _timePassed = 0f;

        Shoot();
    }

    void Shoot()
    {
        Ray ray = _rayProvider.CreateRay();

        int cnt = Physics.RaycastNonAlloc(ray, _hits, 1000f, _enemyLayer);

        if (cnt == 0)
            return;

        for (int i = 0; i < cnt; i++)
        {
            RaycastHit hit = _hits[i];

            if (!hit.collider.TryGetComponent<IDamageable>(out var damageable))
                continue;

            damageable.TakeDamage(_damageAmount);
        }
    }
}
