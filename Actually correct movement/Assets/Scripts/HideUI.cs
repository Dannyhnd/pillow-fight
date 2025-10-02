using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class HideUI : MonoBehaviour
{
    void Start()
    {
        // Start with UI hidden
        gameObject.SetActive(false);
        
        // Subscribe to network events if NetworkManager exists
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        // Show UI when any client connects (including the local client)
        gameObject.SetActive(true);
    }

    private void OnServerStarted()
    {
        // Show UI when server starts
        gameObject.SetActive(true);
    }

    private void OnClientDisconnected(ulong clientId)
    {
        // Hide UI when client disconnects
        if (NetworkManager.Singleton != null && !NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            gameObject.SetActive(false);
        }
    }

    // Fallback Update method in case events don't work as expected
    void Update()
    {
        // Show UI when connected to network
        if (NetworkManager.Singleton != null && (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer))
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
