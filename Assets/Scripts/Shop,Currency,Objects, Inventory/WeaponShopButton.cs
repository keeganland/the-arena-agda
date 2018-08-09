using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponShopButton : MonoBehaviour {

    public WeaponObject weapon;

    public Text weaponName;
    public Text weaponDescription;
    public Text weaponDescription2;
    public RawImage weaponImage;
    public GameObject aquiredWeapon;

    public Text currentMoney;

    public void OnEnable()
    {
        currentMoney.text = "$" + FindObjectOfType<SaveManager>().CurrentMoney.ToString();
    }

    public void AquireWeapon()
    {
        InventoryManager.AquireItem(weapon.weaponName);

        Debug.Log("here");
        weaponName.text = weapon.weaponName;
        weaponImage.texture = weapon.Icon.texture;
        weaponDescription.text = weapon.description;
        weaponDescription2.text = weapon.description2;
        aquiredWeapon.SetActive(true);
    }
}
