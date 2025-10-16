using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.UI;
using System.Net;
using TMPro;

public class NetworkStartUI : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;
    public TMP_InputField ipInputField; // For clients to enter IP
    public TMP_Text hostIpDisplay;      // For hosts to show their IP
    public GameObject gameui;

    void Start()
    {
        hostButton.onClick.AddListener(StartAsHost);
        clientButton.onClick.AddListener(StartAsClient);
    }

    void StartAsHost()
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = "0.0.0.0"; // Listen on all interfaces
        transport.ConnectionData.Port = 7777;

        NetworkManager.Singleton.StartHost();

        // Display local IP for clients to connect
        string localIP = GetLocalIPAddress();
        hostIpDisplay.text = "Host IP: " + localIP + ":7777";
    }

    void StartAsClient()
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        // Get IP from InputField
        string ip = ipInputField.text;
        if (string.IsNullOrEmpty(ip))
        {
            Debug.LogWarning("No IP entered!");
            return;
        }

        transport.ConnectionData.Address = ip;
        transport.ConnectionData.Port = 7777;

        NetworkManager.Singleton.StartClient();
    }

    void Update()
    {
        // Hide menu once connected
        if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer)
        {
            gameObject.SetActive(false);
            gameui.SetActive(true);
        }
    }

    // Get your local IPv4 address for LAN play
    private string GetLocalIPAddress()
    {
        string localIP = "127.0.0.1";
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
}
