using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IStageObject))]
public class StageObjVfx : MonoBehaviour
{
    IStageObject _stageObj;
    [SerializeField] ParticleSystem _particles;

    private void Awake() => _stageObj = GetComponent<IStageObject>();

    private void OnEnable() => _stageObj.OnInitialized += PlayVfx;

    private void OnDisable() => _stageObj.OnInitialized -= PlayVfx;

    void PlayVfx(IStageObject _) => _particles.Play();
}
