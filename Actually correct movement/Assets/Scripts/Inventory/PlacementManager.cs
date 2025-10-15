using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [Header("References")]
    public InventoryManager inventoryManager;
    public Camera mainCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceItem();
        }
    }

    void TryPlaceItem()
    {
        Item selectedItem = InventoryManager.instance.GetSelectedItem(false);

        if (selectedItem == null)
        {
            Debug.Log("No item selected.");
            return;
        }

        if (selectedItem.placeablePrefab  == null)
        {
            Debug.LogWarning($"Item '{selectedItem.itemName}' has no prefab assigned!");
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 placePosition = hit.point;
            placePosition = new Vector3(
                Mathf.Round(placePosition.x),
                Mathf.Round(placePosition.y),
                Mathf.Round(placePosition.z)
            );

            Instantiate(selectedItem.placeablePrefab, placePosition, Quaternion.identity);
            Debug.Log($"Placed {selectedItem.itemName} at {placePosition}");;
        }
        else
        {
            Debug.Log("No valid placement surface detected under mouse.");
        }
    }
}