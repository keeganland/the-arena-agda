using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponShopButton : MonoBehaviour {

    public WeaponObject[] weapon;

    public Text weaponName;
    public Text weaponDescription;
    public Text weaponDescription2;
    public RawImage weaponImage;
    public GameObject aquiredWeapon;

    public Text currentMoney;

    private int weaponNumber = 0;

    public void OnEnable()
    {
        currentMoney.text = "$" + SaveManager.Instance.CurrentMoney.ToString();
    }

    public void AquireWeapon()
    {
        if(FindObjectOfType<SaveManager>().currentFight == 1)
        {
            weaponNumber = 1;
        }

        InventoryManager.AquireItem(weapon[weaponNumber].weaponName);

        weaponName.text = weapon[weaponNumber].weaponName;
        weaponImage.texture = weapon[weaponNumber].Icon.texture;
        weaponDescription.text = weapon[weaponNumber].description;
        weaponDescription2.text = weapon[weaponNumber].description2;
        aquiredWeapon.SetActive(true);
    }
}
