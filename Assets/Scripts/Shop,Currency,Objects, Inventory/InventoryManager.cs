using System.Collections;
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

    public static Dictionary<string, WeaponObject> AvailableItems = new Dictionary<string, WeaponObject>(); //All items ?
    public static Dictionary<string,WeaponObject> StoredItems = new Dictionary<string, WeaponObject>();  //Item available in the inventory UI;
    public static Dictionary<string,WeaponObject> EquipedItemsBoy = new Dictionary<string, WeaponObject>(); //How it will work : All items of type WeaponObjects has a type : Head, Chest, Passive, Hand, etc... The player will be allowed only ONE 
    public static Dictionary<string, WeaponObject> EquipedItemsGirl = new Dictionary<string, WeaponObject>();
    //item of each equiped. (One weapon + helmet + passive), etc...  
    public delegate void OnItemChanged();
    public static OnItemChanged onItemChangedCallback;

    static WeaponObject weaponToEquip;

    public static int itemsMax = 20;
    public static InventoryUI inventoryUI;

    static float BoyFireRate = 0;
    static int BoyDamage = 0;
    static float BoyRange;
    static int BoyHealth = 60;

    static float GirlFireRate = 0;
    static int GirlDamage = 0;
    static float GirlRange;
    static int GirlHealth = 40;

    static int BoyDamageInitial = 0;
    static int BoyHealthInitial = 60;
    static int GirlDamageInitial = 0;
    static int GirlHealthInitial = 40;

    static int BoyHealthBonusFromItems;
    static int GirlHealthBonusFromItems;
    static int BoyDamagesBonusFromItems;
    static int GirlDamagesBonusFromItems;

    public static GameObject Boy;
    public static GameObject Girl;

    public GameObject InventoryTab;
    private bool InventoryTabActive;
    static bool BoyItemChanged = true;
    static bool GirlItemChanged = true;

    public static int CurrentMoney = 0;

    public WeaponObject initialgirlweapon;
    public WeaponObject initialboyweapon;

    public GameObject StatsHolder;

    private AudioSource m_audioSource;
    public float SoundScaleFactor;
    public AudioClip OpenBagSFX;
    public AudioClip CloseBagSFX;

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
        inventoryUI = GetComponent<InventoryUI>();
    }

    private void Start() //Alex : I don't understand the "Init" part, so I put that in Awake. It needs to be run before everything else.
    {
        LoadWeaponsSave();

        if (!Boy)
            Boy = GameObject.Find("Boy");
        if (!Girl)
            Girl = GameObject.Find("Girl");

        AquireItem(initialgirlweapon.weaponName);
        EquipItem(initialgirlweapon.weaponName, 1);

        AquireItem(initialboyweapon.weaponName);
        EquipItem(initialboyweapon.weaponName, 2);

        GameObject.Find("/Inventory/CharacterBackgroundimage/GirlWeapon").GetComponent<InventorySlot>().AddItem(initialgirlweapon);
        GameObject.Find("/Inventory/CharacterBackgroundimage/BoyWeapon").GetComponent<InventorySlot>().AddItem(initialboyweapon);

        InventorySlot[] inventoryslots = GameObject.Find("/Inventory/ObjectsBackgroundimage/InventoryBackground").GetComponentsInChildren<InventorySlot>();

        foreach (InventorySlot slots in inventoryslots)
        {
            slots.ClearSlot();
        }

        InventoryTab.SetActive(false);

        //Get all items, equiped items, and available items from SaveManager? 
        CalculateBonuses();
        inventoryUI = GetComponent<InventoryUI>();

        m_audioSource = GetComponent<AudioSource>();
        SoundManager.onSoundChangedCallback += UpdateSound;
    }

    private void Update()
    {
        InventoryUI();
    }

    public static void AquireItem(string itemName) //Every time the player will find an item, I'll put the item from the Array AvailableItems to StoredItems;
    {
        WeaponObject item;
        //string itemKey;
        AvailableItems.TryGetValue(itemName,out item);

        if (item.weaponName == itemName && !StoredItems.ContainsValue(item) && item.cost <= CurrentMoney)
         {
                //Debug.Log("You Bought The Item");
                StoredItems.Add(item.weaponName, item);
                SaveManager.StoredItems.Add(item);
                SubstractMoney(item.cost);
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

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    public static void RemoveItemInventory(string itemName)
    {
        if (StoredItems.ContainsKey(itemName))
        {
            WeaponObject item;
            StoredItems.TryGetValue(itemName, out item);
            StoredItems.Remove(itemName); //Remove item or decrease item count (-1).
            SaveManager.StoredItems.Remove(item);

            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
        }
    }

    public static void EquipItem(string itemName, int boy) //Boy = true, Girl = false; 
    {
        //Debug.Log("How many times do I try to equip?");
        //Debug.Log("Is my object stored ? " + StoredItems.ContainsKey(itemName));

        if (StoredItems.ContainsKey(itemName))
        {
            StoredItems.TryGetValue(itemName, out weaponToEquip);
        }
        else return;

        if(boy == 2)
        {
            //Debug.Log("Here with the 2 boy");
            List<string> Keys = new List<string>(EquipedItemsBoy.Keys);

            if(EquipedItemsBoy.Count != 0)
            {
                foreach(string key in Keys)
                {
                    WeaponObject item;
                    EquipedItemsBoy.TryGetValue(key, out item);

                    if(weaponToEquip.slotType == item.slotType)
                    {
                        EquipedItemsBoy.Remove(item.weaponName);
                        EquipedItemsBoy.Add(weaponToEquip.weaponName, weaponToEquip);
                        SaveManager.EquipedItemsBoy.Add(weaponToEquip);
                        SaveManager.EquipedItemsBoy.Remove(item);
                    }
                    else
                    {
                        EquipedItemsBoy.Add(weaponToEquip.weaponName, weaponToEquip);
                        SaveManager.EquipedItemsBoy.Add(weaponToEquip);
                        //Debug.Log("You Equiped The Item for the boy");
                    }
                } 
            }
            else
            {
                EquipedItemsBoy.Add(weaponToEquip.weaponName, weaponToEquip);
                SaveManager.EquipedItemsBoy.Add(weaponToEquip);
                //Debug.Log("You Equiped The Item for the boy");
            }
            BoyItemChanged = true;
            //Debug.Log(EquipedItemsBoy.ContainsKey(itemName));
        }
        if(boy == 1)
        {
            //Debug.Log("Here with the 1st boy");
            List<string> Keys = new List<string>(EquipedItemsGirl.Keys);

            if (EquipedItemsGirl.Count != 0)
            {
                foreach (string key in Keys)
                {
                    WeaponObject item;
                    EquipedItemsGirl.TryGetValue(key, out item);

                    if (weaponToEquip.slotType == item.slotType)
                    {
                        EquipedItemsGirl.Remove(item.weaponName);
                        EquipedItemsGirl.Add(weaponToEquip.weaponName, weaponToEquip);
                        SaveManager.EquipedItemsGirl.Add(weaponToEquip);
                        SaveManager.EquipedItemsGirl.Remove(item);
                    }
                    else
                    {
                        EquipedItemsGirl.Add(weaponToEquip.weaponName, weaponToEquip);
                        SaveManager.EquipedItemsGirl.Add(weaponToEquip);
                        //Debug.Log("You Equiped The Item for the boy");
                    }
                }
            }

            else
            {
                EquipedItemsGirl.Add(weaponToEquip.weaponName, weaponToEquip);
                SaveManager.EquipedItemsGirl.Add(weaponToEquip);
                //Debug.Log("You Equiped The Item for the boy");
            }
            GirlItemChanged = true;
        }
        CalculateBonuses();
    }

    public static void UnEquipItem(string itemName, int boy)
    {
        if (boy == 2)
        {
            //Debug.Log(EquipedItemsBoy.ContainsKey(itemName));
            WeaponObject weaponToRemove;
            EquipedItemsBoy.TryGetValue(itemName, out weaponToRemove);


            //Debug.Log(EquipedItemsBoy.Count + " Equiped Count");
            EquipedItemsBoy.Remove(weaponToRemove.weaponName);
            SaveManager.EquipedItemsBoy.Remove(weaponToRemove);

            //Debug.Log(EquipedItemsBoy.Count + " Equiped Count 2");

            //Debug.Log(SaveManager.EquipedItemsBoy.Count + " Save manager saved");
            //Debug.Log("Unequip");
            BoyItemChanged = true;
        }
        else if(boy == 1)
        {
            WeaponObject weaponToRemove;
            EquipedItemsGirl.TryGetValue(itemName, out weaponToRemove);
            Debug.Log(weaponToRemove.weaponName);

            //Debug.Log(EquipedItemsGirl.Count + " Equiped Count");
            EquipedItemsGirl.Remove(weaponToRemove.weaponName);
            SaveManager.EquipedItemsGirl.Remove(weaponToRemove);

            //Debug.Log(EquipedItemsGirl.Count + " Equiped Count 2");

            //Debug.Log(SaveManager.EquipedItemsGirl.Count + " Save manager saved");
            //Debug.Log("Unequip");
            GirlItemChanged = true;
        }

        CalculateBonuses();
    }

    private static void CalculateBonuses()
    {
        //Debug.Log("Let's Calculate some bonuses!" + EquipedItemsBoy.Count + " with this many item equiped in Calculation() for the Boy");
        //Debug.Log("Let's Calculate some bonuses!" + EquipedItemsGirl.Count + " with this many item equiped in Calculation() for the Girl");

        int BoyBonusHealth = 0;
        int GirlBonusHealth = 0;

        int BoyBonusDamages = 0;
        int GirlBonusDamages = 0;

        if (EquipedItemsBoy.Count != 0 && BoyItemChanged)
        {
            foreach (KeyValuePair<string, WeaponObject> EquipedPair in EquipedItemsBoy)
            {
                WeaponObject equipedWeapon = EquipedPair.Value;

                if (equipedWeapon)
                {
                    if (equipedWeapon.forBoy)
                    {
                        if (equipedWeapon.slotType == WeaponObject.ObjectType.WEAPON)
                        {
                            BoyFireRate = 5/equipedWeapon.fireRate;
                            Boy.GetComponent<MeleeDamage>().AttackSpeed = BoyFireRate;
                            Boy.GetComponentInChildren<RangeChecker>().GetComponent<SphereCollider>().radius = equipedWeapon.range;
                        }

                        BoyBonusDamages += equipedWeapon.bonusDamage;
                        BoyBonusHealth += equipedWeapon.bonusHealth;

                    }
                }
            }

            BoyDamagesBonusFromItems = BoyBonusDamages;
            BoyHealthBonusFromItems = BoyBonusHealth;

            Boy.GetComponent<HealthController>().totalHealth = BoyHealth + BoyBonusHealth;
            Boy.GetComponent<HealthController>().currentHealth += BoyBonusHealth;
            Boy.GetComponent<MeleeDamage>().Damage = BoyDamage + BoyBonusDamages;

            if (Boy.GetComponent<HealthController>().currentHealth > Boy.GetComponent<HealthController>().totalHealth)
            {
                Boy.GetComponent<HealthController>().currentHealth = Boy.GetComponent<HealthController>().totalHealth;
            }

        }
        else if(BoyItemChanged && EquipedItemsBoy.Count == 0)
        {
            Boy.GetComponent<HealthController>().totalHealth = BoyHealthInitial;
            Boy.GetComponent<HealthController>().currentHealth -= BoyHealthBonusFromItems;
            Boy.GetComponent<MeleeDamage>().Damage = BoyDamageInitial;

            BoyHealthBonusFromItems = 0;
            BoyDamagesBonusFromItems = 0;
        }

        if (EquipedItemsGirl.Count != 0 && GirlItemChanged)
        {
            Debug.Log("Here for Girl");
            foreach (KeyValuePair<string, WeaponObject> EquipedPair in EquipedItemsGirl)
            {
                WeaponObject equipedWeapon = EquipedPair.Value;

                if (equipedWeapon)
                {
                    if (equipedWeapon.forGirl)
                    {
                        if (equipedWeapon.slotType == WeaponObject.ObjectType.WEAPON)
                        {
                            GirlFireRate = 5/equipedWeapon.fireRate;
                            Girl.GetComponent<MeleeDamage>().AttackSpeed = GirlFireRate;
                            Girl.GetComponentInChildren<RangeChecker>().GetComponent<SphereCollider>().radius = equipedWeapon.range;
                        }

                        GirlBonusDamages += equipedWeapon.bonusDamage;
                        GirlBonusHealth += equipedWeapon.bonusHealth;
                    }
                }
            }

            GirlHealthBonusFromItems = GirlBonusHealth;
            GirlDamagesBonusFromItems = GirlBonusDamages;

            Girl.GetComponent<HealthController>().totalHealth = GirlHealth + GirlBonusHealth;
            Girl.GetComponent<HealthController>().currentHealth += GirlBonusHealth;
            Girl.GetComponent<MeleeDamage>().Damage = GirlDamage + GirlBonusDamages;

            if(Girl.GetComponent<HealthController>().currentHealth > Girl.GetComponent<HealthController>().totalHealth)
            {
                Girl.GetComponent<HealthController>().currentHealth = Girl.GetComponent<HealthController>().totalHealth;
            }
        }
        else if(EquipedItemsGirl.Count == 0 && GirlItemChanged)
        {
            Girl.GetComponent<HealthController>().totalHealth = GirlHealthInitial;
            Girl.GetComponent<HealthController>().currentHealth -= GirlHealthBonusFromItems;
            Girl.GetComponent<MeleeDamage>().Damage = GirlDamageInitial;

            GirlHealthBonusFromItems = 0;
            GirlDamagesBonusFromItems = 0;
        }

        EventManager.TriggerEvent("refreshUI");
        EventManager.TriggerEvent("RefreshInventoryUI");

        BoyItemChanged = false;
        GirlItemChanged = false;
    }

    private static void LoadWeaponsSave()
    {
        if (SaveManager.AvailableItems.Count != 0)
        {
            foreach (WeaponObject item in SaveManager.AvailableItems)
            {
                AvailableItems.Add(item.weaponName, item);
            }
        }
        if (SaveManager.StoredItems.Count != 0)
        {
            foreach (WeaponObject item in SaveManager.StoredItems)
            {
                StoredItems.Add(item.weaponName, item);
            }
        }
        if (SaveManager.EquipedItemsBoy.Count != 0)
        {
            foreach (WeaponObject item in SaveManager.EquipedItemsBoy)
            {
                EquipedItemsBoy.Add(item.weaponName, item);
            }
        }
        if (SaveManager.EquipedItemsGirl.Count != 0)
        {
            foreach (WeaponObject item in SaveManager.EquipedItemsGirl)
            {
                EquipedItemsGirl.Add(item.weaponName, item);
            }
        }
        CurrentMoney = SaveManager.CurrentMoney;
    }

    public static void AddMoney(int addmoney)
    {
        CurrentMoney += addmoney;
        SaveManager.CurrentMoney = CurrentMoney;
    }

    public static void SubstractMoney(int substractmoney)
    {
        CurrentMoney -= substractmoney;
        SaveManager.CurrentMoney = CurrentMoney;
    }

    private void InventoryUI()
    {
        if(Input.GetKeyDown(KeyCode.I) && !InventoryTabActive)
        {
            InventoryTab.SetActive(true);
            InventoryTabActive = true;
            m_audioSource.PlayOneShot(OpenBagSFX);
        }
        else if(Input.GetKeyDown(KeyCode.I) && InventoryTabActive)
        {
            StatsHolder.SetActive(false);
            InventoryTab.SetActive(false);
            InventoryTabActive = false;
            m_audioSource.PlayOneShot(CloseBagSFX);
        }
    }

    void UpdateSound()
    {
        m_audioSource.volume = (SoundManager.SFXVolume * SoundScaleFactor) / 100;
    }
}
