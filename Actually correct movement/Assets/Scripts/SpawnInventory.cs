using UnityEngine;
using Unity.Netcode;

public class SpawnInventory : NetworkBehaviour
{
    [SerializeField] private GameObject toolbarPrefab;
    [SerializeField] private Camera playerCamera;
    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();

        if (!IsOwner) return; // Only the local player should spawn the inventory

        // Spawn the inventory UI
        GameObject toolbarInstance = Instantiate(toolbarPrefab);
        toolbarInstance.SetActive(true);

        // Optionally, parent it to the canvas or set its position
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas != null)
        {
            toolbarInstance.transform.SetParent(canvas.transform, false);
        }

        // Set up the camera if needed
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            Debug.LogError("PlayerCamera not found in children of player prefab!");
        }
    }
}
