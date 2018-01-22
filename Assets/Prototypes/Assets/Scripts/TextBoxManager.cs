/**
 * This file is designed after gamesplusjames' Unity tutorials on YouTube. 
 * https://www.youtube.com/watch?v=ehmBIP5sj0M
 * See in particular the 5:40 mark
 * 
 * We create an empty game object called TextImporter, then attach this script as a component.
 * This allows us to pass the game object a text file to use as a text asset. 
 * 
 * I may want to replace this with a general "UI manager"
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Need this for UI object types (such as Text) from Unity 5+

public class TextBoxManager : MonoBehaviour
{
    //As of yet most of these are unused.

    //these are game objects and unity stuff
    public GameObject textBox;
    public Text boxContent;

    //these exist for the management of the external .txt file
    public TextAsset textFile;
    public string[] textLines;

    //these refer to particular lines in the text file
    public int currentLine;
    public int endAtLine;

    //gamesplusjames has a PlayerController data type here. But his player char obviously works pretty different?
    //he even explicitly says. It will be in there to prevent the player from moving during dialogue

    public bool isActive; //if active, the player may use enter to increment lines and other ways of interacting with the text box
    public bool stopPlayerMovement; //not going to use this until I figure out how to deal with there being two players

    /**
     * Necessary for stopping player movement:
     * Modifications to the Player_Movement.cs file. We need some reference to the player IN this file
     */

    // Use this for initialization
    void Start()
    {
        //todo: initialize player variable HERE if we're going to do things like freeze the player during dialogue

        if (textFile != null) //ensure that the text file actually exists
        {
            textLines = (textFile.text.Split('\n')); //Keegan NTS: weird this is valid syntax- i have never used round brackets () like that?
        }

        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }

        if (isActive)
        {
            EnableTextBox();
        }
        else
        {
            DisableTextBox();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(!isActive)
        {
            return;
        }

        boxContent.text = textLines[currentLine];

        if(Input.GetKeyDown(KeyCode.Return))
        {
            currentLine += 1; //the way things work at the moment is just increment through the array
            if (currentLine > endAtLine)
            {
                DisableTextBox();
            }
        }
    }

    public void EnableTextBox()
    {
        textBox.SetActive(true);
        isActive = true;

        /**
         * Pseudocode
         * 
         * if (stopPlayerMovement) {
         *  player . canMove = false
         * }
         */

    }

    public void DisableTextBox()
    {
        textBox.SetActive(false);
        isActive = false;

        /**
         * player. canMove = true
         */
    }

    public void ReloadScript(TextAsset theText)
    {
        if (theText != null)
        {
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));
        }
    }
}
