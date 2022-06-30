using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMusicManager : MonoBehaviour
{
    
    private void Awake()
    {
        Destroy(GameObject.FindGameObjectWithTag("GameMusic")); 
    }
}
