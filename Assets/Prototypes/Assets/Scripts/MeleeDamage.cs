using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour {

    public int Damage;
    public int Aggro;
    public float AttackSpeed;
    public GameObject _Sprite;
    private float AttackTimer;
    public GameObject m_target;

    //-----------------Alex Modifications-----------//
    [SerializeField]
    private GameObject[] spellPrefab;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("MeleeDamage: Damage Target" + m_target.name);
        AttackTimer += Time.deltaTime; //including this although I'm not sure I have to. Should the animation Timer do this for me?
        if(AttackTimer >= AttackSpeed && (this.GetComponentInChildren<RangeChecker>().InRange(m_target)) && m_target!=null)
        {
            DamageData dmgData = new DamageData();
            dmgData.damage = Damage;
            CastSpell();
            

            //Debug.Log("MeleeDamage: deal damage to " + m_target.name);

            /*AggroData aggroData = new AggroData();
            aggroData.aggro = Aggro;*/
            //Debug.Log("MeleeDamage: change Aggro of " + m_target.name);

            //Really need to make sure MessageHandler is on all enemies and players
            MessageHandler msgHandler = m_target.GetComponent<MessageHandler>();

            if (msgHandler)
            {
                msgHandler.GiveMessage(MessageTypes.DAMAGED, this.gameObject, dmgData);
                //msgHandler.GiveMessage(MessageTypes.AGGROCHANGED, this.gameObject, aggroData);
                //Debug.Log("MeleeDamage: this = " + this.name);
            }
            AttackTimer = 0.0f;
        }
    }

    public void TargetChanges(GameObject target)
    {
        m_target = target;
    }
    
    public void CastSpell()
    {
        Vector3 direction = m_target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        transform.LookAt(m_target.transform);

        int rotation = 0;

        if ( 45 <= transform.eulerAngles.y && transform.eulerAngles.y < 135 )
        {
            rotation = 3;
        }
        else if (0 <= transform.eulerAngles.y && transform.eulerAngles.y < 45)
        {
            Debug.Log("here");
            rotation = 1;
        }
        else if (225 <= transform.eulerAngles.y && transform.eulerAngles.y < 315)
        {
            rotation = 4;
        }
        else
        {
            rotation = 2;
        }

        if (rotation != 0)
        {
            _Sprite.GetComponent<SpriteScript2>().ForcePlayerRotation(rotation);
        }
        GameObject go = Instantiate(spellPrefab[0], transform.position, Quaternion.Euler(0, -angle, 0));
        go.gameObject.GetComponent<Spell>().SetTarget(m_target);
    }

    
}
