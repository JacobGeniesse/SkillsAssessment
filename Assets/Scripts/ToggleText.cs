using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class ToggleText : MonoBehaviour
{
    //Input master list
    public InputActionAsset MasterList;
    
    //Various Inputs
    private InputAction showName;
    private InputAction showTasks;


    //Text to toggle
    public GameObject nameText;
    public GameObject taskText;

    //Restart Vars

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        showName = MasterList["ShowName"];

        showTasks = MasterList["ShowTasks"];

    }

    // Update is called once per frame
    void Update()
    {
        if (showName.WasReleasedThisFrame())
        {
            nameText.SetActive(!nameText.activeSelf);
        }

        if (showTasks.WasReleasedThisFrame())
        {
            taskText.SetActive(!taskText.activeSelf);
        }

    }
}
