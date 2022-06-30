using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using NetworkEvent = Unity.Networking.Transport.NetworkEvent;
using Unity.Services.Relay.Http;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using System.Linq;

public class RelayManager : MonoBehaviour
{
   

    [SerializeField]
    const int m_MaxConnections = 2;
    public string RelayJoinCode;

    public bool IsRelayEnabled => Transport != null && Transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;

    public UnityTransport Transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();


    public struct RelayHostData
    {
        public string JoinCode;
        public string IPv4Address;
        public ushort Port;
        //public Guid AllocationID;
        public byte[] AllocationIDBytes;
        public byte[] ConnectionData;
        public byte[] Key;
    }

    public struct RelayJoinData
    {
        public string IPv4Address;
        public ushort Port;
        public Guid AllocationID;
        public byte[] AllocationIDBytes;
        public byte[] ConnectionData;
        public byte[] HostConnectionData;
        public byte[] Key;
    }

    public static async Task<RelayHostData> AllocateRelayServerAndGetJoinCode(int maxConnections)
    {
        //Initialize the Unity Services engine
        await UnityServices.InitializeAsync();
        //Always autheticate your users beforehand
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        Allocation allocation;
        string createJoinCode;

        try
        {
            allocation  = await RelayService.Instance.CreateAllocationAsync(maxConnections);
               
        }
        catch (Exception e)
        {
            Debug.LogError($"Relay create allocation request failed {e.Message}");
            throw;
        }
        Debug.Log($"server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        Debug.Log($"server: {allocation.AllocationId}");

        try
        {
            createJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log($"Codigo: { createJoinCode}");
        }
        catch
        {
            Debug.LogError("Relay create join code request failed");
            throw;
        }

        var dtlsEndpoint = allocation.ServerEndpoints.First(e => e.ConnectionType == "dtls");

        RelayHostData data = new RelayHostData
        {
            // WARNING allocation.RelayServer is deprecated
            JoinCode = createJoinCode,
            IPv4Address = dtlsEndpoint.Host,
            Port = (ushort)dtlsEndpoint.Port,
            AllocationIDBytes = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            Key = allocation.Key,
        };

        return data;

        
    }

    public static async Task<RelayJoinData> JoinRelayServerFromJoinCode(string joinCode)
    {

        //Initialize the Unity Services engine
        await UnityServices.InitializeAsync();
        //Always authenticate your users beforehand
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        JoinAllocation allocation;
        try
        {
            allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        }
        catch
        {
            Debug.LogError("Relay create join code request failed");
            throw;
        }

        Debug.Log($"client: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        Debug.Log($"host: {allocation.HostConnectionData[0]} {allocation.HostConnectionData[1]}");
        Debug.Log($"client: {allocation.AllocationId}");

        var dtlsEndpoint = allocation.ServerEndpoints.First(e => e.ConnectionType == "dtls");

        RelayJoinData data = new RelayJoinData
        {
            // WARNING allocation.RelayServer is deprecated. It's best to read from ServerEndpoints.
            IPv4Address = dtlsEndpoint.Host,
            Port = (ushort)dtlsEndpoint.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            HostConnectionData = allocation.HostConnectionData,
            Key = allocation.Key,
        };

        return data;
    }

 
}
