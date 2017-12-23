/**
 * This file is designed after gamesplusjames' Unity tutorials on YouTube. 
 * https://www.youtube.com/watch?v=ehmBIP5sj0M
 * See in particular the 5:40 mark
 * 
 * We create an empty game object called TextImporter, then attach this script as a component.
 * This allows us to pass the game object a text file to use as a text asset. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Need this for UI object types (such as Text) from Unity 5+

public class TextImporter : MonoBehaviour {
    //As of yet most of these are unused.

    public GameObject textBox;

    public Text theText; // TODO: rename this garbage

    public TextAsset textFile;
    public string[] textLines;

    public int currentLine;
    public int endAtLine;

    //gamesplusjames has a PlayerController data type here. But his player char obviously works pretty different?

	// Use this for initialization
	void Start () {

        
		
        if (textFile != null) //ensure that the text file actually exists
        {
            textLines = (textFile.text.Split('\n')); //Keegan NTS: weird this is valid syntax- i have never used round brackets () like that?
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
