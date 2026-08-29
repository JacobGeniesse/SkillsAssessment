using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LapManager : MonoBehaviour
{
    private TextMeshProUGUI placementText;

    public List<GameObject> Runners = new List<GameObject>();
    public List<RunnerManager> RunningComponents = new List<RunnerManager>();

    public List<GameObject> CurrentPlacements = new List<GameObject>();
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
            CurrentPlacements.Add(Runners[i]);
            PlacementScores.Add(0);

            if(Runners[i].TryGetComponent<RunnerManager>(out RunnerManager holdingCell))
            {
                RunningComponents.Add(holdingCell);
            }
            else
            {
                Debug.LogWarning(Runners[i] + "was not counted for this race!");
            }
        }
        //PlacementUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < PlacementScores.Count; i++)
        {
            CalcScore(i);
            OrderScores();
            TextUpdate();
        }
    }

    private void CalcScore(int TargetRunner)
    {
        PlacementScores[TargetRunner] = 0;  
        for(int i =0; i < RunningComponents.Count; i++)
        {
            if (RunningComponents[TargetRunner].laps > RunningComponents[i].laps)
            {
                PlacementScores[TargetRunner] += 100f;
            }

            if (RunningComponents[TargetRunner].CheckpointsCleared > RunningComponents[i].CheckpointsCleared)
            {
                PlacementScores[TargetRunner] += 10f;
            }

            if (RunningComponents[TargetRunner].distanceToTarget > RunningComponents[i].distanceToTarget)
            {
                PlacementScores[TargetRunner] += 1f;
            }
        }
    }

    private void OrderScores()
    {
        CurrentPlacements.Clear();
        CurrentPlacements.Add(Runners[0]);
        for(int i = 1; i < PlacementScores.Count; i++)
        {
            for(int j = 1; j < Runners.Count; j++)
            {
                if (PlacementScores[i] > PlacementScores[j - 1])
                {
                    if(j - 1 < CurrentPlacements.Count - 1)
                    {
                        CurrentPlacements.Insert(j - 1, Runners[i]);
                    }
                    else
                    {
                        CurrentPlacements.Add(Runners[i]);
                    }
                    break;
                }
                if (j == PlacementScores.Count - 1)
                {
                    CurrentPlacements.Add(Runners[i]);
                }
            }
        }
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
