using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeletransporterScript : MonoBehaviour
{
    public float PositionX = 0;
    public float PositionY = 0;
    public string PlayerPermmited = "";


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if ((collision.gameObject.name == "Player1(Clone)" || collision.gameObject.name == "Player2(Clone)") && PlayerPermmited == collision.gameObject.name)
        {
            Debug.Log("objeto collision" + collision.gameObject.name);
            collision.gameObject.transform.position = new Vector3(PositionX, PositionY, 0);
        }
    }
}
