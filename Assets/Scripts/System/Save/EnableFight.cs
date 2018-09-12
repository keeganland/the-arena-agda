using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableFight : MonoBehaviour
{

    SaveManager saveManager;

    public List<GameObject> Fights;

    [SerializeField] int fightNumber;

    private void Awake()
    {
        saveManager = FindObjectOfType<SaveManager>();
    }

    public void EnableNextFight()
    {
        fightNumber = saveManager.fightNumberAvailable;

        if(Fights[fightNumber].activeSelf == false)
        {
            Fights[fightNumber].SetActive(true);
            if(!saveManager.fights.Contains(Fights[fightNumber].name))
            {
                saveManager.fights.Add(Fights[fightNumber].name);
                fightNumber += 1;
                saveManager.fightNumberAvailable = fightNumber;
            }
        }

        for (int i = 0; i < saveManager.fights.Count; i++)
        {
            Fights[i].SetActive(true);
        }

    }
}
