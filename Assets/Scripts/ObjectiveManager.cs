using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    private static ObjectiveManager instance;

    public List<InteractableObjective> objectives;
    public WorldExit worldExit;
    public GameObject nextObjectiveIndicator;
    //public WorldExit exit;

    private bool objectivesCompleted = false;
    private GameObject currentObjective;
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
        if (!objectivesCompleted) {
            InteractableObjective nextObjective = objectives.Find(o => !o.isCompleted());
            if (nextObjective != null && nextObjective != currentObjective) {
                SetCurrentObjective(nextObjective.gameObject);
            }

            List<InteractableObjective> completedObjectives = objectives.FindAll(obj => obj.isCompleted());
            if (completedObjectives.Count == objectives.Count) {
                EnableExit();
                SetCurrentObjective(worldExit.gameObject);
                objectivesCompleted = true;
            }
        }
    }

    private void SetCurrentObjective(GameObject newObjective)
    {
        currentObjective = newObjective;
        Vector3 newPosition = new Vector3(newObjective.transform.position.x, newObjective.transform.position.y + 2, newObjective.transform.position.z);
        nextObjectiveIndicator.transform.position = newPosition;
    }

    private void EnableExit()
    {
        Debug.Log("The world exit has been enabled, you may proceed to the next lvl");
        worldExit.OpenDoors();
    }

    public static ObjectiveManager GetInstance()
    {
        if (instance == null) {
            Debug.LogError("No instance of ObjectiveManager was found!");
        }

        return instance;
    }
}
