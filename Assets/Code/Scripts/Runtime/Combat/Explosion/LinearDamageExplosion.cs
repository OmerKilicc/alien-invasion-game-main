using UnityEngine;

public class LinearDamageExplosion : MonoBehaviour, IExplosive
{
    [SerializeField] int _minDamage;
    [SerializeField] int _maxDamage;
    [SerializeField] float _range;

    [SerializeField] Vector3 _offset;

    Vector3 Position => transform.TransformPoint(_offset);

    public void Explode()
    {
        RaycastHit[] hits = Physics.SphereCastAll(Position, _range, Vector3.up);

        if (hits.Length == 0)
            return;

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            if (hit.transform == transform || !hit.collider.TryGetComponent<IDamageable>(out var damageable))
                continue;

            float dist = Vector3.Distance(Position, hit.collider.transform.position);
            float step = Mathf.Clamp01((_range - dist) / _range);

            int dmg = (int)Mathf.Lerp(_minDamage, _maxDamage, step);
            damageable.TakeDamage(dmg);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Position, _range);
    }
}
