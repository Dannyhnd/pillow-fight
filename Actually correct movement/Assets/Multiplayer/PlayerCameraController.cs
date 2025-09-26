using UnityEngine;
using Unity.Netcode;

public class PlayerCameraController : NetworkBehaviour
{
    private Camera playerCamera;

    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>(true);

        if (playerCamera != null)
        {
            // Ensure all player cameras are disabled until ownership is resolved
            playerCamera.gameObject.SetActive(false);
        }
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log($"[PlayerCameraController] OnNetworkSpawn. Owner: {IsOwner}");

        if (IsOwner && playerCamera != null)
        {
            // Disable the default scene camera if it exists
            if (Camera.main != null && Camera.main != playerCamera)
            {
                Camera.main.gameObject.SetActive(false);
            }

            // Activate this player's camera
            playerCamera.gameObject.SetActive(true);
        }
    }
}
