using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Event", menuName = "SO Channels/Enemy")]
public class EnemyEventSO : ScriptableObject
{
    public event Action<IEnemy> OnTrigger;
    public void Invoke(IEnemy enemy) => OnTrigger?.Invoke(enemy);
}
