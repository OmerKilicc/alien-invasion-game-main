using Euphrates;
using UnityEngine;

public class EnemyExplosionTrigger : MonoBehaviour
{
    IEnemy _enemy;
    [SerializeField] TriggerChannelSO _enemyExplosionTrigger;

    private void Awake()
    {
        _enemy = GetComponent<IEnemy>();
    }

    private void OnEnable()
    {
        _enemy.OnDefeated += _enemyExplosionTrigger.Invoke;
    }

    private void OnDisable()
    {
        _enemy.OnDefeated -= _enemyExplosionTrigger.Invoke;
    }
}
