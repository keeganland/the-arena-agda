using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitAquiredWeapon : MonoBehaviour {
	
	// Update is called once per frame
	void Update () 
    {
        if(Input.anyKeyDown == true)
        {
            gameObject.SetActive(false);
        }
	}
}
