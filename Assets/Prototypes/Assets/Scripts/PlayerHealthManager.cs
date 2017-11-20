using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour {
    public int playerMaxHealth;
    public int playerCurrentHealth;

	// Use this for initialization
	void Start () {
        playerCurrentHealth = playerMaxHealth;

	}
	
	// Update is called once per frame
	void Update () {
        //takes out player once they die
        if (playerCurrentHealth <= 0)
        {
            gameObject.SetActive(false);
        }

	}


    public void HurtPLayer(int damageToGive)
    {
        playerCurrentHealth -= damageToGive;
    }

}