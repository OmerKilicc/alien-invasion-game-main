using UnityEngine;

public class PlayerMovementForBonusLevel : MonoBehaviour
{
    [SerializeField] float _speed;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }
}
