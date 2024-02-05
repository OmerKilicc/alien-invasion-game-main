using UnityEngine;

public class DestructibleEnemyIndicator : MonoBehaviour
{
    IDestruction _destruction;

    [SerializeField] TransformHolderSO _enemies;

    private void Awake()
    {
        _destruction = GetComponent<IDestruction>();
    }

    private void OnEnable()
    {
        _destruction.OnDestructed += RemoveEnemy;
    }

    private void OnDisable()
    {
        _destruction.OnDestructed -= RemoveEnemy;
        RemoveEnemy();
    }

    private void Start() => AddEnemy();

    void AddEnemy() => _enemies.AddTransform(transform);

    void RemoveEnemy() => _enemies.RemoveTransform(transform);
}
