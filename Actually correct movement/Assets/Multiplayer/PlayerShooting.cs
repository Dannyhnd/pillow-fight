using UnityEngine;
using Unity.Netcode;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float spawnOffset = 1f; // distance from player
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

        Vector3 direction = (targetPosition - transform.position).normalized;

        // Rotate the direction 90° in the XY plane to get a perpendicular vector
        Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0f).normalized;

        // Tune these as you like
        float forwardOffset = 1.0f;  // how far in front of player
        float sideOffset = 1.5f;     // how far to the side

        // Compute final spawn position
        Vector3 spawnPos = transform.position + direction * forwardOffset + perpendicular * sideOffset;

        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        projectile.transform.right = direction;
        projectile.GetComponent<NetworkObject>().Spawn(true);

    }
}
