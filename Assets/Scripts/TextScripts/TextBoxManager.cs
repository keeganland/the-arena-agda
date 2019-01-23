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
using UnityEngine.SceneManagement;

public class TextBoxManager : MonoBehaviour
{

    private Scene neverUnload;
    private GameObject dialogCanvas;

    //these are game objects and unity stuff
    private GameObject textBox;
    private GameObject namePlate;
    private GameObject interactivityCue;
    private GameObject dialogPrompt;

    public Text boxContent;
	public Text npcNameTag;

    public GameObject TextBallon;
    private GameObject npcGameObject; //Alex : Get the NPC position to Spawn the "Text" Ballon.
    private GameObject textBallon;

    //these exist for the management of the external .txt file
    public TextAsset textFile;
    public TextAsset xmlDialogFile;
	public string npcName;
	public string[] textLines;
	public Queue<string> textQueue;
    public bool useXml;

    //these refer to particular lines in the text file because we're using a string array for some dumb reason
    //public int currentLine;
    //public int endAtLine;

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

    public AudioClip TextScrollSFX;
    public float SoundScaleFactor;
    private AudioSource m_audioSource;
    


    #region Singleton Stuff
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
    private void Init()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
        //TODO: Move a lot of what was in Start, etc. in here
    }
    #endregion

    #region Inherited from MonoBehaviour
    private void Awake()
    {

        //Depends on our hierarchy being laid out just-so. Would be better to do it some other way, but I need to design that.
        List<GameObject> neverUnloadRootObjects = new List<GameObject>();
        Scene neverUnload = SceneManager.GetSceneByName("NeverUnload");
        neverUnload.GetRootGameObjects(neverUnloadRootObjects);
        dialogCanvas = neverUnloadRootObjects.Find(x => x.name == "DialogCanvas");
        textBox = dialogCanvas.transform.Find("DialogTextbox").gameObject;
        namePlate = textBox.transform.Find("NamePlate").gameObject;
        dialogPrompt = textBox.transform.Find("DialogYesNoPrompt").gameObject;
        //boxContent = textBox.transform.Find("DialogText").gameObject;



    }
    private void Start()
    {
        /**
		 * Keegan NTS: Initialize the script. Lots of redundancy with the Reload method. Revisit plz
		 */

        m_audioSource = GetComponent<AudioSource>();
        SoundManager.onSoundChangedCallback += UpdateSound;

        textQueue = new Queue<string> ();

		//to be replaced with something that parses XML

        if (useXml)
        {
            ReloadScriptXML(xmlDialogFile);
        }
        else
        {
            if (textFile != null) //ensure that the text file actually exists
            {
                textLines = (textFile.text.Split('\n')); //Keegan NTS: weird that this is valid syntax- i have never used round brackets () like that?
            }

            for (int i = 0; i < textLines.Length; i++)
            {
                textQueue.Enqueue(textLines[i]);
            }
        }
    }
    private void Update()
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
                //currentLine += 1; //the way things work at the moment is just increment through the array. should sooner or later be replaced with a queue
                //if(((textQueue.Count == 0 && !eventAtEndofText) || (currentLine > endAtLine && !eventAtEndofText)))
                if(textQueue.Count == 0 && !eventAtEndofText)
                {
                    DisableTextBox();
                }
                else if (textQueue.Count == 0 && eventAtEndofText)
                {
                    if (dialogPrompt == null || dialogPrompt.activeSelf == false) 
                    {
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
    #endregion

    private IEnumerator TextScroll (string lineOfText)
    {
        int letter = 0;
        boxContent.text = "";
        isTyping = true;
        cancelTyping = false;

        bool playSound = true; //silly thing to make the sound play less
        while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            boxContent.text += lineOfText[letter];
            if (playSound)
            {
                m_audioSource.PlayOneShot(TextScrollSFX);
                playSound = false;
            }
            else
            {
                playSound = true;
            }            
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
            textBallon = Instantiate(TextBallon, npcGameObject.transform.position + new Vector3(0,0,1.36f), Quaternion.Euler(90, 0, 0));

		if (npcNameTag != null) {

            npcNameTag.text = NPCName;

            //The nameplate background for the name text only shows up if it exists as a gameobject, obviously, but also only if the NPC name isn't a blank string
            if (namePlate != null && npcNameTag.text != "")
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
            foreach(Sprite sp in sprites)
            {
                if(sp.name == SpriteNameInSheet)
                {
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

    public GameObject InteractivityCue
    {
        get
        {
            return interactivityCue;
        }
        set
        {
            interactivityCue = value;
        }
    }

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
    public void PlayerAnswersYes()
    {
        Debug.Log("Player clicked Yes!");
        this.DisableDialogPrompt();
        EventManager.TriggerEvent("answersYes");
    }
    public void PlayerAnswersNo()
    {
        Debug.Log("Player clicked No!");
        this.DisableDialogPrompt();
        EventManager.TriggerEvent("answersNo");
    }
    void UpdateSound()
    {
        m_audioSource.volume = (SoundManager.SFXVolume * SoundScaleFactor) / 100;
    }

    #region Properties
    public string NPCName
    {
        get
        {
            return npcName;
        }
        set
        {
            npcName = value;
        }
    }
    public GameObject NPCGameObject
    {
        get
        {
            return npcGameObject;
        }
        set
        {
            npcGameObject = value;
        }

    }
    public string LastTriggered
    {
        get
        {
            return lastTriggeredBy;
        }
        set
        {
            lastTriggeredBy = value;
        }
    }
    public bool IsActive
    {
        get
        {
            return isActive;
        }
        set
        {
            isActive = value;
        }
    }
    #endregion

}
