using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RunnerManager : MonoBehaviour
{
    public int laps = 0;
    public float stoppingDistance = 2;

    public int RunnerID;

    public int CurrentTarget;

    public float CheckpointsCleared = 0;

    public float distanceToTarget; //Used for placement calc

    public List<GameObject> Checkpoints = new List<GameObject>();

    public NavMeshAgent agent;

    private bool noDoubleDip = true; //Prevents a script from running twice.

    public float placementScore = 0;

    private bool addLap = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Checkpoints.Count > 0 && Checkpoints[0] != null && agent != null)
        {
            CurrentTarget = 0;
            agent.SetDestination(Checkpoints[CurrentTarget].transform.position);
        }
        else
        {
            Debug.LogError(agent.name + " is having problems with checkpoints!");
        }

        CheckpointsCleared = 0;


        distanceToTarget = agent.remainingDistance;

    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= stoppingDistance && noDoubleDip == true)
        {
            noDoubleDip = false;
            NewCheckpoint();
        }

        distanceToTarget = agent.remainingDistance;
    }

    private void NewCheckpoint()
    {
        if (Checkpoints.Count > 0)
        {
            if(addLap == true)
            {
                laps++;
            }

            distanceToTarget = 0;
            if (CurrentTarget < Checkpoints.Count - 1)
            {
                CheckpointsCleared++;
                CurrentTarget++;
                agent.SetDestination(Checkpoints[CurrentTarget].transform.position);
                addLap = false;
            }
            else
            {
                addLap = true;
                CheckpointsCleared = 0;
                CurrentTarget = 0;
                agent.SetDestination(Checkpoints[CurrentTarget].transform.position);
            }

        }
        else
        {
            Debug.LogError(agent.name + " has no checkpoints!");
        }
        noDoubleDip = true;
    }

}
