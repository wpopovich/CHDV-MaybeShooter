using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Objective")]
public class ObjectiveScript : ScriptableObject
{
    public enum Type
    {
        Intel,
        Hackable,
        Lootable
    }
    public Type objectiveType;
    public string objectiveText;
    public string completedText;
    public InventoryItem.ItemType pickupType;
    public AudioClip audioClip;

    public string GetObjectiveText()
    {
        return objectiveText;
    }

    public Type GetObjectiveType()
    {
        return objectiveType;
    }
}
