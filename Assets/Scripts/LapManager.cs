using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LapManager : MonoBehaviour
{
    private List<GameObject> Runners = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Runners = FindGameObjectsWithTag("Runner").ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
