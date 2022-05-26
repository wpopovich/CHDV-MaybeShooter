using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private InventoryItem item;

    public InventoryItem LootInventory()
    {
        return item;
    } 

    public void SetInventory(InventoryItem item)
    {
        if (item != null)
            this.item = item;
    }
}
