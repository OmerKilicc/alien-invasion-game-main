using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class LaserMoveHandler : MonoBehaviour
{
    [SerializeField] TransformHolderSO _laserHolder;
    [SerializeField, Tooltip("This value determines after how many lasers system should switch to job system.")] int _jobsTreshold = 10;

    [Space]
    [SerializeField] float _laserSpeed;

    void Update()
    {
        int cnt = _laserHolder.TransformCount;

        if (cnt == 0)
            return;

        if (cnt < _jobsTreshold)
        {
            MoveLasers();
            return;
        }

        MoveLasersJobs();
    }

    void MoveLasers()
    {
        for (int i = 0; i < _laserHolder.TransformCount; i++)
        {
            Transform laser = _laserHolder.GetTransform(i);
            laser.position += _laserSpeed * Time.deltaTime * laser.forward;
        }
    }

    void MoveLasersJobs()
    {
        int cnt = _laserHolder.TransformCount;

        NativeArray<float3> positions = new NativeArray<float3>(cnt, Allocator.TempJob);
        NativeArray<float3> headings = new NativeArray<float3>(cnt, Allocator.TempJob);

        for (int i = 0; i < _laserHolder.TransformCount; i++)
        {
            positions[i] = _laserHolder.GetTransform(i).position;
            headings[i] = _laserHolder.GetTransform(i).forward;
        }

        var job = new MoveLaserJob()
        {
            Positions = positions,
            Headings = headings,

            Speed = _laserSpeed,
            DeltaTime = Time.deltaTime
        };

        int processors = Environment.ProcessorCount;

        JobHandle handle = job.Schedule(cnt, processors);
        handle.Complete();

        for (int i = 0; i < _laserHolder.TransformCount; i++)
        {
            _laserHolder.GetTransform(i).position = positions[i];
        }

        positions.Dispose();
        headings.Dispose();
    }

    [BurstCompile]
    public struct MoveLaserJob : IJobParallelFor
    {
        public NativeArray<float3> Positions;
        [ReadOnly] public NativeArray<float3> Headings;

        public float Speed;
        public float DeltaTime;

        public void Execute(int index)
        {
            Positions[index] += Speed * DeltaTime * Headings[index];
        }
    }
}
