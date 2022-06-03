using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public void GoToCreateOrJoinScene()
    {
        SceneManager.LoadScene("CreateJoinScene", LoadSceneMode.Single);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
    }

    public void GoToCreateGame()
    {
        SceneManager.LoadScene("CreateGameScene", LoadSceneMode.Single);
    }
    public void GoToJoinGame()
    {
        SceneManager.LoadScene("JoinGameScene", LoadSceneMode.Single);
    }
    public void Play()
    {
        SceneManager.LoadScene("FightScene", LoadSceneMode.Single);
    }
    public void GoToCredits()
    {
        SceneManager.LoadScene("CreditsScene", LoadSceneMode.Single);
    }
}
