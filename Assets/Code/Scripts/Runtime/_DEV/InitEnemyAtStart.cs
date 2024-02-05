using UnityEngine;

[RequireComponent(typeof(IStageObject))]
public class InitEnemyAtStart : MonoBehaviour
{
    IStageObject _stageObject;

    private void Awake() => _stageObject = GetComponent<IStageObject>();

    void Start() => _stageObject.Init();
}
