using System;
using UnityEngine;

public interface IEnemy
{
    public EnemySO EnemyData { get; }
    public event Action OnDefeated;
    public void Attack(Transform player);
}
