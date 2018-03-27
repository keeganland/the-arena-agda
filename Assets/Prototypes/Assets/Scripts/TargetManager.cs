using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public enum TargetOptions { WEAKEST, STRONGEST, NEAREST, FARTHEST, RANDOM, AGGRO }
    public TargetOptions targetOption /*= TargetOptions.NEAREST*/;//This was in the example script, don't think I need it

    TargetRangeChecker m_range;
    EnemyMovement m_enemyM;
    float closestDistance = 0.0f;
    float farthestDistance = 0.0f;

    //set this up to get rid of error. would like to pass this into targetmanager instead
    //this should be private, making public to try to fix error in enemymovement
    public GameObject curTarget = null;

    // Use this for initialization
    void Start()
    {
        m_range = GetComponentInChildren<TargetRangeChecker>();
        m_enemyM = GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //check to make sure there's something in range.
        if (!m_range)
            return;

        switch (targetOption)
        {
            case TargetOptions.FARTHEST:
                TargetFarthest();
                break;
            case TargetOptions.NEAREST:
                TargetNearest();
                break;
            case TargetOptions.RANDOM:
                TargetRandom(curTarget);
                break;
            case TargetOptions.STRONGEST:
                TargetStrongest();
                break;
            case TargetOptions.WEAKEST:
                TargetWeakest();
                break;
            case TargetOptions.AGGRO:
                TargetAggro();
                break;
        }
    }
    private void TargetFarthest()
    {
        List<GameObject> validTargets = m_range.GetValidTargets();
        bool curTargetinRange;

        for(int i =0; i<validTargets.Count; i++)
        {
            curTargetinRange = false;
            if(validTargets[i] == curTarget)
            {
                curTargetinRange = true;
                break;
            }
            if(curTargetinRange == false)
            {
                curTarget = null;
            }
        }
        if(validTargets.Count == 0)
        {
            curTarget = null;
        }
        for(int i = 0; i < validTargets.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, validTargets[i].transform.position);
            if(!curTarget || dist > farthestDistance || curTarget == validTargets[i])
            {
                curTarget = validTargets[i];
                farthestDistance = dist;
            }
        }
        m_enemyM.TargetSet(curTarget);
        Debug.Log("TargetManager/FarthestTarget: " + curTarget.name);
    }
    private void TargetNearest()
    {
        List<GameObject> validTargets = m_range.GetValidTargets();

        for (int i = 0; i < validTargets.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, validTargets[i].transform.position);
           // Debug.Log(dist);
            if (!curTarget || dist < closestDistance || curTarget == validTargets[i])
            {
                //Debug.Log("in loop");
                curTarget = validTargets[i];
                closestDistance = dist;
                Debug.Log(closestDistance);
            }
        }
        m_enemyM.TargetSet(curTarget);
        //Debug.Log("TargetManager/NearestTarget: " + curTarget.name);
        //Debug.Log(validTargets.Count);
    }
    private void TargetRandom(GameObject go)
    {
        if (!go)
        {
            List<GameObject> validTargets = m_range.GetValidTargets();//only want trigger stay on this one
            int num = Random.Range(0, validTargets.Count);
            curTarget = validTargets[num];
        }
        m_enemyM.TargetSet(curTarget);
        if (curTarget != null)
        {
            Debug.Log("TargetManager/RandomTarget: " + curTarget.name);
        }
    }
    private void TargetStrongest()
    {
        List<GameObject> validTargets = m_range.GetValidTargets();

        int highestHealth = 0;

        for(int i = 0; i < validTargets.Count; i++)
        {
            int hp = validTargets[i].GetComponent<HealthController>().totalHealth;
            if(!curTarget || hp > highestHealth)
            {
                highestHealth = hp;
                curTarget = validTargets[i];
            }
        }
        m_enemyM.TargetSet(curTarget);
        Debug.Log("TargetManager/StrongestTarget: " + curTarget.name);
    }
    private void TargetWeakest()
    {
        List<GameObject> validTargets = m_range.GetValidTargets();

        int lowestHealth = 0;

        for (int i = 0; i < validTargets.Count; i++)
        {
            int hp = validTargets[i].GetComponent<HealthController>().totalHealth;
            if (!curTarget || hp < lowestHealth)
            {
                lowestHealth = hp;
                curTarget = validTargets[i];
            }
        }
        m_enemyM.TargetSet(curTarget);
        Debug.Log("TargetManager/WeakestTarget: " + curTarget.name);
    }
    private void TargetAggro()
    {
        List<GameObject> validTargets = m_range.GetValidTargets();

        int highestAggro = 0;

        for(int i = 0; i <validTargets.Count; i++)
        {
            int Aggro = 1; /*something here to pull Aggro of players*/
            if(!curTarget || Aggro > highestAggro)
            {
                highestAggro = Aggro;
                curTarget = validTargets[i];
            }
        }
    }
}


//Need to revamp this is order to include the Aggro in the Enemy target settings
//First need to create aggro. What makes it?
//      Attacks, healing, abilities
//          Therefore needs to be some kind of a slider(?) value
//          Order is (from most affect to least): Abilities, Healing, Attacks
//          Note: more powerful Attacks do more damage
//          Should only be implemented ince we have abilities set up?
//Each attack increases Aggro of that enemy on that player
//How should healing work? Increases Aggro for all players?
//Abilities to affect Aggro of all enemies and only some enemies?
//Will this make it too complicated?
//      Either for the player or for us as developers
//Should probably just count up and not bother counting down at all
//May run into problems regarding enemies alternating back and forth between two players if they are far apart and never attacking
//Maybe better to call it threat/hate to avoid blatantly copying FF14
//Need to decide how we want to show enmity (if we do at all)
//      Probably don't (hidden mechanic, not core to gameplay)
//Could be an interesting mechanic to summon a dummy with high emnity/threat but low HP which would only last for a while
//Interesting to add held items beyond just armour which impact/increase threat
//FF14: 1 point damage = 1 point enmity, 1 point healed = 0.5 point enmity, skill use = 1 enmity
//      Overhealing might generate up to 6 points enmity per 1 point healed
//          Probs not true but interesting mechanic
//      Some enemies will attack target regardless of enmity
//Dragon Age Inquisition Mechanics:
//      When players are seen, all have base threat of 10
//          Heavy armour increases threat by 10
//          Light armour increases threat by 5
//      After initial threat, distance threat is added
//          Closer = more threat
//          Threat change per distance is marginally smaller at closer distances
//              Makes sense as this slightly limits the advantage of ranged characters
//      Then begin drawing threat
//          Damage threat:
//              Threat increase = 100 * damagedealt/maxhealth of enemy
//              Less effective against bosses with 1000s of health points
//      Includes a threat decay of 0.5 threat per second
//      Has max distance of 60m, beyond which there is no threat given
//      At 20% health, enemies have a 50% chance of not switching to a higher threat opponent
//          90% chance for less than 10% health