using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsText : MonoBehaviour {

    public GameObject Player;

    public Text HP;
    public Text DAMAGES;
    public Text RANGE;
    public Text HEALING;

    public Text HP2;
    public Text DAMAGES2;
    public Text RANGE2;
    public Text HEALING2;

    // Use this for initialization
    private void OnEnable () 
    {
        UpdateStatsTextUI();
        EventManager.StartListening("RefreshInventoryUI",UpdateStatsTextUI);
	}

    private void OnDisable()
    {
        EventManager.StopListening("RefreshInventoryUI", UpdateStatsTextUI);   
    }

    public void UpdateStatsTextUI()
    {
        if(HP)
        {
            HP.text = "HP : " + Player.GetComponent<HealthController>().currentHealth + "/" + Player.GetComponent<HealthController>().totalHealth;
            HP2.text = HP.text;
        }
        if(DAMAGES)
        {
            DAMAGES.text = "Damages : " + Player.GetComponent<MeleeDamage>().Damage;
            DAMAGES2.text = DAMAGES.text;

        }
        if(RANGE)
        {
            RANGE.text = "Range : " + Player.GetComponentInChildren<RangeChecker>().gameObject.GetComponent<SphereCollider>().radius.ToString();
            RANGE2.text = RANGE.text;

        }
        if(HEALING)
        {
            HEALING.text = "Healing : " + Player.GetComponent<SpellCommand>().Healing;
            HEALING2.text = HEALING.text;

        }
    }
}
