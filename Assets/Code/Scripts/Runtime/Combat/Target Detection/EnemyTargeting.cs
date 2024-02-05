using Euphrates;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class EnemyTargeting : MonoBehaviour
{
    Transform _transform;

    [SerializeField] TransformHolderSO _targets;

    [SerializeField] TriggerChannelSO _startShoot;
    [SerializeField] TriggerChannelSO _stopShoot;

    [SerializeField] float _selectionTreshold;

    bool _wasShooting = false;
    bool _shoot = false;
    // NativeBool _nativeShoot = new NativeBool(false, Allocator.Persistent);

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        _shoot = false;

        if (_targets.TransformCount == 0)
        {
            if (_wasShooting)
                _stopShoot?.Invoke();

            _wasShooting = false;
            return;
        }

        NativeArray<float3> positions = new NativeArray<float3>(_targets.TransformCount, Allocator.TempJob);
        NativeArray<bool> inRange = new NativeArray<bool>(_targets.TransformCount, Allocator.TempJob);

        PopulateArray(positions);

        var job = new SelectionJob()
        {
            OriginPosition = _transform.position,
            OriginHeading = _transform.forward,

            Positions = positions,
            SelectionTreshold = _selectionTreshold,

            InRange = inRange
            //Shoot = _nativeShoot
        };

        job.Schedule(positions.Length, System.Environment.ProcessorCount).Complete();

        _shoot = Shoot(inRange);

        if (!_wasShooting && _shoot)
            _startShoot?.Invoke();

        else if (_wasShooting && !_shoot)
            _stopShoot?.Invoke();

        _wasShooting = _shoot;

        positions.Dispose();
        inRange.Dispose();
    }

    void PopulateArray(NativeArray<float3> positions)
    {
        for (int i = 0; i < _targets.TransformCount; i++)
            positions[i] = _targets.GetTransform(i).position;
    }

    bool Shoot(NativeArray<bool> inRange)
    {
        for (int i = 0; i < inRange.Length; i++)
            if (inRange[i])
                return true;

        return false;
    }

    [BurstCompile]
    struct SelectionJob : IJobParallelFor
    {
        [ReadOnly] public float3 OriginPosition;
        [ReadOnly] public float3 OriginHeading;

        [ReadOnly] public NativeArray<float3> Positions;
        [ReadOnly] public float SelectionTreshold;

        [WriteOnly] public NativeArray<bool> InRange; 
        // public NativeBool Shoot;

        public void Execute(int index)
        {
            float3 tpos = Positions[index];

            float3 dir = tpos - OriginPosition;
            dir = math.normalize(dir);

            float dot = math.dot(OriginHeading, dir);

            if (dot >= SelectionTreshold)
            {
                InRange[index] = true;
                //Shoot.Value = true;
            }
        }
    }

//    [NativeContainer]
//    [NativeContainerIsAtomicWriteOnly]
//    unsafe struct NativeBool : IDisposable
//    {
//        [NativeDisableUnsafePtrRestriction]
//        bool* _value;

//#if ENABLE_UNITY_COLLECTIONS_CHECKS
//        internal AtomicSafetyHandle m_Safety;
//        // The dispose sentinel tracks memory leaks. It is a managed type so it is cleared to null when scheduling a job
//        // The job cannot dispose the container, and no one else can dispose it until the job has run, so it is ok to not pass it along
//        // This attribute is required, without it this NativeContainer cannot be passed to a job; since that would give the job access to a managed object
//        [NativeSetClassTypeToNullOnSchedule]
//        internal DisposeSentinel m_DisposeSentinel;
//#endif

//        // Keep track of where the memory for this was allocated
//        Allocator _allocatorLabel;

//        public NativeBool(bool value, Allocator label)
//        {
//            // This check is redundant since we always use an int that is blittable.
//            // It is here as an example of how to check for type correctness for generic types.
//#if ENABLE_UNITY_COLLECTIONS_CHECKS
//            if (!UnsafeUtility.IsBlittable<int>())
//                throw new ArgumentException(string.Format("{0} used in NativeQueue<{0}> must be blittable", typeof(int)));
//#endif
//            _allocatorLabel = label;

//            // Allocate native memory for a single integer
//            _value = (bool*)UnsafeUtility.Malloc(UnsafeUtility.SizeOf<bool>(), 4, label);

//            // Create a dispose sentinel to track memory leaks. This also creates the AtomicSafetyHandle
//#if ENABLE_UNITY_COLLECTIONS_CHECKS
//            DisposeSentinel.Create(out m_Safety, out m_DisposeSentinel, 0, label);
//#endif
//            // Initialize the count to 0 to avoid uninitialized data
//            Value = false;
//        }

//        public bool Value
//        {
//            get
//            {
//                // Verify that the caller has read permission on this data. 
//                // This is the race condition protection, without these checks the AtomicSafetyHandle is useless
//#if ENABLE_UNITY_COLLECTIONS_CHECKS
//                AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
//#endif
//                return *_value;
//            }
//            set
//            {
//                // Verify that the caller has write permission on this data. This is the race condition protection, without these checks the AtomicSafetyHandle is useless
//#if ENABLE_UNITY_COLLECTIONS_CHECKS
//                AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
//#endif
//                *_value = value;
//            }
//        }

//        public bool IsCreated
//        {
//            get { return _value != null; }
//        }

//        public void Dispose()
//        {
//            // Let the dispose sentinel know that the data has been freed so it does not report any memory leaks
//#if ENABLE_UNITY_COLLECTIONS_CHECKS
//            DisposeSentinel.Dispose(ref m_Safety, ref m_DisposeSentinel);
//#endif

//            UnsafeUtility.Free(_value, _allocatorLabel);
//            _value = null;
//        }
//    }
}
