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
}
