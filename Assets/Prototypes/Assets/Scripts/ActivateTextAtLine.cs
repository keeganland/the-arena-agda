using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour {

    public TextAsset theText;

    public int startLine;
    public int endLine;

    public TextBoxManager theTextManager;

    public bool requireButtonPress;
    public bool waitForPress;


    public bool destroyWhenActivated;


	// Use this for initialization
	void Start () {
        theTextManager = FindObjectOfType<TextBoxManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (waitForPress && Input.GetKeyDown(KeyCode.Return))
        {
            theTextManager.ReloadScript(theText);
            theTextManager.currentLine = startLine;
            theTextManager.endAtLine = endLine;
            theTextManager.EnableTextBox();

            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Boy")
        //if (other.CompareTag("Player"))
        {
            if(requireButtonPress)
            {
                waitForPress = true;
                return;
            }

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

    void OnTriggerExit(Collider other)
    {
        if(other.name == "Boy")
        {
            waitForPress = false;
        }
    }
}
