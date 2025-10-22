using UnityEngine;
using Unity.Netcode;

public class PlayerPlacement : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;

    private GameObject previewObject;

    private Item lastSelectedItem;
    
    private Quaternion currentRotation = Quaternion.identity;



    private void Start()
    {
        if (!IsOwner)
        {
            enabled = false; 
            return;
        }

        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
    }

    
    private void Update()
    {
        if (!IsOwner) return;

        Item selectedItem = InventoryManager.instance.GetSelectedItem(false);

        if (selectedItem != lastSelectedItem)
        {
            lastSelectedItem = selectedItem;

            currentRotation = Quaternion.identity;

            if (previewObject != null)
            {
                Destroy(previewObject);
                previewObject = null;
            }

            if (selectedItem != null && selectedItem.previewObject != null)
            {
                previewObject = Instantiate(selectedItem.previewObject);
                previewObject.transform.rotation = currentRotation;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && previewObject != null)
        {
            currentRotation *= Quaternion.Euler(0, 0, 90); 
            previewObject.transform.rotation = currentRotation;
        }

        if (previewObject != null)
        {
            Vector3 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            previewObject.transform.position = mousePos;
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceItem();
        }
    }


    void ShowPreview(Item item)
    {

        if (previewObject != null)
            Destroy(previewObject);

        previewObject = Instantiate(item.placeablePrefab);
        
        Transform previewChild = previewObject.transform.Find("PreviewObject");
        if (previewChild != null)
        {
            previewChild.gameObject.SetActive(true);
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

        if (selectedItem.type == ItemType.BuildingBlock)
        {
            Vector3 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            Debug.Log($"Requesting to place item {selectedItem.itemName} at {mousePos}");

            PlaceItemServerRpc(selectedItem.itemName, mousePos);
        }
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

    GameObject obj = Instantiate(itemToPlace.placeablePrefab, position, currentRotation);
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
