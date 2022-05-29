using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    public InventoryItem item;
    public static Inventory instance;

    public InventoryItem LootInventory()
    {
        return item;
    }

    public static Inventory GetInstace()
    {
        return instance;
    }
    
    public void SetInventory(InventoryItem item)
    {
        if (item != null)
            this.item = item;
    }
}
