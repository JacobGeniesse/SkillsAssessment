using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class ToggleText : MonoBehaviour
{
    public InputActionAsset MasterList;
    private InputAction showName;
    private InputAction showTasks;

    public GameObject nameText;
    public GameObject taskText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        showName = MasterList["ShowName"];
        //nameText = GameObject.Find("Name");


        showTasks = MasterList["ShowTasks"];
        //taskText = GameObject.Find("Tasks");
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
