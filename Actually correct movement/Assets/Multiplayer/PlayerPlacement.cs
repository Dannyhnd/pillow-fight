using UnityEngine;
using Unity.Netcode;

public class PlayerPlacement : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;

    private void Start()
    {
        if (!IsOwner)
        {
            enabled = false; // Only local player handles input
            return;
        }

        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceItem();
        }
    }

    private void TryPlaceItem()
    {
        Item selectedItem = InventoryManager.instance.GetSelectedItem(false);
        if (selectedItem == null)
        {
            Debug.Log("No placeable item selected.");
            return;
        }

        Vector3 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Debug.Log($"Requesting to place item {selectedItem.itemName} at {mousePos}");

        PlaceItemServerRpc(selectedItem.itemName, mousePos);
    }

[ServerRpc(RequireOwnership = false)]
private void PlaceItemServerRpc(string itemName, Vector3 position)
{
    Debug.Log($"Server placing item {itemName} at {position}");

    Item itemToPlace = FindItemByName(itemName);
    if (itemToPlace == null)
    {
        Debug.LogWarning($"Server could not find item {itemName}");
        return;
    }

    if (itemToPlace.placeablePrefab == null)
    {
        Debug.LogWarning($"Item {itemName} has no prefab assigned!");
        return;
    }

    GameObject obj = Instantiate(itemToPlace.placeablePrefab, position, Quaternion.identity);
    NetworkObject netObj = obj.GetComponent<NetworkObject>();

    if (netObj != null)
    {
        netObj.Spawn(true); 
        Debug.Log($"Spawned network object {obj.name}");
    }
    else
    {
        Debug.LogError("Placeable prefab is missing NetworkObject component!");
        Destroy(obj);
    }
}
    private Item FindItemByName(string name)
    {
        Item[] allItems = Resources.LoadAll<Item>("");
        foreach (var item in allItems)
        {
            if (item.itemName == name)
                return item;
        }
        return null;
    }
}
