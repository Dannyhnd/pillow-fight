using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoInventory : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public void GetSelectedItem()
    {
        Item receivedItem = inventoryManager.GetSelectedItem(false);
        if (receivedItem != null)
        {
            Debug.Log("Received item: " + receivedItem.itemName);
        }
        else
        {
            Debug.Log("No item received!");
        }
    }

    public void UseSelectedItem()
    {
        Item receivedItem = inventoryManager.GetSelectedItem(true);
        if (receivedItem != null)
        {
            Debug.Log("Used item: " + receivedItem.itemName);
        }
        else
        {
            Debug.Log("No item used!");
        }
    }

}
