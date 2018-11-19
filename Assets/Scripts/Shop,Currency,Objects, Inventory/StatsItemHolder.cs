using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsItemHolder : MonoBehaviour {

    //For this stats item holder, I think the best is to create a function that will automaticaly place the non-null stats in order. For exemple, if HP bonus of an item is 0, we don't want it displayed. 

    Text[] textholders;
    WeaponObject weaponObject;

	void OnEnable()
    {
        textholders = GetComponentsInChildren<Text>();
        textholders[0].text = weaponObject.weaponName;

        if(weaponObject.bonusHealth != 0)
        {
            for(int i = 1; i < textholders.Length; i++)
            {
                if(textholders[i].text == "")
                {
                    textholders[i].text = "Bonus HP : " + ("<color=cyan>" + weaponObject.bonusHealth.ToString() + "</color>");
                    break;
                }
            }
        }

        if(weaponObject.bonusDamage != 0)
        {
            for (int i = 1; i < textholders.Length; i++)
            {
                if (textholders[i].text == "")
                {
                    textholders[i].text = "Bonus Damages : " + ("<color=cyan>" + weaponObject.bonusDamage.ToString() + "</color>");
                    break;
                }
            }
        }

        if (weaponObject.fireRate != 0)
        {
            for (int i = 1; i < textholders.Length; i++)
            {
                if (textholders[i].text == "")
                {
                    textholders[i].text = "Attack Speed : " + ("<color=cyan>" + weaponObject.fireRate.ToString() + "</color>");
                    break;
                }
            }
        }

        if (weaponObject.range != 0)
        {
            for (int i = 1; i < textholders.Length; i++)
            {
                if (textholders[i].text == "")
                {
                    textholders[i].text = "Attack Range : " + ("<color=cyan>" + weaponObject.range.ToString() + "</color>");
                    break;
                }
            }
        }


    }

    private void OnDisable()
    {
        foreach(Text text in textholders)
        {
            text.text = "";
        }
    }
    // Update is called once per frame
    void Update ()
    {
        transform.position = Input.mousePosition;
	}

    public void GetWeaponObject(WeaponObject weapon)
    {
        weaponObject = weapon;
    }
}
