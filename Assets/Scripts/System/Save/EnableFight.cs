using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableFight : MonoBehaviour {

    SaveManager saveManager;

    public List<GameObject> Fights;

    int fightNumber = 0;

    private void Awake()
    {
        saveManager = FindObjectOfType<SaveManager>();
        fightNumber = saveManager.fightNumber;
    }

    private void Start()
    {
        for (int i = 0; i < saveManager.fights.Count; i++)
        {
            Fights[i].SetActive(true); 
        }
    }

    public void EnableNextFight()
    {
        if(Fights[fightNumber].activeSelf == false)
        {
            Fights[fightNumber].SetActive(true);
            if(!saveManager.fights.Contains(Fights[fightNumber].name))
            {
                saveManager.fights.Add(Fights[fightNumber].name);
                fightNumber += 1;
                saveManager.fightNumber = fightNumber; 
            }
        }
    }
}
