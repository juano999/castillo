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

    public void ExitGame()
    {
        Application.Quit();
    }
   
    public void GoToFightScene()
    {
        SceneManager.LoadScene("FightScene", LoadSceneMode.Single);
    }
    public void GoToCredits()
    {
        SceneManager.LoadScene("CreditsScene", LoadSceneMode.Single);
    }

    
}
