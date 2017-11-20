using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthManager : MonoBehaviour {
    public int BossMaxHealth;
    public int BossCurrentHealth;

	// Use this for initialization
	void Start () {
        BossCurrentHealth = BossMaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        //takes out boss once they die
        if (BossCurrentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    //boy hurting boss
    public void HurtBossMelee(int damageToGiveMelee)
    {
        BossCurrentHealth -= damageToGiveMelee;
    }
    //girl hurting boss
    public void HurtBossRange(int damageToGiveRange)
    {
        BossCurrentHealth -= damageToGiveRange;
    }
}
