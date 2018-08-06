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
using System.Xml;
using UnityEngine;
using UnityEngine.UI; //Need this for UI object types (such as Text) from Unity 5+

public class TextBoxManager : MonoBehaviour
{

    //these are game objects and unity stuff
    public GameObject textBox;
    public GameObject namePlate;
    public GameObject interactivityCue; //Alex : I don't really know what it does but I suppose it's like a choice box for the player : (Yes/No)??
                                        //I will use it as if it is (and create a bool eventAtEndofText too.
    public Text boxContent;
	public Text NPCNameTag;

    public GameObject TextBallon;
    private GameObject NPCGameObject; //Alex : Get the NPC position to Spawn the "Text" Ballon.
    private GameObject textBallon;

    //these exist for the management of the external .txt file
    public TextAsset textFile;
    public TextAsset xmlDialogFile;
	public string NPCName;
	public string[] textLines;
	public Queue<string> textQueue;
    public bool useXml;

    //these refer to particular lines in the text file because we're using a string array for some dumb reason
    public int currentLine;
    public int endAtLine;

    public MovementManager movementManager;
    public NPCMovementManager theNPCMovementManager;

    //public bool isActive;
    public bool cueActive = false;
    public bool stopPlayerMovement;
    public bool stopNPCMovement;
    public bool eventAtEndofText;

    private bool isTyping = false;
    private bool cancelTyping = false;
	private bool isActive = false;
	//if active, the player may use enter to increment lines and other ways of interacting with the text box. Maybe switch to private
	private string lastTriggeredBy = "";

    public float typeSpeed;

    PauseMenu pauseMenu;
    // Use this for initialization
    void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
		/**
		 * Keegan NTS: Initialize the script. Lots of redundancy with the Reload method. Revisit plz
		 */

		textQueue = new Queue<string> ();

		//to be replaced with something that parses XML

        if (useXml)
        {
            ReloadScriptXML(xmlDialogFile);
        }
        else //deprecate once xml is firmly in place
        {
            if (textFile != null) //ensure that the text file actually exists
            {
                textLines = (textFile.text.Split('\n')); //Keegan NTS: weird that this is valid syntax- i have never used round brackets () like that?
            }

            if (endAtLine == 0)
            {
                endAtLine = textLines.Length - 1;
            }

            for (int i = 0; i < textLines.Length; i++)
            {
                textQueue.Enqueue(textLines[i]);
            }
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
        if(Input.GetKeyDown(KeyCode.Escape) && isActive)
        {
            DisableTextBox();
            DisableCue();
            pauseMenu.Resume();
        }

        if (!isActive)
        {
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            if (!isTyping)
            {
                currentLine += 1; //the way things work at the moment is just increment through the array. should sooner or later be replaced with a queue
                if(((textQueue.Count == 0 && !eventAtEndofText) || (currentLine > endAtLine && !eventAtEndofText)))
                {
                    DisableTextBox();
                }
                else if (((textQueue.Count == 0 && eventAtEndofText) || (currentLine > endAtLine && eventAtEndofText)))
                {
                    if (interactivityCue.activeSelf == false)
                    {
                        EnableCue();
                    }
                    return;
                }
                else
                {
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
        textBallon = Instantiate(TextBallon, NPCGameObject.transform.position + new Vector3(0,0,1.36f), Quaternion.Euler(90, 0, 0));

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
            EventManager.TriggerEvent("StopMoving");
        }
        if (stopNPCMovement)
        {
            //theNPCMovementManager.StopNPCMovement();
        }
 
		//StartCoroutine(TextScroll(textLines[currentLine]));
		StartCoroutine(TextScroll(textQueue.Dequeue()));
    }

    public void DisableTextBox()
    {
        textBox.SetActive(false);
        isActive = false;
        Destroy(textBallon);
        EventManager.TriggerEvent("StartMoving"); //Alex: I added this line here 'cause I don't know what is "movementManager";

        if (namePlate != null)
        {
            namePlate.SetActive(false);
        }
        if (movementManager != null)
        {
            EventManager.TriggerEvent("StartMoving");
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

    public void setNPCGameObject(GameObject NPC)
    {
        NPCGameObject = NPC;
    }

    public void EnableCue()
    {
        if (interactivityCue)
        {
            interactivityCue.SetActive(true);
            cueActive = true;
        }
    }

    //Alex : I'll just put the exit of the Text (if eventAtEndOfText) here and I'll disable it with the "button"

    public void DisableCue()
    {
        if (interactivityCue)
        {
            interactivityCue.SetActive(false);
            cueActive = false;
        }
    }

	public bool getIsActive()
	{
		return isActive;
	}

	public void setLastTriggered(string gameObjName)
	{
        //Debug.Log (gameObjName + " triggered the TextBoxManager");
		lastTriggeredBy = gameObjName;
	}

	public string getLastTriggered()
	{
		return lastTriggeredBy;
	}

    public void ReloadScriptXML(TextAsset xmlFile)
    {
        textQueue.Clear();
        if (xmlFile != null)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlFile.text);

            foreach (XmlNode xmlNode in xmlDocument["scenes"].ChildNodes)
            {
                foreach (XmlNode line in xmlNode["lines"].ChildNodes)
                {
                    Debug.Log("Enqueuing line: " + line.InnerText);
                    textQueue.Enqueue(line.InnerText);
                }
            }
        }
    }
}
