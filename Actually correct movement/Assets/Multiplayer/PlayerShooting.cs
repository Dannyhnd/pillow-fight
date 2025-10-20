using UnityEngine;
using Unity.Netcode;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float spawnOffset = 1.0f; // distance from player
    [SerializeField] private Camera playerCamera;

    public bool isheld;

    private void Awake()
    {
        if (!IsOwner) return; // Only the local player needs the camera

        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
            Debug.LogError("PlayerCamera not found in children of player prefab!");
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (!isheld && Input.GetButtonDown("Fire1"))
        {
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        if (playerCamera == null || projectilePrefab == null) return;

        // Convert mouse position to world space
        Vector3 mouseWorldPos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // Send request to server to spawn projectile
        ShootServerRpc(mouseWorldPos);
    }

    [ServerRpc]
    private void ShootServerRpc(Vector3 targetPosition, ServerRpcParams rpcParams = default)
    {
        if (projectilePrefab == null) return;

        Vector3 direction = (targetPosition - transform.position).normalized;

        // Optional: compute perpendicular offset
        Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0f).normalized;
        float sideOffset = 1.5f;
        float forwardOffset = spawnOffset;

        Vector3 spawnPos = transform.position + direction * forwardOffset + perpendicular * sideOffset;

        // Instantiate and spawn the projectile on the server
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        projectile.transform.right = direction;
        NetworkObject netObj = projectile.GetComponent<NetworkObject>();

        if (netObj != null)
        {
            netObj.Spawn(true); // Spawn for all clients
        }
        else
        {
            Debug.LogError("Projectile prefab is missing NetworkObject component!");
            Destroy(projectile);
        }
    }
}
