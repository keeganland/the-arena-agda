using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]


/* Alex : first step of creating a shop. Tutorial : https://unity3d.com/fr/learn/tutorials/modules/intermediate/live-training-archive/creating-an-in-game-shop
 *
 */

 public class WeaponObject : ScriptableObject {

    public Image Icon;

    public enum ObjectType {WEAPON, HEAD, HAND, CHEST, BOOTS, PASSIVE};
    public ObjectType Object;

    public bool forBoy;
    public bool forGirl;

    public string weaponName = "Weapon Name";
    public int cost = 5;
    public string description = "Weapon Description";


    //Basic Stats for Weapons;
    public float fireRate = .5f;
    public int bonusDamage = 10;
    public float range = 100;

    //Basic Stats for Armors;
    public int bonusHealth;
}
 