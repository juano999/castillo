using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MatchManagerScript : NetworkBehaviour
{
    public NetworkVariable<int> FloorLevel = new NetworkVariable<int>();
    public NetworkVariable<int> PlayerWithAdvantage = new NetworkVariable<int>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        FloorLevel.Value = 0;
        PlayerWithAdvantage.Value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
