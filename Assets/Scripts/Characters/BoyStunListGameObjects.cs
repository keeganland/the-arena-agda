using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyStunListGameObjects : MonoBehaviour {

    public List<GameObject> EnemiesList;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            if(!EnemiesList.Contains(other.gameObject))
            {
                EnemiesList.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            if (EnemiesList.Contains(other.gameObject))
            {
                EnemiesList.Remove(other.gameObject);
            }
        }
    }

    public void ResetList()
    {
        EnemiesList.Clear();
    }
}
