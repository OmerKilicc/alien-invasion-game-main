using System;
using UnityEngine;
using Euphrates.Path;

public interface ITraveller
{
    public bool FaceMoveVector { get; set; }
    public void SetFacingTarget(Transform target);
    public void MoveToPosition(Vector3 position, Transform parent = null);
    public void MoveOnPathSegment(Vector3[] path, Transform parent = null);
    public void SetPath(Path path);
    public event Action OnReachedPosition;
    public event Action OnPathEnded;
    public float Speed { get; set; }
    public float TurnSpeed { get; set; }
    public void Stop();
}
