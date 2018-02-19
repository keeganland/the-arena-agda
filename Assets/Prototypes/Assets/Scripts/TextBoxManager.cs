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
    public GameObject interactivityCue;
    public Text boxContent;

    //these exist for the management of the external .txt file
    public TextAsset textFile;
    public string[] textLines; //Keegan NTS: wait why not just use a queue for this hm

    //these refer to particular lines in the text file because we're using a string array for some dumb reason
    public int currentLine;
    public int endAtLine;

    public MovementManager movementManager;

    public bool isActive; //if active, the player may use enter to increment lines and other ways of interacting with the text box. Maybe switch to private
    public bool cueActive = false;
    public bool stopPlayerMovement;

    private bool isTyping = false;
    private bool cancelTyping = false;

    public float typeSpeed;

    // Use this for initialization
    void Start()
    {
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

        if(!isActive)
        {
            return;
        }

        /*
        if (cueActive)
        {
            EnableCue();
        }
        else
        (
            DisableCue();
        )*/

        if(Input.GetKeyDown(KeyCode.Return))
        {

            if (!isTyping)
            {
                currentLine += 1; //the way things work at the moment is just increment through the array. should sooner or later be replaced with a queue
                if (currentLine > endAtLine)
                {
                    DisableTextBox();
                }
                else
                {
                    StartCoroutine(TextScroll(textLines[currentLine]));
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
        isActive = true;

        if (stopPlayerMovement)
        {
            movementManager.StopPlayerMovement();
        }
        StartCoroutine(TextScroll(textLines[currentLine]));
    }

    public void DisableTextBox()
    {
        textBox.SetActive(false);
        isActive = false;

        movementManager.StartPlayerMovement();
    }

    public void ReloadScript(TextAsset theText)
    {
        if (theText != null)
        {
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));
        }
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
}
