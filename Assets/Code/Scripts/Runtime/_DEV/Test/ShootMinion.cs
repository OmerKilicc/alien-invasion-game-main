using UnityEngine;
using UnityEngine.InputSystem;

public class ShootMinion : MonoBehaviour
{
    void Update()
    {
        if (!Keyboard.current.spaceKey.wasPressedThisFrame)
            return;

        Minion m = FindObjectOfType<Minion>();

        if (!m)
            return;

        m.TakeDamage(1);
    }
}
