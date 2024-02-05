using UnityEngine;
using Euphrates.Path;

[RequireComponent(typeof(ShipMovement))]
public class StartPath : MonoBehaviour
{
    ShipMovement _shipMovement;
    [SerializeField] Path _path;

    private void Awake() => _shipMovement = GetComponent<ShipMovement>();

    private void Start() => _shipMovement.SetPath(_path);
}
