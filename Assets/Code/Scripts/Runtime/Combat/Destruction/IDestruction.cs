using System;

public interface IDestruction
{
    public event Action OnDestructed;
    public void Destruct();
}
