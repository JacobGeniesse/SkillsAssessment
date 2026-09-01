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

    void Start()
    {
        //Assign input vars to their corressponding inputs
        showName = MasterList["ShowName"];

        showTasks = MasterList["ShowTasks"];

    }

    void Update()
    {
        //Input handling for showing my name
        if (showName.WasReleasedThisFrame())
        {
            nameText.SetActive(!nameText.activeSelf);
        }

        //Input handling for showing the tasks accomplished
        if (showTasks.WasReleasedThisFrame())
        {
            taskText.SetActive(!taskText.activeSelf);
        }

    }
}
