using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkStartUI : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;
    public Button serverButton;
    public GameObject gameui;

    void Start()
    {
        hostButton.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
        clientButton.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
        serverButton.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
    }

    void Update()
    {
        // Hide UI once connected
        if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer)
        {
            gameObject.SetActive(false);
        }
    }
}
