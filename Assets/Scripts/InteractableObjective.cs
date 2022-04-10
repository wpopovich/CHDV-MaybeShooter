using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjective : MonoBehaviour
{
    public enum InteractableObjectiveType
    {
        Intel
    }

    public InteractableObjectiveType objectiveType;
    
    private bool completed = false;

    // Start is called before the first frame update
    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompleteObjective()
    {
        if (completed)
            return;

        switch (objectiveType) {
            case InteractableObjectiveType.Intel:
                Debug.Log("Intel has been retrieved");
                break;

        }

        completed = true;
    }

    public bool isCompleted()
    {
        return completed;
    }
}
