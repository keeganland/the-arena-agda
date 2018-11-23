using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObjectsPickUp : InteractiveObjectAbstract {

    public WeaponObject item;
    public AudioClip PickUpItem;
    private AudioSource m_audioSource;

    public float SoundScaleFactor;

    new void Start()
    {
        base.Start();

        m_audioSource = GetComponent<AudioSource>();
        SoundManager.onSoundChangedCallback += UpdateSound;
    }

    public override void ActionFunction(GameObject sender)
    {
        //Debug.Log("You aquiring : " + item.weaponName);   
        InventoryManager.AquireItem(item.weaponName);
        //Debug.Log(SaveManager.StoredItems.Count + "Has been stored?");
        m_audioSource.PlayOneShot(PickUpItem);
        Destroy(this.gameObject, 0.5f);
    }

    public override void DoAction(GameObject sender)
    {
        //Debug.Log("Pickup theo object");
        ActionFunction(sender);
    }

    public override IEnumerator Action(GameObject sender)
    {
        throw new System.NotImplementedException();
    }

    public override void CancelAction(GameObject sender)
    {
        throw new System.NotImplementedException();
    }

    void UpdateSound()
    {
        m_audioSource.volume = (SoundManager.SFXVolume * SoundScaleFactor) / 100;
    }
}
