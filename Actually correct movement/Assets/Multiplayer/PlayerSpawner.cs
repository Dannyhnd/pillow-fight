using UnityEngine;
using Unity.Netcode;

public class PlayerSpawner : NetworkBehaviour
{
    public Transform[] spawnPoints; // assign in Inspector
    private int nextIndex = 0;

    public override void OnNetworkSpawn()
    {
        if (IsServer) // only server decides spawn locations
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (spawnPoints.Length == 0) return;

        // Pick next spawn point (round-robin)
        Transform spawnPoint = spawnPoints[nextIndex];
        nextIndex = (nextIndex + 1) % spawnPoints.Length;

        var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
        playerObject.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }
}
