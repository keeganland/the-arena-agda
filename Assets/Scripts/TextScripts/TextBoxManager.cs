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
using UnityEngine.SceneManagement;

public class TextBoxManager : MonoBehaviour
{
    public GameObject textBalloonPrefab;

    #region Variables for referring to child objects of DialogCanvas
    private Scene neverUnload;
    private GameObject dialogCanvas;
    private GameObject textBox;
    private GameObject namePlate;
    private GameObject interactivityCue; // auto-property doesn't work for read-only
    private GameObject yesNoPrompt;
    private Text boxContent;
	private Text npcNameTag;    
    private GameObject npcGameObject; //Alex : Get the NPC position to Spawn the "Text" Ballon.
    private GameObject textBalloon;



    #endregion
    #region Variables for managing the external .txt file containing the dialogue
    private TextAsset textFile;
	private string[] textLines;
	private Queue<string> textQueue;
    #endregion

    public MovementManager movementManager;
    //public NPCMovementManager theNPCMovementManager;

    //public bool isActive;
    //public bool cueActive = false;
    public bool stopPlayerMovement;


    #region Variables for the text scrolling visual effect
    private bool isTyping = false;
    private bool cancelTyping = false;

    public float typeSpeed;
    #endregion

    private string SpriteSheet;
    private string SpriteNameInSheet;
    public SpriteRenderer SpriteHolder;
    public GameObject SpriteHolderGameObject;

    PauseMenu pauseMenu;

    public AudioClip TextScrollSFX;
    public float SoundScaleFactor;
    private AudioSource m_audioSource;




    #region Properties
    #region Read-Write Properties
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
    public TextAsset TextFile
    {
        get
        {
            return textFile;
        }

        set
        {
            textFile = value;
        }
    }
    public string NpcName { get; set; }
    public string LastTriggered { get; set; }
    public bool EventStart { get; set; } //No idea what this is but Alex calls it a lot
    public bool IsYesNoAtEndOfText { get; set; }
    public bool IsEventAtEndOfText { get; set; }
    public bool IsMultiCharDialog { get; set; }
    #endregion
    #region Read-Only Properties
    public GameObject YesNoPrompt
    {
        get
        {
            return yesNoPrompt;
        }
    }
    public GameObject InteractivityCue
    {
        get
        {
            return interactivityCue;
        }
    }
    public bool IsActive
    {
        get
        {
            return textBox.activeSelf;
        }
    }
    #endregion
    #endregion

    #region Singleton Stuff
    private static TextBoxManager textBoxManager;
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


    }
    #endregion    
    #region Inherited from MonoBehaviour
    private void Awake()
    {
        //Depends on our hierarchy being laid out just-so. Would be better to do it some other way, but I need to design that.

        //Set up references to the root objects of the NeverUnload GameObject hierarchy
        //This should happen so often it'd probably make sense to make a class that deals with this in future games.
        List<GameObject> neverUnloadRootObjects = new List<GameObject>();
        Scene neverUnload = SceneManager.GetSceneByName("NeverUnload");
        neverUnload.GetRootGameObjects(neverUnloadRootObjects);

        //Get specifically the key text/dialog related GameObjects, use them to assign
        dialogCanvas = neverUnloadRootObjects.Find(x => x.name == "DialogCanvas");
        textBox = dialogCanvas.transform.Find("DialogTextbox").gameObject;
        namePlate = textBox.transform.Find("NamePlate").gameObject;
        yesNoPrompt = textBox.transform.Find("DialogYesNoPrompt").gameObject;
        boxContent = textBox.transform.Find("DialogText").GetComponent<Text>();
        npcNameTag = namePlate.transform.Find("Name").GetComponent<Text>();

        //Data structures relevant to this class
        textQueue = new Queue<string>();

        //Defaults
        IsMultiCharDialog = false;

        if (textFile != null) //ensure that the text file actually exists
        {
            textLines = (textFile.text.Split('\n'));
            for (int i = 0; i < textLines.Length; i++)
            {
                textQueue.Enqueue(textLines[i]);
            }
        }
        else
        {
            textQueue.Enqueue("Error: There is no text file! You should never see this message in actual gameplay.");
        }
    }
    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        SoundManager.onSoundChangedCallback += UpdateSound;

    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && textBox.activeSelf)
        {
            DisableTextUI();
            pauseMenu.Resume();
        }

        if (!textBox.activeSelf) //used to use a distinct variable for this, but it ended up being logically equivalent to the textBox's active state.
        {
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            if (!isTyping)
            {
                if(textQueue.Count <= 0)
                {
                    if (IsEventAtEndOfText) //Events should take precedence over Yes/No dialog, hence this comes first.
                    {
                        EventStart = true;
                        DisableTextUI();
                    }
                    else if (IsYesNoAtEndOfText)
                    {
                        //if (yesNoPrompt == null || yesNoPrompt.activeSelf == false) //This was calling a method that makes reference to the dialog prompt _when the dialog prompt does not exist_.
                        //When it _did_ exist, So wasteful!
                        if (yesNoPrompt != null)
                        {
                            yesNoPrompt.SetActive(true);
                            //EnableYesNoPrompt();                        
                        }
                        return;
                    }

                    else
                    {
                        DisableTextUI();
                    }
                }
                else
                {
                    this.NextLine();
                }
            }

            else if (isTyping && !cancelTyping)
            {
                cancelTyping = true;
            }
        }
    }
    #endregion

    private void NextLine()
    {
        /* Assumes the following formatting:
         * 
         * */
        if (IsMultiCharDialog)
        {
            string originalLine = textQueue.Dequeue();
            string[] parsedLine = (originalLine.Split(':'));

            if (npcNameTag != null)
            {
                npcNameTag.text = parsedLine[0];

                //The nameplate background for the name text only shows up if it exists as a gameobject, obviously, but also only if the NPC name isn't a blank string
                if (namePlate != null && npcNameTag.text != "")
                {
                    namePlate.SetActive(true);
                }
            }

            StartCoroutine(TextScroll(parsedLine[1]));
        }
        else
        { 
            StartCoroutine(TextScroll(textQueue.Dequeue()));
        }
    }

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
    public void EnableTextUI()
    {
        textBox.SetActive(true);
        if (npcGameObject)
        {
            if (!textBalloon)
                textBalloon = Instantiate(textBalloonPrefab, npcGameObject.transform.position + new Vector3(0, 0, 1.36f), Quaternion.Euler(90, 0, 0));
        }
        if (stopPlayerMovement) 
        {
            EventManager.TriggerEvent("StopMoving");
        }

        /* Maybe the wrong place for this. I think the intent is for face portraits?
         * */
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

        this.NextLine();
    }
    public void DisableTextUI()
    {
        if (InteractivityCue)
        { 
            InteractivityCue.SetActive(false);
        }
        if (YesNoPrompt)
        {
            YesNoPrompt.SetActive(false);
        }
        if (textBox)
        {
            textBox.SetActive(false);
        }
        
        Destroy(textBalloon);

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

        EventManager.TriggerEvent("resetNpcs");
    }
    public void ReloadScript(TextAsset theText)
    {
        if (theText != null)
        {
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));


            textQueue.Clear();
            for (int i = 0; i < textLines.Length; i++)
            {
                textQueue.Enqueue(textLines[i]);
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
        if (yesNoPrompt)
        {
            yesNoPrompt.SetActive(false);
        }
        EventManager.TriggerEvent("answersYes");
    }
    public void PlayerAnswersNo()
    {
        Debug.Log("Player clicked No!");
        if (yesNoPrompt)
        {
            yesNoPrompt.SetActive(false);
        }
        EventManager.TriggerEvent("answersNo");
    }
    void UpdateSound()
    {
        m_audioSource.volume = (SoundManager.SFXVolume * SoundScaleFactor) / 100;
    }
}
