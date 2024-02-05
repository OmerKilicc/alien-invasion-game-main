using System;

public interface IStageObject 
{
    public int StartStage { get; }
    public float Delay { get; }

    public event Action<IStageObject> OnInitialized;
    public event Action<IStageObject> OnLeaveStage;

    public void Init();
}
