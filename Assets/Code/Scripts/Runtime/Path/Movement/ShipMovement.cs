using System;
using UnityEngine;
using Euphrates.Path;

public class ShipMovement : MonoBehaviour, ITraveller
{
    protected static readonly float REACH_TRESHOLD = .45f;

    protected Transform _transform;
    protected Transform _parent;

    protected bool _enabled = false;

    [SerializeField] protected bool _faceMove = true;
    public bool FaceMoveVector { get => _faceMove; set => _faceMove = value; }

    [SerializeField] protected float _speed = 10f;
    public float Speed { get => _speed; set => _speed = value; }

    [SerializeField] protected float _turnSpeed = 5f;
    public float TurnSpeed { get => _turnSpeed; set => _turnSpeed = value; }

    Vector3[] _movePositions;
    int _pathIndex = 0;

    Vector3 TargetPosition
    {
        get
        {
            if (_parent == null)
                return _movePositions[_pathIndex];

            return _parent.TransformPoint(_movePositions[_pathIndex]);

        }
    }
    Vector3 TargetDirection => TargetPosition - _transform.position;

    Transform _aimTarget;

    public event Action OnReachedPosition;
    public event Action OnPathEnded;

    protected void Awake() => _transform = transform;

    protected virtual void FixedUpdate()
    {
        if (!_faceMove && _aimTarget != null)
            SetFacing(_aimTarget.position);

        if (!_enabled)
            return;

        if (_faceMove)
            SetFacing(TargetPosition);

        Move();

        if (!CheckReached())
            return;

        if (_pathIndex != _movePositions.Length - 1)
        {
            _pathIndex++;
            return;
        }

        PositionReached();
    }

    public void Stop()
    {
        _path = null;
        _enabled = false;
    }

    #region Move Operations
    public void MoveToPosition(Vector3 position, Transform parent = null)
    {
        _enabled = true;
        _parent = parent;

        _pathIndex = 0;
        _movePositions = new Vector3[1] { position };
    }

    public void MoveOnPathSegment(Vector3[] path, Transform parent = null)
    {
        _enabled = true;
        _parent = parent;

        _pathIndex = 0;
        _movePositions = path;
    }

    public void SetFacingTarget(Transform target)
    {
        _faceMove = false;
        _aimTarget = target;
    }

    protected virtual void Move() => _transform.position += _speed * Time.fixedDeltaTime * TargetDirection.normalized;

    protected virtual bool CheckReached()
    {
        if (Vector3.Distance(_transform.position, TargetPosition) > REACH_TRESHOLD)
            return false;

        return true;
    }

    void SetFacing(Vector3 target) => _transform.forward = Vector3.Lerp(_transform.forward, (target - _transform.position).normalized, _turnSpeed * Time.deltaTime);
    #endregion

    #region Path Operations
    protected Path _path;
    protected int _segmentIndex = 0;
    public void SetPath(Path path)
    {
        _path = path;
        StartPath();
    }

    protected virtual void StartPath()
    {
        _segmentIndex = 0;

        PathNode node = _path.GetNode(_segmentIndex);

        MoveToPosition(node.transform.position);
    }


    protected virtual void SetOnPath()
    {
        Vector3[] pathSegment = _path.CreateSegmentVertex(_segmentIndex);
        _segmentIndex++;
        MoveOnPathSegment(pathSegment);
    }

    protected virtual void PositionReached()
    {
        _enabled = false;
        OnReachedPosition?.Invoke();

        if (_path == null)
            return;

        var reached = _path.GetNode(_segmentIndex);
        var next = _path.GetNode(_segmentIndex + 1);

        if (next)
            SetOnPath();
        else
        {
            if (!_path.Looping)
            {
                _path = null;
                reached.Reached(transform);
                OnPathEnded?.Invoke();
                return;
            }

            _segmentIndex = 0;
            MoveToPosition(_path.GetNode(0).transform.position);
        }

        reached.Reached(transform);
    }
    #endregion
}