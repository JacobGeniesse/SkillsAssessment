using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ReloadRace : MonoBehaviour
{
    //Input list
    public InputActionAsset MasterList;

    private InputAction quitGame;

    private InputAction fourRace;
    private InputAction fiveRace;

    //Scene names
    private string FourCount = "Race4Count";
    private string FiveCount = "Race5Count";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set input vars to their corresponding inputs
        quitGame = MasterList["Quit"];

        fourRace = MasterList["4Count"];

        fiveRace = MasterList["5Count"];
    }

    // Update is called once per frame
    void Update()
    {
        //Code for quit button
        if (quitGame.WasReleasedThisFrame())
        {
            Debug.Log("Quitting Game");
            Application.Quit();
        }

        //Code for loading the four racer map
        if (fourRace.WasReleasedThisFrame())
        {
            SceneManager.LoadScene(FourCount, LoadSceneMode.Single);
        }

        //Code for loading the five racer map
        if (fiveRace.WasReleasedThisFrame())
        {
            SceneManager.LoadScene(FiveCount, LoadSceneMode.Single);
        }
    }
}
