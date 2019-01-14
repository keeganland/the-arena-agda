using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDarknessDamage : MonoBehaviour
{
    private float DarknessTimer = 0.5F;
    private float ActualTime;
    private int HealAmount = 20;
    private int DamageAmount = 10;

    public Color _DamageColor;
    public Color _HealColor;

    private List<GameObject> inDarkness = new List<GameObject> { };


    // Use this for initialization
    void Start()
    {
        ActualTime = 0.0F;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //Creating a list of the Gameobjects in the Darkness Collider
    private void OnTriggerEnter(Collider other)
    {
        inDarkness.Add(other.gameObject);
    }

   public void InLight (GameObject other)
    {
        inDarkness.Remove(other);
    }

    public void ExitLight(GameObject other)
    {
        inDarkness.Add(other);
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
