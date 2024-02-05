using Euphrates;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    static readonly string TIMER_NAME = "Blaster Laser Destroy - ";

    [SerializeField] string _laserPoolName;
    [SerializeField] Transform _blasterMuzzle;

    HashSet<string> _activeTimers = new HashSet<string>();

    private void OnDisable()
    {
        foreach (var tname in _activeTimers)
            GameTimer.CancleTimer(tname);
    }

    public void SpawnProjectile()
    {
        GameObject blasterLaser = Pooler.Spawn(_laserPoolName, _blasterMuzzle.position, _blasterMuzzle.rotation);

        string tname = TIMER_NAME + blasterLaser.GetInstanceID();

        void ReleaseTimer()
        {
            _activeTimers.Remove(tname);
            Pooler.Release(_laserPoolName, blasterLaser);
        }

        GameTimer.CreateTimer(tname, 4f, ReleaseTimer);
        _activeTimers.Add(tname);
    }
}