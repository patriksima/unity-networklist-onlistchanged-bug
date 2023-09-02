using Unity.Netcode;
using UnityEngine;

public class NetworkListTest : NetworkBehaviour
{
    public NetworkList<int> IntList;

    private void Awake()
    {
        IntList = new NetworkList<int>(new[] { 1, 2, 3, 4, 5 });
        NetworkLog.LogInfoServer("Awake");
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsClient)
        {
            IntList.OnListChanged += IntListChanged;
        }

        NetworkLog.LogInfoServer("OnNetworkSpawn");
    }

    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            IntList.OnListChanged -= IntListChanged;
        }

        base.OnNetworkDespawn();
        NetworkLog.LogInfoServer("OnNetworkDespawn");
    }

    public override void OnDestroy()
    {
        IntList.ResetDirty();
        base.OnDestroy();
        Debug.Log("OnDestroy");
    }

    private void IntListChanged(NetworkListEvent<int> changeEvent)
    {
        if (IsClient)
            Debug.Log("IntListChanged on client");
        NetworkLog.LogInfoServer(
            $"IntListChanged: IntList has changed by event '{changeEvent.Type}' with value '{changeEvent.Value}'");
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeListDataServerRpc()
    {
        IntList[0] = 0;
        IntList.Add(10);
        IntList.Remove(1);
        IntList.SetDirty(true);

        NetworkLog.LogInfoServer("ChangeListDataServerRpc");

        CallAllClientsClientRpc();
    }

    [ClientRpc]
    public void CallAllClientsClientRpc()
    {
        NetworkLog.LogInfoServer("CallAllClientsClientRpc");
    }

    public void OnChangeListData()
    {
        ChangeListDataServerRpc();
    }
}