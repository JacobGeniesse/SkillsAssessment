using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunnerManager : MonoBehaviour
{
    private int laps = 0;
    public float stoppingDistance = 2;

    private int CurrentTarget;
    public List<Transform> UnclearedCheckpoints = new List<Transform>();

    public NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (UnclearedCheckpoints[0] != null && agent != null)
        {
            agent.SetDestination(UnclearedCheckpoints[0].position);
            CurrentTarget = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= stoppingDistance)
        {
            NewCheckpoint();
        }
    }

    private void NewCheckpoint()
    {
        if (CurrentTarget < UnclearedCheckpoints.Count)
        {
                agent.SetDestination(UnclearedCheckpoints[CurrentTarget].position);
            CurrentTarget++;
        }
        else
        {
            CurrentTarget = 0;
            agent.SetDestination(UnclearedCheckpoints[CurrentTarget].position);
            laps++;
        }
    }
}
