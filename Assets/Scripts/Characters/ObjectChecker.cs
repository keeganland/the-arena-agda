using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChecker : MonoBehaviour {

    public List<string> tags;

    [SerializeField] List<GameObject> m_targets = new List<GameObject>();

    //creates list of all tags within the attack range
    private void OnTriggerEnter(Collider other)
    {
        bool invalid = true;

        for (int i = 0; i < tags.Count; i++)
        {
            if (other.CompareTag(tags[i]))
            {
                invalid = false;
                break;
            }
        }
        if (invalid)
            return;

        m_targets.Add(other.gameObject);
        //Debug.Log("RangeChecker/Added " + other.name);
    }

    //removes targets that exit the attack range
    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < m_targets.Count; i++)
        {
            if (other.gameObject == m_targets[i])
            {
                m_targets.Remove(other.gameObject);
                return;
            }
        }
    }

    //Good idea, and ideally we could implement it. But with it on every character it overwhelms the processor, perhaps possible to use if we can simplify it
    /*private void OnTriggerStay(Collider other)
    {
        bool InList = false;
        for (int i = 0; i < m_targets.Count; i++)
        {
            if(other == m_targets[i])
            {
                InList = true;
            }
        }
        if (InList == false && other.CompareTag(tags[0])) //we must only have 1 tag
        {
            m_targets.Add(other.gameObject);
        }
    }*/

    //fetches list of targets
    public List<GameObject> GetValidTargets()
    {
        return m_targets;
    }

    public void ResetList()
    {
        m_targets.Clear();
    }
    //checks if a specific target is within the attack range list
    public bool InRange(GameObject go)
    {
        //Debug.Log("[RangeChecker][InRange] checking1 for: " + go.name);
        for (int i = 0; i < m_targets.Count; i++)
        {
            //Debug.Log("[RangeChecker][InRange] checking2 for: " + go.name);
            if (go == m_targets[i])
                return true;
        }
        //Debug.Log("[RangeChecker][InRange] returning false for " + go.name);
        return false;
    }

    private void Update()
    {
        for (int i = 0; i < m_targets.Count; i++)
        {
            if (m_targets[i].gameObject == null || m_targets[i].GetComponent<Collider>().enabled == false)
            {
                m_targets.Remove(m_targets[i].gameObject);
            }
        }
    }
}
