//Deprecated. Delete once we're sure references to this script are gone!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_switch : MonoBehaviour {

    public bool boyActive = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangePlayer();
        }
    }

    private void ChangePlayer()
    {
        boyActive = !boyActive;
    }
}
