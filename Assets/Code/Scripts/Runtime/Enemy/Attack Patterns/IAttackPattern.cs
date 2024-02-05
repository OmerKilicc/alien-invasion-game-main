using UnityEngine;

public interface IAttackPattern
{
    public void StartAttack(Transform target);
    public void StopAttack();
}
