using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableFight : MonoBehaviour
{

    SaveManager saveManager;

    public List<GameObject> Fights;

    private void Awake()
    {
        saveManager = FindObjectOfType<SaveManager>();
        EnableNextFight(0);
    }

    public void EnableNextFight(int fightNumber)
    {
        if(Fights[fightNumber].activeSelf == false)
        {
            Fights[fightNumber].SetActive(true);
            if(!saveManager.fights.Contains(Fights[fightNumber].name))
            {
                saveManager.fights.Add(Fights[fightNumber].name);            
                saveManager.fightNumberAvailable = fightNumber;   
            }
        }

        for (int i = 0; i < saveManager.fights.Count; i++)
        {
            Fights[i].SetActive(true);
        }

    }
}
