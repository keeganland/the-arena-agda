using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour {

    public TextAsset theText;

    public int startLine;
    public int endLine;

    public TextBoxManager theTextManager;

    public bool destroyWhenActivated;


	// Use this for initialization
	void Start () {
        theTextManager = FindObjectOfType<TextBoxManager>();


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {

        if((other.name == "Girl") || (other.name == "Boy"))
        {
            theTextManager.ReloadScript(theText);
            theTextManager.currentLine = startLine;
            theTextManager.endAtLine = endLine;
            theTextManager.EnableTextBox();

            if(destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }
    }
}
