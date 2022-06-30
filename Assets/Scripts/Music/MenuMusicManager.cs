using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        GameObject [] musicObj = GameObject.FindGameObjectsWithTag("GameMusic");
        //Debug.Log(SceneManager.GetActiveScene().name == "FightScene");
        //if(SceneManager.GetActiveScene().name == "FightScene" || musicObj.Length > 1)
        if(musicObj.Length > 1)
        {
            Destroy(this.gameObject);
        } 
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }

    }
}
