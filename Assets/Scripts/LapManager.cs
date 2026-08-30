using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LapManager : MonoBehaviour
{
    private bool noDoubleDip = true; //Prevents a script from running twice.

    private TextMeshProUGUI placementText;

    public List<GameObject> Runners = new List<GameObject>();
    public List<RunnerManager> RunningComponents = new List<RunnerManager>();

    public List<RunnerManager> CurrentPlacements = new List<RunnerManager>();
    public List<float> PlacementScores = new List<float>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GameObject.Find("Placements").TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI lapsText))
        {
            placementText = lapsText;
        }
        else
        {
            Debug.LogError("Failed to Find Laps Text!");
        }
        Runners = GameObject.FindGameObjectsWithTag("Runner").ToList();
        for(int i = 0; i < Runners.Count; i++)
        {
            PlacementScores.Add(0);
            if (Runners[i].TryGetComponent<RunnerManager>(out RunnerManager holdingCell))
            {
                RunningComponents.Add(holdingCell);
            }
            else
            {
                Debug.LogWarning(Runners[i] + "was not counted for this race!");
            }
        }
        for (int i = 0; i < RunningComponents.Count; i++)
        {
            RunningComponents[i].RunnerID = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(noDoubleDip == true)
        {
            noDoubleDip = false;
            for (int i = 0; i < PlacementScores.Count; i++)
            {
                CalcScore(i);
                OrderScores();
                TextUpdate();
            }
        }
    }

    private void CalcScore(int TargetRunner)
    {
            float tempScore = 0;
            tempScore += 10f * RunningComponents[TargetRunner].Checkpoints.Count * RunningComponents[TargetRunner].laps;

            tempScore += 10f * RunningComponents[TargetRunner].CheckpointsCleared;

            for(int j = 0; j < PlacementScores.Count; j++)
            {
                if (RunningComponents[TargetRunner].distanceToTarget < RunningComponents[j].distanceToTarget && RunningComponents[TargetRunner].distanceToTarget != float.PositiveInfinity)
                {
                    tempScore += 1f;
                }
                else
                {
                    tempScore -= 1f;
                }
            }
            PlacementScores[TargetRunner] = tempScore;
    }

    private void OrderScores()
    {
        List<RunnerManager> orderingList = new List<RunnerManager>();
        orderingList.Add(RunningComponents[0]);
        for (int i = 1; i < PlacementScores.Count; i++)
        {
            for (int j = 0; j < orderingList.Count || j < RunningComponents.Count; j++)
            {
                if (PlacementScores[i] > PlacementScores[orderingList[j].RunnerID])
                {
                    //Debug.Log(Runners[i].name + PlacementScores[i] + " Vs " + Runners[orderingList[j].RunnerID].name + PlacementScores[orderingList[j].RunnerID]);
                    orderingList.Insert(j, RunningComponents[i]);
                    break;
                }
                if(j == orderingList.Count - 1)
                {
                    orderingList.Add(RunningComponents[i]);
                    break;
                }
            }
        }
        //Debug.Log(orderingList[0] + ", " + orderingList[1] + ", " + orderingList[2] + ", " + orderingList[3]);

        CurrentPlacements = orderingList;
    }

    private void TextUpdate()
    {
        string FinalProduct = "";
        for (int j = 0; j < CurrentPlacements.Count; j++)
        {
            string additive = (j + 1) + ": " + CurrentPlacements[j].name + "\n";
            FinalProduct = string.Concat(FinalProduct, additive);
        }

        placementText.text = FinalProduct;
        noDoubleDip = true;
    }

    //Unused Scripts, I had to iterate through this to get to a solution that worked
    /*private void PlacementUpdate()
    {
        for (int i = 1; i < RunningComponents.Count; i++)
        {
            for (int j = 0; j < RunningComponents.Count; j++)
            {
                LapsCheck(i, j);
            }
        }
    }

    private void LapsCheck(int Base, int Comparison)
    {
        if (RunningComponents[Base].laps > RunningComponents[Comparison].laps)
        {
            Debug.Log("!!!");
            ShiftPositions(Base, Comparison);
            TextUpdate();
        }
        else
        {
            MidLapCheck(Base, Comparison);
        }
    }

    private void MidLapCheck(int Base, int Comparison)
    {
        if (RunningComponents[Base].CurrentTarget > RunningComponents[Comparison].CurrentTarget)
        {
            ShiftPositions(Base, Comparison);
            TextUpdate();
        }
        else
        {
            DistanceCheck(Base, Comparison);
        }
    }

    private void DistanceCheck(int Base, int Comparison)
    {
        if (RunningComponents[Base].distanceToTarget < RunningComponents[Comparison].distanceToTarget)
        {
            ShiftPositions(Base, Comparison);
            TextUpdate();
        }
        else if (RunningComponents[Base].distanceToTarget == RunningComponents[Comparison].distanceToTarget)
        {
            Equalized(Base);
        }
    }

    private void Equalized(int CommonPlacing)
    {
        string FinalProduct = "";
        for (int j = 0; j < CurrentPlacements.Count; j++)
        {
            string additive = (CommonPlacing) + ": " + CurrentPlacements[j].name + "\n";
            FinalProduct = string.Concat(FinalProduct, additive);
        }

        placementText.text = FinalProduct;
    }

    private void ShiftPositions(int insertVal, int startVal)
    {
        GameObject movedVar = null;
        GameObject holdingVar = null;
        List<GameObject> updatedList = new List<GameObject>();

        for (int j = 0; j < startVal; j++)
        {
            updatedList.Add(CurrentPlacements[j]);
        }

        for (int i = startVal; i < updatedList.Count - 1; i++)
        {
            movedVar = updatedList[i];
            if (i == startVal)
            {
                updatedList[i] = RunningComponents[insertVal].GameObject();
            }
            else
            {
                updatedList[i] = holdingVar;
            }
            holdingVar = updatedList[i + 1];
            updatedList[i + 1] = movedVar;
        }

        for (int j = 0; j < updatedList.Count; j++)
        {
            Debug.Log(updatedList[j]);
        }
    }*/


}
