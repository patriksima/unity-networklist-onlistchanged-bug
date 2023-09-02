using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkManager))]
public class NetworkCommandLine : MonoBehaviour
{
    private NetworkManager m_NetManager;

    private void Start()
    {
        m_NetManager = GetComponentInParent<NetworkManager>();

        if (Application.isEditor) return;

        var args = GetCommandlineArgs();

        if (!args.TryGetValue("-mode", out var mode)) return;

        switch (mode)
        {
            case "server":
                StartServer();
                break;
            case "host":
                StartHost();
                break;
            case "client":
                StartClient();
                break;
        }
    }

    public void StartServer()
    {
        m_NetManager.StartServer();
    }

    public void StartHost()
    {
        m_NetManager.StartHost();
    }

    public void StartClient()
    {
        m_NetManager.StartClient();
    }

    private Dictionary<string, string> GetCommandlineArgs()
    {
        var argDictionary = new Dictionary<string, string>();

        var args = System.Environment.GetCommandLineArgs();

        for (var i = 0; i < args.Length; ++i)
        {
            var arg = args[i].ToLower();
            if (!arg.StartsWith("-")) continue;
            var value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
            value = (value?.StartsWith("-") ?? false) ? null : value;

            argDictionary.Add(arg, value);
        }

        return argDictionary;
    }
}