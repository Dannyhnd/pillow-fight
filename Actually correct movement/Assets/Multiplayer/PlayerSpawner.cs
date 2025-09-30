using UnityEngine;
using Unity.Netcode;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    private int nextIndex = 0;

    void Awake()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request,
                               NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = true;
        response.CreatePlayerObject = true;

        // Assign spawn point
        Transform spawnPoint = spawnPoints[nextIndex];
        nextIndex = (nextIndex + 1) % spawnPoints.Length;

        response.Position = spawnPoint.position;
        response.Rotation = spawnPoint.rotation;
    }
}
