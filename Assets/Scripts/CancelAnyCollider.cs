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
        }

        if (!saveManager.cancelColliders.Contains(name))
            saveManager.cancelColliders.Add(name);
    }

    private void ChangeCollider()
    {
        if (GetComponent<SphereCollider>() == true)
            GetComponent<SphereCollider>().enabled = SphereActivated;
        if (GetComponent<CapsuleCollider>() == true)
            GetComponent<CapsuleCollider>().enabled = CapsuleActivated;
    }
}
