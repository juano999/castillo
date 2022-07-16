using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerPointScript : MonoBehaviour
{
    public MatchManagerScript matchManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player1(Clone)")
        {
            matchManager.RequestActivateWinnerUIServerRpc(1);
        } else if (collision.gameObject.name == "Player2(Clone)")
        {
            matchManager.RequestActivateWinnerUIServerRpc(2);
        }
    }
}
