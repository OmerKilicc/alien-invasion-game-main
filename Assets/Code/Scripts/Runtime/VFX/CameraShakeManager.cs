using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Euphrates;

public class CameraShakeManager : MonoBehaviour
{
    CinemachineImpulseSource _cameraShake;

    [SerializeField] TriggerChannelSO _explosionTrigger;
    [SerializeField] TriggerChannelSO _playerHasBeenHitTrigger;

    [SerializeField] float _explosionPower = 0.05f;
    [SerializeField] float _playerHasBeenHitPower = 0.01f;

    [SerializeField] Vector3 _explosionVelocityVector = new Vector3(0.4f, -0.4f, 0);
    [SerializeField] Vector3 _playerHasBeenHitVelocityVector = new Vector3(0.1f, -0.1f, 0);

    private void Start()
    {
        _cameraShake = GetComponent<CinemachineImpulseSource>();
    }
    private void OnEnable()
    {

        _explosionTrigger.AddListener(ExplosionShake);
        _playerHasBeenHitTrigger.AddListener(PlayerHasBeenHitShake);
    }

    private void OnDisable()
    {

        _explosionTrigger.RemoveListener(ExplosionShake);
        _playerHasBeenHitTrigger.RemoveListener(PlayerHasBeenHitShake);
    }

    void ExplosionShake()
    {
        _cameraShake.m_DefaultVelocity = _explosionVelocityVector;
        _cameraShake.GenerateImpulse(_explosionPower);
    }

    void PlayerHasBeenHitShake()
    {
        _cameraShake.m_DefaultVelocity = _playerHasBeenHitVelocityVector;
        _cameraShake.GenerateImpulse(_playerHasBeenHitPower);
    }
}
