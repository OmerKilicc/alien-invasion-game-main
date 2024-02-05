using System;

public interface IHealth
{
    public void Init(int baseHealth);
    public event Action<int> OnHealthChange;
    public int BaseHealth { get; }
    public int Health { get; set; }
}
