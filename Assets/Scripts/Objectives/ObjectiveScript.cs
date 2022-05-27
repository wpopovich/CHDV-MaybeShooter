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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetObjectiveText()
    {
        return objectiveText;
    }

    public Type GetObjectiveType()
    {
        return objectiveType;
    }
}
