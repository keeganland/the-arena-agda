﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    /* Alex : I don't know how i'll do it but here is what I think it should do : 
     * 
     * - Get all items available in the game, 
     * - select the ones the player has access to (through SaveManager and an array ?), 
     * - display them in the inventory UI,
     * - get the Items the Player currently has equiped,
     * - give the bonuses to the player (change weapon, etc...).
     * 
     * I propose that we store the items equiped, available, and all items in arrays then move the items between arrays.
     * 
     * Here is what I'll work on right now : create passive items (that doesn't need inventory, UI, and such) and make sure they impact the player's stats. 
     * 
     * I'll copy the structure of "EventMAnager" (Which is a singleton?).
     */
    private static InventoryManager inventoryManager;

    public static List<WeaponObject> AvailableItems; //All items ?
    public static List<WeaponObject> StoredItems;  //Item available in the inventory UI;
    public static List<WeaponObject> EquipedItems; //How it will work : All items of type WeaponObjects has a type : Head, Chest, Passive, Hand, etc... The player will be allowed only ONE 
                                            //item of each equiped. (One weapon + helmet + passive), etc...  

    static WeaponObject weaponToEquip;

    static float BoyFireRate = 5;
    static int BoyDamage = 50;
    static float BoyRange;
    static int BoyHealth = 60;

    static float GirlFireRate = 1.5f;
    static int GirlDamage = 10;
    static float GirlRange;
    static int GirlHealth = 40;

    public static GameObject Boy;
    public static GameObject Girl;

    public static int CurrentMoney = 0;

    /*Alex : What CURRENTLY DOESN'T WORK : 
     * 
     *  we only have an "EquipedItems" array for the girl AND the boy, which means that only one HELMET, CHEST, PASSIVE, etc... WeaponObject can be equiped for both. 
     *  we'll need to have 2 different array (one for girl and one for boy). Which means review the logic in CalculateBonuses(); 
     */

    public static InventoryManager Instance
    {
        get
        {
            if (!inventoryManager)
            {
                inventoryManager = FindObjectOfType(typeof(InventoryManager)) as InventoryManager;

                if (!inventoryManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    inventoryManager.Init();
                }
            }
            return inventoryManager;
        }
    }

    void Init()
    {
        if (!Boy)
            Boy = GameObject.Find("Boy");
        if (!Girl)
            Girl = GameObject.Find("Girl");

        //Get all items, equiped items, and available items from SaveManager? 
        LoadWeaponsSave();
        CalculateBonuses();

    }

    private void Awake() //Alex : I don't understand the "Init" part, so I put that in Awake. It needs to be run before everything else.
    {
        if (!Boy)
            Boy = GameObject.Find("Boy");
        if (!Girl)
            Girl = GameObject.Find("Girl");

        //Get all items, equiped items, and available items from SaveManager? 
        LoadWeaponsSave();
        CalculateBonuses();
    }

    public static void AquireItem(string itemName) //Every time the player will find an item, I'll put the item from the Array AvailableItems to StoredItems;
    {
        foreach(WeaponObject item in AvailableItems)
        {
            if (item.weaponName == itemName && !StoredItems.Contains(item) && item.cost <= CurrentMoney)
            {
                Debug.Log("You Bought The Item");
                StoredItems.Add(item);
                SubstractMoney(item.cost);
                EquipItem(item.weaponName);
            }
            else if (item.weaponName == itemName && item.cost <= CurrentMoney)
            {
                return; //Do Item +1, for exemple : Flower in current inventory = 1, then we'll have 2 flowers.
            }
            else if (item.weaponName == itemName && item.cost > CurrentMoney)
            {
                Debug.Log("You do not have enough money");
            }
            else return;                
        }

        FindObjectOfType<SaveManager>().StoredItems = StoredItems;
    }

    public static void RemoveItemInventory(string itemName)
    {
        foreach (WeaponObject item in StoredItems)
        {
            if (item.weaponName == itemName)
            {
                StoredItems.Remove(item); //Remove item or decrease item count (-1).
            }
            else return; 
        }

        FindObjectOfType<SaveManager>().StoredItems = StoredItems;
    }

    public static void EquipItem(string itemName) 
    {
        Debug.Log(itemName);
        foreach(WeaponObject weapon in StoredItems)
        {
            if(weapon.weaponName == itemName)
            {
                weaponToEquip = weapon;
            }
            else
            {
                Debug.Log("You can't equip an item you don't have in your inventory"); 
            }
        }

        Debug.Log(weaponToEquip);

        foreach(WeaponObject item in StoredItems)
        {
            if(item.Object == weaponToEquip.Object)
            {
                EquipedItems.Remove(item);
                EquipedItems.Add(weaponToEquip);
            }
            else 
            {
                EquipedItems.Add(weaponToEquip);
                Debug.Log("You Equiped The Item");
            }
        }

        FindObjectOfType<SaveManager>().EquipedItems = EquipedItems;
        CalculateBonuses();
    }

    public static void UnEquipItem(string itemName)
    {
        foreach(WeaponObject weapon in EquipedItems)
        {
            if (weapon.weaponName == itemName)
                EquipedItems.Remove(weapon);
        }

        FindObjectOfType<SaveManager>().EquipedItems = EquipedItems;
    }

    private static void CalculateBonuses()
    {
        int BoyBonusHealth = 0;
        int GirlBonusHealth = 0;

        if (EquipedItems.Count != 0)
        {
            foreach (WeaponObject equipedWeapon in EquipedItems)
            {
                if (equipedWeapon)
                {
                    if (equipedWeapon.forBoy)
                    {
                        if (equipedWeapon.Object == WeaponObject.ObjectType.WEAPON)
                        {
                            BoyFireRate = equipedWeapon.fireRate;
                            Boy.GetComponent<MeleeDamage>().AttackSpeed = BoyFireRate;
                        }

                        BoyDamage += equipedWeapon.bonusDamage;
                        BoyBonusHealth += equipedWeapon.bonusHealth;
                    }
                    if (equipedWeapon.forGirl)
                    {
                        if (equipedWeapon.Object == WeaponObject.ObjectType.WEAPON)
                        {
                            GirlFireRate = equipedWeapon.fireRate;
                            Girl.GetComponent<MeleeDamage>().AttackSpeed = GirlFireRate;
                        }

                        GirlDamage += equipedWeapon.bonusDamage;
                        GirlBonusHealth += equipedWeapon.bonusHealth;
                    }
                }
            }

            Boy.GetComponent<HealthController>().totalHealth = BoyHealth + BoyBonusHealth;
            Boy.GetComponent<HealthController>().currentHealth += BoyBonusHealth;
            Boy.GetComponent<MeleeDamage>().Damage = BoyDamage;

            Girl.GetComponent<HealthController>().totalHealth = GirlHealth + GirlBonusHealth;
            Girl.GetComponent<HealthController>().currentHealth += GirlBonusHealth;
            Girl.GetComponent<MeleeDamage>().Damage = GirlDamage;

            EventManager.TriggerEvent("refreshUI");
        }
    }

    private static void LoadWeaponsSave()
    {
        AvailableItems = FindObjectOfType<SaveManager>().AvailableItems;
        StoredItems = FindObjectOfType<SaveManager>().StoredItems;
        EquipedItems = FindObjectOfType<SaveManager>().EquipedItems;
        CurrentMoney = FindObjectOfType<SaveManager>().CurrentMoney;
    }

    public static void AddMoney(int addmoney)
    {
        CurrentMoney += addmoney;
        FindObjectOfType<SaveManager>().CurrentMoney = CurrentMoney;
    }

    public static void SubstractMoney(int substractmoney)
    {
        CurrentMoney -= substractmoney;
        FindObjectOfType<SaveManager>().CurrentMoney = CurrentMoney;
    }
}
