using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjective : MonoBehaviour
{
    public ObjectiveScript objective;
    private bool completed = false;

    public void CompleteObjective()
    {
        if (completed)
            return;

        switch (objective.GetObjectiveType()) {
            case ObjectiveScript.Type.Intel:
                Debug.Log("Intel has been retrieved");
                break;

        }

        completed = true;
    }

    public bool isCompleted()
    {
        return completed;
    }

    public string GetObjectiveDescription()
    {
        return objective.GetObjectiveText();
    }
}
