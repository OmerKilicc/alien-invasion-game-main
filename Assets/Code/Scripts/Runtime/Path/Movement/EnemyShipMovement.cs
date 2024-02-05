using UnityEngine;

public class EnemyShipMovement : ShipMovement
{
    [Space]
    [SerializeField] EnemySO _enemyData;

    private void Start()
    {
        if (_enemyData == null)
            return;

        if (_enemyData.Speed != -1)
            _speed = _enemyData.Speed;

        if (_enemyData.TurnSpeed != -1)
            _turnSpeed = _enemyData.TurnSpeed;
    }
}
