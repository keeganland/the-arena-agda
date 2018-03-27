using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemDestroyed : MonoBehaviour {

    public bool _IsDestroyed;

    private void Update()
    {
        if(_IsDestroyed == true)
        {
            gameObject.SetActive(false);
        }
    }
}
