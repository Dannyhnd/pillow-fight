using UnityEngine;
using Unity.Netcode;

public class PlayerPlacement : NetworkBehaviour
{
    public GameObject placablePrefab;
    [SerializeField] private Camera playerCamera;
    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();

        if (!IsOwner)
        {
            enabled = false; // only local player handles input
            return;
        }

        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();

        if (inventoryManager == null)
            inventoryManager = FindObjectOfType<InventoryManager>(); // find your UI inventory in scene
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

        GameObject placeablePrefab = selectedItem.placeablePrefab; // <- you'll need to add this field to Item
        if (placeablePrefab != null)
        {
            Instantiate(placeablePrefab, mousePos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"{selectedItem.itemName} has no prefab assigned to place.");
        }
    }


    [ServerRpc]
    private void PlaceItemServerRpc(string itemName, Vector3 position, ServerRpcParams rpcParams = default)
    {
        Item itemToPlace = FindItemByName(itemName);
        if (itemToPlace == null || itemToPlace.placeablePrefab  == null)
        {
            Debug.LogWarning($"Item {itemName} not found on server!");
            return;
        }

        GameObject obj = Instantiate(itemToPlace.placeablePrefab , position, Quaternion.identity);
        obj.GetComponent<NetworkObject>().Spawn(true);
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
