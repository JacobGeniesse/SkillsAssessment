using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ReloadRace : MonoBehaviour
{
    public InputActionAsset MasterList;

    private InputAction quitGame;

    private InputAction fourRace;
    private InputAction fiveRace;

    private string FourCount = "Race4Count";
    private string FiveCount = "Race5Count";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        quitGame = MasterList["Quit"];

        fourRace = MasterList["4Count"];

        fiveRace = MasterList["5Count"];
    }

    // Update is called once per frame
    void Update()
    {
        if (quitGame.WasReleasedThisFrame())
        {
            Debug.Log("Quitting Game");
            Application.Quit();
        }

        if (fourRace.WasReleasedThisFrame())
        {
            SceneManager.LoadScene(FourCount, LoadSceneMode.Single);
        }

        if (fiveRace.WasReleasedThisFrame())
        {
            SceneManager.LoadScene(FiveCount, LoadSceneMode.Single);
        }
    }
}
