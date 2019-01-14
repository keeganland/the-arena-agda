using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessDamage : MonoBehaviour {

    private float DarknessTimer = 0.5F;
    private float ActualTime;
    private bool isDarkness;
    private bool isLight;
    private int HealAmount = 20;
    private int DamageAmount = 10;

    public Color _DamageColor;
    public Color _HealColor;

    // Use this for initialization
    void Start () {
        isDarkness = false;
        isLight = false;
        ActualTime = 0.0F;
	}
	
	// Update is called once per frame
	void Update () {
        //Check if the GameObject is not in the light
        Debug.Log("DarknessDamage: isLight = " + isLight);
        Debug.Log("DarknessDamage: isDarkness = " + isDarkness);
        Debug.Log("Darkness Damage: " + this.gameObject.name);
		while(isDarkness == true && isLight == false)
        {
            Debug.Log("Darkness Damage: In the dark and not the light");
            //If this is the ghoul, heal
            if (this.tag == "Enemy")
            {
                Debug.Log("DarknessDamage: Tag = Enemy");
                StartCoroutine("HealGhoul");
            }

            //If this is a player, deal damage
            if(this.tag == "Player")
            {
                Debug.Log("Darkness Damage: Tag = Player");
                StartCoroutine("DamagePlayer");
            }
        }
	}

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Darkness Damage: Trigger of " + other.name);
        if(other.tag == "Torches")
        {
            isLight = true;
        }
        if(other.tag == "Darkness")
        {
            isDarkness = true;
        }
    }

    //Healing the Ghoul
    private IEnumerator HealGhoul()
    {
        MessageHandler msgHandler = this.GetComponent<MessageHandler>();
        RecoverData rcvrData = new RecoverData();
        rcvrData.HP_up = HealAmount;
        if (msgHandler)
        {
            msgHandler.GiveMessage(MessageTypes.HEALED, this.gameObject, rcvrData);
            DisplayHealing(this.gameObject, _HealColor, HealAmount);
        }
        yield return new WaitForSeconds(DarknessTimer);
    }

    //Damaging the Players
    private IEnumerator DamagePlayer()
    {
        MessageHandler msgHandler = this.GetComponent<MessageHandler>();
        DamageData dmgData = new DamageData();
        dmgData.damage = DamageAmount;
        if (msgHandler)
        {
            msgHandler.GiveMessage(MessageTypes.DAMAGED, this.gameObject, dmgData);
            DisplayDamage(this.gameObject, _DamageColor, DamageAmount);
        }

        yield return new WaitForSeconds(DarknessTimer);
    }

    //Displaying the damage and updating slider
    private void DisplayDamage(GameObject targetdisplay, Color damageColor, int damageText)
    {
        GameObject go = targetdisplay.GetComponent<HealthController>().Sprite;
        Canvas[] canvas = go.GetComponentsInChildren<Canvas>();
        if (canvas.Length != 0)
        {
            for (int i = 0; i < canvas.Length; i++)
            {
                if (canvas[i].GetComponentInChildren<DamageDisplayScript>())
                    canvas[i].GetComponentInChildren<DamageDisplayScript>().GetDamageText(damageColor, damageText);
            }
        }
    }

    //Displaying Healing and updating slider
    private void DisplayHealing(GameObject targetdisplay, Color healingColor, int healingText)
    {
        GameObject go = targetdisplay.GetComponent<HealthController>().Sprite;
        Canvas[] canvas = go.GetComponentsInChildren<Canvas>();
        if (canvas.Length != 0)
        {
            for (int i = 0; i < canvas.Length; i++)
            {
                if (canvas[i].GetComponentInChildren<DamageDisplayScript>())
                    canvas[i].GetComponentInChildren<DamageDisplayScript>().GetDamageText(healingColor, healingText);
            }
        }
    }
}
