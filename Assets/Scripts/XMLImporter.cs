using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;


/**
 * Temporary. Integrate with the text manager eventually.
 * 
 * see:
 * http://amalgamatelabs.com/Blog/2/unity_and_xml
 * https://www.youtube.com/watch?v=SrdrtsRYtPA&t=535s
 */

public class XMLImporter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LoadSceneData()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("test");
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(textAsset.text);

    }
}
