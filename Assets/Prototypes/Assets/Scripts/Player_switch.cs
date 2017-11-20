using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_switch : MonoBehaviour {

    public bool _Switchplayer = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangePlayer();
        }
    }

    private void ChangePlayer()
    {
        _Switchplayer = !_Switchplayer;
    }
}
