using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RunnerManager : MonoBehaviour
{
    //Score tracking vars
    public int laps = 0;
    public int CurrentTarget;
    public float CheckpointsCleared = 0;
    public float distanceToTarget; //Used for placement calc

    [HideInInspector] public float backupDistance; //Used as a backup incase distance to target isn't finsihed calcing
    public int RunnerID;

    //Checkpoints to run through
    public List<GameObject> Checkpoints = new List<GameObject>();

    public float stoppingDistance = 2; //distance at which checkpoints can be cleared
    public NavMeshAgent agent; //navmesh agent ref

    private bool noDoubleDip = true; //Theoretically Prevents a script from running twice.

    private bool addLap = false; //Var for allowing a lap to be incremented when passing across the finish line

    void Start()
    {
        //Set the current target for the racer at the start with a debug message in case of error.
        if (Checkpoints.Count > 0 && Checkpoints[0] != null && agent != null)
        {
            CurrentTarget = 0;
            agent.SetDestination(Checkpoints[CurrentTarget].transform.position);
        }
        else
        {
            Debug.LogError(agent.name + " is having problems with checkpoints!");
        }

        //Set the number of checkpoints cleared to 0
        CheckpointsCleared = 0;

        //Establish distance to target for scoring purposes
        distanceToTarget = agent.remainingDistance;
        backupDistance = distanceToTarget;

    }

    // Update is called once per frame
    void Update()
    {
        //Check if the agent can advance to the next checkpoint
        if (agent.remainingDistance <= stoppingDistance && noDoubleDip == true)
        {
            noDoubleDip = false;
            NewCheckpoint();
        }
        
        //Run distance check for scoring purposes, if remaining distance can't be calc'd then use a backup distance
        if(agent.pathPending != true && agent.remainingDistance != float.PositiveInfinity)
        {
            distanceToTarget = agent.remainingDistance;
            backupDistance = distanceToTarget;
        }
        else
        {
            distanceToTarget = backupDistance;
        }
    }

    //Func to handle incrementing checkpoints
    private void NewCheckpoint()
    {
        if (Checkpoints.Count > 0)
        {
            //Adding to the lap counter
            if(addLap == true)
            {
                laps++;
            }
            //Setting distance to target
            distanceToTarget = 0;

            //If THere are more checkpoints to run through increment the counter to the next checkpoint
            if (CurrentTarget < Checkpoints.Count - 1)
            {
                CheckpointsCleared++;
                CurrentTarget++;
                agent.SetDestination(Checkpoints[CurrentTarget].transform.position);
                addLap = false;
            }
            else //If there are no more checkpoints to run through set the counters back to 0 and then let the lap counter increment once it reaches the starting checkpoint
            {
                addLap = true;
                CheckpointsCleared = 0;
                CurrentTarget = 0;
                agent.SetDestination(Checkpoints[CurrentTarget].transform.position);
            }

        }
        else //If there are no checkpoints to run through print an error with the name of the runner so that the problem can be fixed
        {
            Debug.LogError(agent.name + " has no checkpoints!");
        }
        noDoubleDip = true;
    }

}
