using UnityEngine;
using UnityEngine.InputSystem;

public class DamageSingleEnemy : MonoBehaviour
{
    IDamageable _enemyHitbox;

    private void Start()
    {
        _enemyHitbox = FindObjectOfType<Hitbox>();
    }

    void Update()
    {
        if (!Keyboard.current.spaceKey.wasPressedThisFrame)
            return;

        _enemyHitbox.TakeDamage(10);
    }
}
