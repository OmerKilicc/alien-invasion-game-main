using Euphrates;
using System.Threading.Tasks;
using UnityEngine;

public class MissileTutorialHandler : MonoBehaviour
{
    [SerializeField] TriggerChannelSO _missileTutorialStart;
    [SerializeField] TriggerChannelSO _missileTutorialComplete;

    [Space]
    [SerializeField] TriggerChannelSO _enableControls;
    [SerializeField] TriggerChannelSO _disableControls;

    [Space]
    [SerializeField] IntSO _stage;
    [SerializeField] TriggerChannelSO _startStage;

    private void OnEnable() => _startStage.AddListener(StageStart);

    private void OnDisable() => _startStage.RemoveListener(StageStart);

    void StageStart()
    {
        if (_stage == 0)
        {
            StartTutorial();
            return;
        }

        if (_stage == 1)
            EndTutorial();
    }

    async void StartTutorial()
    {
        await Task.Yield();

        print("Tutorial Started");
        _disableControls.Invoke();
        _missileTutorialStart.Invoke();
    }

    async void EndTutorial()
    {
        await Task.Yield();

        print("Tutorial Ended");
        _enableControls.Invoke();
        _missileTutorialComplete.Invoke();

        enabled = false;
    }

}
