﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Alex : sorry Keegan, I didn't wanted to do it. I initially wanted to "save" using a proper method like a .xml or .text but I'm afraid I don't know how it works.
 * 
 * I am creating this game Object to store stuff that we don't want to see again once we "Reload" or "SetActive" the scene. 
 * 
 * For example : - I want that when I click on the png, I don't have ALL the dialogue again but just another (shorter) one. 
 *               - I want that the fights are released slowly (that when we choose Fight 1 for the FIRST time, we don't see Fight 2 or Fight 3 yet). And which is being update (and kept) after every fight.
 *               - I want that once we exit fights, we come back from the door instead of the initial spawn point. 
 *               - I want that once we get a tutorial, we don't see it AGAIN until we create a new save.
 * 
 * I'll keep all the bools here in NeverUnload until we get a better and safer way to do it. (Indeed, the bools won't be kept if we quit the game... maybe I can pull that off I dont' know).
 * 
 * I'll try to organise it well so we can keep ALL the actions that needs to be saved here for ALL the scenes. 
 * 
 * I'll keep all the scripts using this manager into /System/Save, and put them here too.
 * 
 * 
*/
public class SaveManager : MonoBehaviour 
{
    [Header("General")]
    /*Scripts linked but can be used with any scenes
     * "ActivateTextAtLine.cs"
     */
    public List<string> dialogueSaver; 
    public List<string> cancelColliders; //Alex : I put this here because "CancelAnyShield.cs" is only used in "ArenaEntrance" for now. we'll move it in General if it is used more. "CancelAnyShield.cs" is used to avoid Triggers a second time (for tutorials for example)
    public List<string> tutorials;
    [Header("ArenaEntrance")]
    /* Scripts Linked with Arena Entrance : 
     *  "EnableFights.cs"
     */
    public List<string> fights;
    public int fightNumberAvailable; //The fight we're currently on. 
    public int currentFight;
    public bool returnFromArena;

    [Header("Inventory")]
    public List<WeaponObject> AvailableItems;
    public List<WeaponObject> StoredItems;
    public List<WeaponObject> EquipedItems;
    public int CurrentMoney;

    private void Start()
    {
        
    }

}