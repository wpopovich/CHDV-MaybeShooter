using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    private static ObjectiveManager instance;

    public List<InteractableObjective> objectives;
    //public WorldExit exit;

    private bool objectivesCompleted = false;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (objectivesCompleted == true)
            return;

        List<InteractableObjective> completedObjectives = objectives.FindAll(obj => obj.isCompleted());
        
        if (completedObjectives.Count == objectives.Count) {
            EnableExit();
            objectivesCompleted = true;
        }
    }

    private void EnableExit()
    {
        Debug.Log("The world exit has been enabled, you may proceed to the next lvl");
    }

    public static ObjectiveManager GetInstance()
    {
        if (instance == null) {
            Debug.LogError("No instance of ObjectiveManager was found!");
        }

        return instance;
    }
}
