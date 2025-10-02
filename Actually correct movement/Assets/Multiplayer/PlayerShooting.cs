using UnityEngine;
using Unity.Netcode;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float spawnOffset = 100f; // distance from player
    [SerializeField] private Camera playerCamera;

    public bool isheld;
    private void Awake()
    {
        if (!IsOwner) return; // Only the local player cares about its camera

        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
            Debug.LogError("PlayerCamera not found in children of player prefab!");
    }

    private void Update()
    {
        if (!IsOwner) return; // Only handle input for local player
        if (isheld == false)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                FireProjectile();
            }
        }
    }

    private void FireProjectile()
    {
        if (playerCamera == null || projectilePrefab == null) return;

        // Convert mouse position to world space using this player's camera
        Vector3 mouseWorldPos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // Send mouse world position to server
        ShootServerRpc(mouseWorldPos);
    }

    [ServerRpc]
    private void ShootServerRpc(Vector3 targetPosition, ServerRpcParams rpcParams = default)
    {
        if (projectilePrefab == null) return;

        // Compute direction from player to cursor
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Compute spawn position offset so projectile spawns outside the player
        Vector3 spawnPos = transform.position + direction * spawnOffset;

        // Instantiate and spawn projectile
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        projectile.transform.right = direction; // face the direction
        projectile.GetComponent<NetworkObject>().Spawn(true);
    }
}
