using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBossRange : MonoBehaviour {
    public int damageToGiveRange;
    public bool enterExit;

	// Use this for initialization
	void Start () {
        enterExit = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (enterExit == true)
        {
            if (other.gameObject.tag == "Enemy")
            {
                    other.gameObject.GetComponent<BossHealthManager>().HurtBossRange(damageToGiveRange);
                    enterExit = false; 
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enterExit = true;
        }
    }
}
