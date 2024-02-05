using UnityEngine;

public class StartAttack : MonoBehaviour, INodeEvent
{
    Transform _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }

    public void Invoke(Transform traveller)
    {
        if (!traveller.TryGetComponent<IEnemy>(out var enemy))
            return;

        enemy.Attack(_player);
    }
}
