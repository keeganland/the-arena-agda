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

    //these are game objects and unity stuff
    public GameObject textBox;
    public GameObject namePlate;
    public GameObject interactivityCue;

    public Text boxContent;
	public Text NPCNameTag;

    //these exist for the management of the external .txt file
    public TextAsset textFile;
	public string NPCName;
	public string[] textLines;
	public Queue<string> textQueue;

    //these refer to particular lines in the text file because we're using a string array for some dumb reason
    public int currentLine;
    public int endAtLine;

    public MovementManager movementManager;
    public NPCMovementManager theNPCMovementManager;

    //public bool isActive;
    public bool cueActive = false;
    public bool stopPlayerMovement;
    public bool stopNPCMovement;

    private bool isTyping = false;
    private bool cancelTyping = false;
	private bool isActive = false;
	//if active, the player may use enter to increment lines and other ways of interacting with the text box. Maybe switch to private
	private string lastTriggeredBy = "";

    public float typeSpeed;

    // Use this for initialization
    void Start()
    {

		/**
		 * Keegan NTS: Initialize the script. Lots of redundancy with the Reload method. Revisit plz
		 */

		textQueue = new Queue<string> ();

		//to be replaced with something that parses XML
        if (textFile != null) //ensure that the text file actually exists
        {
            textLines = (textFile.text.Split('\n')); //Keegan NTS: weird that this is valid syntax- i have never used round brackets () like that?
        }
			
		for (int i = 0; i < textLines.Length; i++) {
			textQueue.Enqueue(textLines[i]);
		}
		/**
		 * Done
		 */


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

        if (cueActive)
        {
            EnableCue();
        }
        else
        {
            DisableCue();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!isActive)
        {
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.Return))
        {

            if (!isTyping)
            {
                currentLine += 1; //the way things work at the moment is just increment through the array. should sooner or later be replaced with a queue
                //if (currentLine > endAtLine)
				if((textQueue.Count == 0) || (currentLine > endAtLine))
                {
                    DisableTextBox();
                }
                else
                {
                    //StartCoroutine(TextScroll(textLines[currentLine]));
					StartCoroutine(TextScroll(textQueue.Dequeue()));
                }
            }

            else if (isTyping && !cancelTyping)
            {
                cancelTyping = true;
            }
        }
    }

    private IEnumerator TextScroll (string lineOfText)
    {
        int letter = 0;
        boxContent.text = "";
        isTyping = true;
        cancelTyping = false;
        while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            boxContent.text += lineOfText[letter];
            letter += 1;
            yield return new WaitForSeconds(typeSpeed);
        }
        boxContent.text = lineOfText; //when the update loop breaks the while loop by making cancelTyping false, we want to make sure that the whole text line is displayed
        isTyping = false;
        cancelTyping = false;
    }


    public void EnableTextBox()
    {
        textBox.SetActive(true);
		if (NPCNameTag != null) {

            NPCNameTag.text = NPCName;

            //The nameplate background for the name text only shows up if it exists as a gameobject, obviously, but also only if the NPC name isn't a blank string
            if (namePlate != null && NPCNameTag.text != "")
            {
                namePlate.SetActive(true);
            }
		}
        isActive = true;

        if (stopPlayerMovement)
        {
            movementManager.StopPlayerMovement();
        }
        if (stopNPCMovement)
        {
            theNPCMovementManager.StopNPCMovement();
        }
 
		//StartCoroutine(TextScroll(textLines[currentLine]));
		StartCoroutine(TextScroll(textQueue.Dequeue()));
    }

    public void DisableTextBox()
    {
        textBox.SetActive(false);
        isActive = false;
        if (namePlate != null)
        {
            namePlate.SetActive(false);
        }
        if (movementManager != null)
        {
            movementManager.StartPlayerMovement();
        }
        if (theNPCMovementManager != null)
        {
            theNPCMovementManager.StartNPCMovement();
        }
    }

    public void ReloadScript(TextAsset theText)
    {
        if (theText != null)
        {
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));
        }

		textQueue.Clear();
		for (int i = 0; i < textLines.Length; i++) 
		{
			textQueue.Enqueue(textLines[i]);
		}	
	}

	public void setNPCName(string newName)
	{
		NPCName = newName;
	}

    public void EnableCue()
    {
        interactivityCue.SetActive(true);
        cueActive = true;
    }

    public void DisableCue()
    {
        interactivityCue.SetActive(false);
        cueActive = false;
    }

	public bool getIsActive()
	{
		return isActive;
	}

	public void setLastTriggered(string gameObjName)
	{
		Debug.Log (gameObjName + " triggered the TextBoxManager");
		lastTriggeredBy = gameObjName;
	}

	public string getLastTriggered()
	{
		return lastTriggeredBy;
	}
}
