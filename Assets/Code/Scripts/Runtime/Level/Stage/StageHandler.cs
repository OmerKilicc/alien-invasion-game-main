using Euphrates;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageHandler : MonoBehaviour
{
    int CurrentStage
    {
        get
        {
            int min = int.MaxValue;

            foreach (var sob in _stageObjects)
            {
                if (sob.StartStage > min)
                    continue;

                min = sob.StartStage;
            }

            return min;
        }
    }

    [SerializeField] TriggerChannelSO _resetStages;
    [SerializeField] TriggerChannelSO _setStage;
    [SerializeField] TriggerChannelSO _startStage;
    [SerializeField] TriggerChannelSO _allStagesComplete;

    [Space]
    [SerializeField] IntSO _stage;
    [SerializeField] IntSO _stageCount;

    List<IStageObject> _stageObjects = new List<IStageObject>();
    List<IStageObject> _currentStageObjects = new List<IStageObject>();

    private void OnEnable()
    {
        _resetStages.AddListener(ResetStages);
        _startStage.AddListener(StartCurrentStage);

    }

    private void OnDisable()
    {
        _resetStages.RemoveListener(ResetStages);
        _startStage.RemoveListener(StartCurrentStage);
    }

    void GetStageObjects() => _stageObjects = FindObjectsOfType<MonoBehaviour>().OfType<IStageObject>().ToList();

    void GetCurrentStageObjects()
    {
        int currentStage = CurrentStage;

        _currentStageObjects.Clear();

        foreach (var sob in _stageObjects)
        {
            if (sob.StartStage != currentStage)
                continue;

            _currentStageObjects.Add(sob);
            sob.OnLeaveStage += OnObjectLeaveStage;
        }
    }

    public void ResetStages()
    {
        GetStageObjects();

        SetStageCount();

        SetStage();
    }

    void SetStageCount()
    {
        int cnt = 0;

        List<int> foundStages = new List<int>();

        foreach (var sto in _stageObjects)
        {
            if (foundStages.Exists(s => s == sto.StartStage))
                continue;

            cnt++;
            foundStages.Add(sto.StartStage);
        }

        _stageCount.Value = cnt;
    }

    void CompleteCurrentStage()
    {
        int currentStage = CurrentStage;

        _stageObjects.RemoveAll(s => s.StartStage == currentStage);

        if (_stageObjects.Count != 0)
        {
            SetStage();
            return;
        }

        _allStagesComplete.Invoke();
    }

    void StartCurrentStage()
    {
        foreach (var sob in _currentStageObjects)
            sob.Init();
    }

    void OnObjectLeaveStage(IStageObject stageObject)
    {
        stageObject.OnLeaveStage -= OnObjectLeaveStage;

        _currentStageObjects.Remove(stageObject);

        if (_currentStageObjects.Count != 0)
            return;

        CompleteCurrentStage();
    }

    void SetStage()
    {
        _stage.Value = CurrentStage;

        GetCurrentStageObjects();
        _setStage.Invoke();
    }
}
