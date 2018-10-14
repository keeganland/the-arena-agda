using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelAnyCollider : MonoBehaviour
{
    SaveManager saveManager;

    public bool SphereActivated;
    public bool CapsuleActivated;

    private void Awake()
    {
        saveManager = FindObjectOfType<SaveManager>();
    }

    private void Start()
    {
        if (saveManager.cancelColliders.Contains(name))
        {
            ChangeCollider();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.name == "Girl")
       {
            ChangeCollider();
            other.GetComponentInChildren<RangeChecker>().ResetList();
       }

        if (!saveManager.cancelColliders.Contains(name))
            saveManager.cancelColliders.Add(name);
    }

    private void ChangeCollider()
    {
        if (GetComponent<SphereCollider>())
            Destroy(GetComponent<SphereCollider>());
        if (GetComponent<CapsuleCollider>())
            GetComponent<CapsuleCollider>().enabled = CapsuleActivated;


    }
}
