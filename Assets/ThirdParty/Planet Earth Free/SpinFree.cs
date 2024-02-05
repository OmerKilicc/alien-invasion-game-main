using UnityEngine;

/// <summary>
/// Spin the object at a specified speed
/// </summary>
public class SpinFree : MonoBehaviour
{
	Transform _transform;

	[SerializeField] bool _spin  = true;
	[SerializeField] float _speed = 1f;

	void Awake() => _transform = transform;

    void Update()
	{
		if (!_spin)
			return;

		_transform.rotation *= Quaternion.Euler(0, _speed * Time.deltaTime, 0);
	}
}