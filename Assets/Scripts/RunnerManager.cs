using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RunnerManager : MonoBehaviour
{
    public int laps = 0;
    public float stoppingDistance = 2;

    public int RunnerID;

    public int CurrentTarget;

    public float CheckpointsCleared;

    public float distanceToTarget; //Used for placement calc

    public List<Transform> Checkpoints = new List<Transform>();

    public NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Checkpoints.Count > 0 && Checkpoints[0] != null && agent != null)
        {
            agent.SetDestination(Checkpoints[0].position);
            CurrentTarget = 0;
        }
        else
        {
            Debug.LogError(agent.name + " is having problems with checkpoints!");
        }

        CheckpointsCleared = (float)(CurrentTarget - 1) / (float)(Checkpoints.Count);
        distanceToTarget = agent.remainingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= stoppingDistance)
        {
            NewCheckpoint();
        }

        distanceToTarget = agent.remainingDistance;
    }

    private void NewCheckpoint()
    {
        if(Checkpoints.Count > 0)
        {
            if (CurrentTarget < Checkpoints.Count)
            {
                agent.SetDestination(Checkpoints[CurrentTarget].position);
                CheckpointsCleared = (float)CurrentTarget / (float)(Checkpoints.Count - 1);
                CurrentTarget++;
            }
            else
            {
                CheckpointsCleared = (float)CurrentTarget / (float)(Checkpoints.Count - 1);
                CurrentTarget = 0;
                agent.SetDestination(Checkpoints[CurrentTarget].position);
                laps++;
            }
        }
        else
        {
            Debug.LogError(agent.name + " has no checkpoints!");
        }
    }
}
