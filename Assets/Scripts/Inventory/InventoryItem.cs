using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class InventoryItem : ScriptableObject
{
    [SerializeField]
    private ItemType itemType;

    public enum ItemType {
        KeyCard
    }

    public ItemType getItemType()
    {
        return itemType;
    }
}
