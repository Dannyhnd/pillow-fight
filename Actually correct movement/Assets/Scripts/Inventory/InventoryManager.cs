using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int selectedSlotIndex = 0;
    public static InventoryManager instance;
    public Item[] startItems;
    int selectedSlot = -1;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // ✅ Make sure inventorySlots are assigned in the Inspector
        if (inventorySlots == null || inventorySlots.Length == 0)
        {
            Debug.LogError("InventoryManager: No inventory slots assigned in the Inspector!");
            return;
        }

        // ✅ Initialize all slots to deselected
        foreach (var slot in inventorySlots)
        {
            if (slot != null)
                slot.Deselect();
            else
                Debug.LogError("InventoryManager: One or more slots are missing in the Inspector!");
        }

        // ✅ Select the first slot
        ChangeSelectedSlot(0);
        selectedSlotIndex = 0;

        // ✅ Add any starting items
        foreach (var item in startItems)
        {
            AddItem(item);
        }
    }

    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 5)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
            inventorySlots[selectedSlot].Deselect();

        inventorySlots[newValue].Select();
        selectedSlot = newValue;        
        selectedSlotIndex = newValue; // ✅ Add this line

    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return;
            }
        }
    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        if (inventoryItem != null)
        {
            inventoryItem.InitialiseItem(item);
            slot.item = item; 
        }
    }


    public void SelectSlot(int index)
    {
        selectedSlotIndex = index;
    }

    public Item GetSelectedItem(bool use)
    {
        Debug.Log($"Getting item from slot {selectedSlotIndex}");

        if (inventorySlots[selectedSlotIndex] == null)
        {
            Debug.LogError($"Slot {selectedSlotIndex} is null!");
            return null;
        }

        Item item = inventorySlots[selectedSlotIndex].item;

        if (item == null)
        {
            Debug.LogWarning($"No item in slot {selectedSlotIndex}");
            return null;
        }

        Debug.Log($"Selected item: {item.itemName}");

        if (use)
        {
            inventorySlots[selectedSlotIndex].item = null;
        }

        return item;
    }

}