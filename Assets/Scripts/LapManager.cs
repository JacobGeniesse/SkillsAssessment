using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LapManager : MonoBehaviour
{
    private bool noDoubleDip = true; // Theoretically prevents a script from running twice.

    private TextMeshProUGUI placementText; //Text file for showing racer's placements

    public List<GameObject> Runners = new List<GameObject>(); //List of runner game objects
    public List<RunnerManager> RunningComponents = new List<RunnerManager>(); //List of the scripts that makes the racers run

    public List<RunnerManager> CurrentPlacements = new List<RunnerManager>(); //List of placements to be displayed in text
    public List<float> PlacementScores = new List<float>(); //List of the racer's placement scores that determine placings

    void Start()
    {
        //Assign the lapsText var
        if(GameObject.Find("Placements").TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI lapsText))
        {
            placementText = lapsText;
        }
        else
        {
            Debug.LogError("Failed to Find Laps Text!");
        }
        //Add the runner game objects to the list
        Runners = GameObject.FindGameObjectsWithTag("Runner").ToList();

        for(int i = 0; i < Runners.Count; i++)
        {
            PlacementScores.Add(0); //Assign a score to each runner
            if (Runners[i].TryGetComponent<RunnerManager>(out RunnerManager holdingCell))
            {
                RunningComponents.Add(holdingCell); //Add their runnign components to a list
            }
            else
            {
                Debug.LogWarning(Runners[i] + "was not counted for this race!"); //print an error message if that's impossible
            }
        }
        for (int i = 0; i < RunningComponents.Count; i++)
        {
            RunningComponents[i].RunnerID = i; //Set runner IDs so that placement scores don't ever get jumbled
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate placement scores, order those scores in a list, and then update the text
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

    //Func for calculating a runner's placement score
    private void CalcScore(int TargetRunner)
    {
        float tempScore = 0; //temporary placement score

        //Add score per lap
        tempScore += 10f * RunningComponents[TargetRunner].Checkpoints.Count * RunningComponents[TargetRunner].laps;

        //Add to score for checkpoints cleared
        tempScore += 10f * RunningComponents[TargetRunner].CheckpointsCleared;

        //Determine a distance score by comparing to the other racers
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
        //Set the placement score equal to tempScore
        PlacementScores[TargetRunner] = tempScore;
    }

    //Func for ordering racers based on their PlacementScores
    private void OrderScores()
    {
        //temp var for storing the ordered list
        List<RunnerManager> orderingList = new List<RunnerManager>();
        //Add a base racer to compare against
        orderingList.Add(RunningComponents[0]);

        /*For loops that go through the scores current in orderingList
         * until it finds a score that it is greater than. If it finds a score that it is greater than
         * it inserts itself into that spot, if not it is added to the end of the list
         */
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

        //Set the CurrentPlacements list equal to the orderingList
        CurrentPlacements = orderingList;
    }

    //Func for handling updating the text
    private void TextUpdate()
    {
        //string for holding the final list
        string FinalProduct = "";

        //for each name in CurrentPlacements concat the name in CurrentPlacements in a  format to the final product
        for (int j = 0; j < CurrentPlacements.Count; j++)
        {
            string additive = (j + 1) + ": " + CurrentPlacements[j].name + "\n";
            FinalProduct = string.Concat(FinalProduct, additive);
        }

        //Set the text object equal to the final product
        placementText.text = FinalProduct;
        noDoubleDip = true;
    }

    //Unused Scripts, I had to iterate through these to get to a solution that worked
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
