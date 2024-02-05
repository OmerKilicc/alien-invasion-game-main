using UnityEngine;
using System.Collections;

public class ShotBehavior : MonoBehaviour {

	[SerializeField] float _blasterSpeed = 400f;
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * Time.deltaTime * _blasterSpeed;
	}
}
