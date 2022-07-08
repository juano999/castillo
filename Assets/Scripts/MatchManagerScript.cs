using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MatchManagerScript : NetworkBehaviour
{
    public Camera cam;

    public NetworkVariable<int> FloorLevel = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    public NetworkVariable<int> PlayerWithAdvantage = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public GameObject DoorLeft;
    public GameObject DoorRight;
    public GameObject GoAheadBlue;
    public GameObject GoAheadRed;
    

    private Vector3[,] spawnPositions =
    {
        { new Vector3(-5.29f,0.20f,0), new Vector3(5.29f,0.20f,0)},     //Piso 0
        { new Vector3(-5.29f,3.0f,0), new Vector3(5.29f,3.0f,0)},       //Piso 1
        { new Vector3(-5.29f,-2.8f,0), new Vector3(5.29f,-2.8f,0)},     //Piso -1
        { new Vector3(-5.29f,5.86f,0), new Vector3(5.29f,0,0)},         //Piso 2 (Victoria Azul)
        { new Vector3(-5.29f,0,0), new Vector3(5.29f,-5.62f,0)},        //Piso -2 (Victoria Rojo)
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        FloorLevel.OnValueChanged += OnFloorLevelChange;
        PlayerWithAdvantage.OnValueChanged += PlayerWithAdvantageChanged;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnFloorLevelChange(int previous, int current)
    {
        GameObject player1 = GameObject.Find("Player1(Clone)");
        GameObject player2 = GameObject.Find("Player2(Clone)");
        if(player1==null || player2==null)
        {
            return;
        }
        switch (current)
        {
            case 0:
                cam.transform.position = new Vector3(0, 0, -9);
                player1.transform.position = spawnPositions[0, 0];
                player2.transform.position = spawnPositions[0, 1];
                break;
            case 1:
                cam.transform.position = new Vector3(0, 3.11f, -9);
                player1.transform.position = spawnPositions[1, 0];
                player2.transform.position = spawnPositions[1, 1];
                break;
            case -1:
                cam.transform.position = new Vector3(0, -2.91f, -9);
                player1.transform.position = spawnPositions[2, 0];
                player2.transform.position = spawnPositions[2, 1];
                break;
            case 2:
                cam.transform.position = new Vector3(0, 5.94f, -9);
                player1.transform.position = spawnPositions[3, 0];
                player2.transform.position = spawnPositions[3, 1];
                break;
            case -2:
                cam.transform.position = new Vector3(0, -5.24f, -9);
                player1.transform.position = spawnPositions[4, 0];
                player2.transform.position = spawnPositions[4, 1];
                break;
            default:
                cam.transform.position = new Vector3(0, 0, -9);
                break;
        }
        ChangePlayerWithAdvantageServerRpc(-1);
    }
    public void PlayerWithAdvantageChanged(int previous, int current)
    {
        Debug.Log("Player With Advantage: " + current);
        if(current == 0)
        {
            GoAheadBlue.SetActive(false);
            GoAheadRed.SetActive(true);
            DoorLeft.SetActive(false);
            DoorRight.SetActive(true);
        } else if (current == 1)
        {
            GoAheadRed.SetActive(false);
            GoAheadBlue.SetActive(true);
            DoorRight.SetActive(false);
            DoorLeft.SetActive(true);
        } else
        {
            GoAheadRed.SetActive(false);
            GoAheadBlue.SetActive(false);
            DoorRight.SetActive(true);
            DoorLeft.SetActive(true);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeFloorServerRpc(int newFloorLevel)
    {
        FloorLevel.Value = newFloorLevel;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangePlayerWithAdvantageServerRpc(int numPlayer)
    {
        PlayerWithAdvantage.Value = numPlayer;
    }
}
