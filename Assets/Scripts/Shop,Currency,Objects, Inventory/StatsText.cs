using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsText : MonoBehaviour {

    public GameObject Player;

    public Text HP;
    public Text DAMAGES;
    public Text RANGE;
    public Text ATTACKSPEED;

    public Text HP2;
    public Text DAMAGES2;
    public Text RANGE2;
    public Text ATTACKSPEED2;

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
            HP.text = "HP : " + ("<color=lime>" + Player.GetComponent<HealthController>().currentHealth + "/" + Player.GetComponent<HealthController>().totalHealth + "</color>");
            HP2.text = HP.text;
        }
        if(DAMAGES)
        {
            DAMAGES.text = "Damages : " + ("<color=lime>" + Player.GetComponent<MeleeDamage>().Damage + "</color>");
            DAMAGES2.text = DAMAGES.text;

        }
        if(RANGE)
        {
            RANGE.text = "Range : " + ("<color=lime>" + Player.GetComponentInChildren<RangeChecker>().gameObject.GetComponent<SphereCollider>().radius.ToString() + "</color>");
            RANGE2.text = RANGE.text;

        }
        if(ATTACKSPEED)
        {
            ATTACKSPEED.text = "Attack Speed : " + ("<color=lime>" + 5 /Player.GetComponent<MeleeDamage>().AttackSpeed + "</color>");
            ATTACKSPEED2.text = ATTACKSPEED.text;

        }
    }
}
