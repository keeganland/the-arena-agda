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


 /**
  * Todo 9/21: turn this into a persistent singleton object for NeverUnload
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
    public GameObject dialogPrompt;

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
    //public bool cueActive = false;
    public bool stopPlayerMovement;
    public bool stopNPCMovement;
    public bool eventAtEndofText;
    public bool eventStart;

    private bool isTyping = false;
    private bool cancelTyping = false;
	private bool isActive = false;
	//if active, the player may use enter to increment lines and other ways of interacting with the text box. Maybe switch to private
	private string lastTriggeredBy = "";

    public float typeSpeed;

    private string SpriteSheet;
    private string SpriteNameInSheet;
    public SpriteRenderer SpriteHolder;
    public GameObject SpriteHolderGameObject;

    PauseMenu pauseMenu;


    //Holds the single object for singleton design pattern
    public static TextBoxManager textBoxManager;


    public static TextBoxManager Instance
    {
        get
        {
            if(!textBoxManager)
            {
                textBoxManager = FindObjectOfType(typeof(TextBoxManager)) as TextBoxManager;

                if(!textBoxManager)
                {
                    Debug.LogError("There needs to be one active TextBoxManager script on a GameObject in your scene.");
                }
                else
                {
                    textBoxManager.Init();
                }
            }
            return textBoxManager;
        }
    }

    void Init()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
        //TODO: Move a lot of what was in Start, etc. in here
    }




    void Start()
    {
        //pauseMenu = FindObjectOfType<PauseMenu>(); // moved to Init
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


        /*
         * Keegan 2018/9/21- as far as i know, the below existed strictly for testing through the Unity inspector
         * 
         * I cannot think of any other circumstance in which they'd actually be relevant
         */


        /*
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
        */
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
                    if (dialogPrompt == null || dialogPrompt.activeSelf == false) 
                    {
                        //EnableCue();
                        EnableDialogPrompt();
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
        if(!textBallon)
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

        if (SpriteNameInSheet != "" && SpriteHolder)
        {
            SpriteHolderGameObject.SetActive(true);
            Sprite[] sprites = Resources.LoadAll<Sprite>(SpriteSheet);
            Debug.Log(sprites.Length);
            foreach(Sprite sp in sprites)
            {
                Debug.Log(SpriteNameInSheet);
                if(sp.name == SpriteNameInSheet)
                {
                    Debug.Log(sp.name);
                    SpriteHolder.sprite = sp;
                }
            }
        }
        else if(SpriteHolderGameObject){
            SpriteHolderGameObject.SetActive(false);
            SpriteHolder.sprite = null; 
        }
 
		//StartCoroutine(TextScroll(textLines[currentLine]));
		StartCoroutine(TextScroll(textQueue.Dequeue()));
    }

    public void DisableTextBox()
    {
        textBox.SetActive(false);
        isActive = false;
        Destroy(textBallon);

        /*
        if(NPCGameObject)
            NPCGameObject.GetComponent<ActivateTextAtLine>().ResetText(); //Alex : Not sure it's the best way to reset the text. 
            //It's not. 
        */
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


    /*
     * 2018/10/13 - Should to be turned back into providing "Press Enter" type cues in order to defend against some possible regressions. Leave for now.
     * Consider restoring cueActive for those purposes
     */
    public void EnableCue()
    {
        if (interactivityCue)
        {
            interactivityCue.SetActive(true);
            //cueActive = true;
        }
        else
        {
            eventStart = true;
            DisableTextBox();
        } 
    }

    public void EnableDialogPrompt()
    {
        if (dialogPrompt)
        {
            dialogPrompt.SetActive(true);
        }
        else
        {
            eventStart = true;
            DisableTextBox();
        }
    }



    public void SetInteractivityCue(GameObject cue)
    {
        interactivityCue = cue;
    }
    //Alex : I'll just put the exit of the Text (if eventAtEndOfText) here and I'll disable it with the "button"

    public void DisableCue()
    {
        if (interactivityCue)
        {
            interactivityCue.SetActive(false);
            //cueActive = false;
        }
    }

    public void DisableDialogPrompt()
    {
        if (dialogPrompt)
        {
            dialogPrompt.SetActive(false);
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

    public void SetSprite(string spriteSheetName, string spriteNameInSheet)
    {
        SpriteSheet = spriteSheetName;
        SpriteNameInSheet = spriteNameInSheet;
    }
}
