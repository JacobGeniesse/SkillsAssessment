using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class LapDisplay : MonoBehaviour
{
    private TextMeshProUGUI lapCountText;

    private List<GameObject> Runners = new List<GameObject>();
    private List<RunnerManager> RunningComponents = new List<RunnerManager>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameObject.Find("Laps").TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI lapsText))
        {
            lapCountText = lapsText;
        }
        else
        {
            Debug.LogError("Failed to Find Laps Text!");
        }
        Runners = GameObject.FindGameObjectsWithTag("Runner").ToList();

        for (int i = 0; i < Runners.Count; i++)
        {
            if (Runners[i].TryGetComponent<RunnerManager>(out RunnerManager holdingCell))
            {
                RunningComponents.Add(holdingCell);
            }
            else
            {
                Debug.LogWarning(Runners[i] + "was not counted for this race!");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        TextAssembly();
    }

    private void TextAssembly()
    {
        string FinalProduct = "Laps Finished: \n";
        for (int i = 0; i < RunningComponents.Count; i++)
        {
            string additive =  RunningComponents[i].name + ": " + RunningComponents[i].laps + "\t";
            FinalProduct = string.Concat(FinalProduct, additive);
        }

        lapCountText.text = FinalProduct;
    }
}
