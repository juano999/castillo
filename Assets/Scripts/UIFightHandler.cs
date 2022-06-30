using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIFightHandler : MonoBehaviour
{

    public GameObject ChoosePnl;
    public GameObject CreatePnl;
    public GameObject JoinPnl;
    public GameObject OutGameUI;

    public Button startHostButton;
    public Button startClientButton;

    public InputField joinCodeInput;
    public Button playButton;
    public Text joinCodeText;

    void Start()
    {


        startHostButton?.onClick.AddListener(async () =>
        {
            RelayManager.RelayHostData data = await RelayManager.AllocateRelayServerAndGetJoinCode(2);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(data.IPv4Address, data.Port, data.AllocationIDBytes, data.Key, data.ConnectionData, true);
            NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("Host started...");

                joinCodeText.text = data.JoinCode;
            }
            else
            {
                Debug.Log("Unable to start host...");
            }
        });


        startClientButton?.onClick.AddListener(async () =>
       {
           if (!string.IsNullOrEmpty(joinCodeInput.text))
           {
               RelayManager.RelayJoinData data = await RelayManager.JoinRelayServerFromJoinCode(joinCodeInput.text);
               NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(data.IPv4Address, data.Port, data.AllocationIDBytes, data.Key, data.ConnectionData, data.HostConnectionData, true);
           }
           NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
           if (NetworkManager.Singleton.StartClient())
           {
               Debug.Log("Client started...");
               DisactivateOutGameUI();
           }
           else
               Debug.Log("Unable to start client...");



           
       });

        playButton?.onClick.AddListener(() =>
        {
            DisactivateOutGameUI();
        });
    }



    public void ActivateCreatePnl()
    {
        ChoosePnl.SetActive(false);
        CreatePnl.SetActive(true);
        Debug.Log("Desactivado: ChoosePanel, Activado: CreatePanel");
    }
    public void ActivateJoinPnl()
    {
        ChoosePnl.SetActive(false);
        JoinPnl.SetActive(true);
        Debug.Log("Desactivado: ChoosePanel, Activado: JoinPanel");
    }
    public void ActivateChoosePnl()
    {
        JoinPnl.SetActive(false);
        CreatePnl.SetActive(false);
        ChoosePnl.SetActive(true);

        Debug.Log("Desactivado: JoinPanel,CreatePanel ; Activado: ChoosePanel");
    }

    public void DisactivateOutGameUI()
    {
        OutGameUI.SetActive(false);
        Debug.Log("Desactivado: OutGameUI");
    }

    public void BackButtonHandler()
    {

        if (ChoosePnl.activeSelf)
        {
            Debug.Log("CAmbiando a Menu Principal");
            SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
        }
        else if (JoinPnl.activeSelf || CreatePnl.activeSelf)
        {
            ActivateChoosePnl();
        }
    }


    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        // The client identifier to be authenticated
        var clientId = request.ClientNetworkId;

        // Additional connection data defined by user code
        var connectionData = request.Payload;

        // Your approval logic determines the following values
        response.Approved = true;
        response.CreatePlayerObject = true;

        // The prefab hash value of the NetworkPrefab, if null the default NetworkManager player prefab is used
        response.PlayerPrefabHash = null;
        
        Debug.Log("ClientID" + clientId);
        Debug.Log("connectionData" + connectionData);

        // Position to spawn the player object (if null it uses default of Vector3.zero)
        if( clientId > 0)
        {
        response.Position = new Vector3(5.00f, 0.20f, 0);

        } else
        {

        response.Position = new Vector3(-7.00f, 0.20f, 0);
        }

        // Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
        response.Rotation = Quaternion.identity;

        // If additional approval steps are needed, set this to true until the additional steps are complete
        // once it transitions from true to false the connection approval response will be processed.
        response.Pending = false;
    }


}
