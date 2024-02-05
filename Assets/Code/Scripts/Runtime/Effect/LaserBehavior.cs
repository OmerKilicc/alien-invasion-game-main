using Euphrates;
using UnityEngine;

public class LaserBehavior : MonoBehaviour, IPoolable
{
    const string TIMER_NAME = "laser_death_timer[{0}]";

    [SerializeField] TransformHolderSO _laserHolder;

    private void OnDestroy()
    {
        _laserHolder.RemoveTransform(transform);
        CancleTimer();
    }

    public void OnDestroyed() { }

    public void OnGet()
    {
        _laserHolder.AddTransform(transform);
        GameTimer.CreateTimer(string.Format(TIMER_NAME, gameObject.GetInstanceID().ToString()), 5f, OnReleased);
    }

    public void OnReleased()
    {
        _laserHolder.RemoveTransform(transform);
        CancleTimer();
    }

    void CancleTimer() => GameTimer.CancleTimer(string.Format(TIMER_NAME, gameObject.GetInstanceID().ToString()));
}
