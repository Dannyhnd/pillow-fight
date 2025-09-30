using UnityEngine;
using Unity.Netcode;

public class PlayerShooting : NetworkBehaviour
{
    public GameObject ProjectilePrefab;
    public Transform LaunchOffset;

    void Update()
    {
        if (!IsOwner) return; // only local player handles input

        if (Input.GetButtonDown("Fire1"))
        {
            ShootServerRpc();
        }
    }

    [ServerRpc]
    private void ShootServerRpc(ServerRpcParams rpcParams = default)
    {
        GameObject projectile = Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
        projectile.GetComponent<NetworkObject>().Spawn(true);
    }
}
