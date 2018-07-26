using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnMouseHover : MonoBehaviour {

    Image image;

	// Use this for initialization
	void Start () 
    {
        image = GetComponent<Image>();
	}

    public void MouseOver()
    {
        image.enabled = true;
    }

    public void MouseExit()
    {
        image.enabled = false;
    }
}
