using UnityEngine;

public class ForwardRay : MonoBehaviour, IRayProvider
{
    public Ray CreateRay() => new Ray(transform.position, transform.forward);
}
