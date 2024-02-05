using Euphrates;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] LayerMaskSO _shotLayer;

    [Space]
    [Header("Stats")]
    [SerializeField] IntSO _blastersDamage;
    [SerializeField] FloatSO _weaponRange;
    [SerializeField] FloatSO _fireInterval;

    [Space]
    [Header("Events")]
    [SerializeField] TriggerChannelSO _onShootTrigger;
    [SerializeField] TriggerChannelSO _shootingStartTrigger;
    [SerializeField] TriggerChannelSO _shootingStopTrigger;
    [SerializeField] TriggerChannelSO _enemyHitTrigger;

    IRayProvider _rayProvider;

    bool _canShoot = false;
    bool _paused = false;

    void Start()
    {
        _rayProvider = GetComponent<IRayProvider>();
    }
    private void OnEnable()
    {
        _shootingStartTrigger.AddListener(StartShooting);
        _shootingStopTrigger.AddListener(StopShooting);
    }

    private void OnDisable()
    {
        _shootingStartTrigger.RemoveListener(StartShooting);
        _shootingStopTrigger.RemoveListener(StopShooting);
    }

    void StartShooting() => _canShoot = true;
    void StopShooting() => _canShoot = false;

    public void PauseShooting() => _paused = true;
    public void UnPauseShooting() => _paused = false;

    float _timePassed = 0.0f;
    private void Update()
    {
        _timePassed += Time.deltaTime;

        if (!_canShoot || _paused || _timePassed < _fireInterval)
            return;

        _timePassed = 0.0f;

        Shoot();
    }

    RaycastHit[] _hits = new RaycastHit[10];
    void Shoot()
    {
        // Create a vector at the center of our camera's viewport
        Ray ray = _rayProvider.CreateRay();

        _onShootTrigger.Invoke();

        //int hitCount = Physics.RaycastNonAlloc(ray, _hits, _weaponRange, _shotLayer.Value);

        ////// Check if our raycast has hit anything
        //if (hitCount == 0)
        //    return;


        //if (!_hits[hitCount - 1].collider.TryGetComponent<IDamageable>(out var damageable))
        //    return;

        if (!Physics.Raycast(ray, out var hit, _weaponRange, _shotLayer.Value))
            return;

        if (!hit.collider.TryGetComponent<IDamageable>(out var damageable))
            return;

        _enemyHitTrigger.Invoke();

        damageable.TakeDamage(_blastersDamage.Value);
    }

}
