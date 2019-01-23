using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class NeverUnloadData : MonoBehaviour
{
    [SerializeField] private Scene neverUnload;
    [SerializeField] private List<GameObject> neverUnloadRootObjects;
    [SerializeField] private GameObject charactersRoot;
    [SerializeField] private List<GameObject> childrenOfCharactersRoot;
    [SerializeField] private static Dictionary<string, GameObject> gameObjectDictionary;

    public List<GameObject> NeverUnloadRootObjects
    {
        get
        {
            return neverUnloadRootObjects;
        }
    }

    public Dictionary<string, GameObject> GameObjectDictionary
    {
        get
        {
            return gameObjectDictionary;
        }
    }

    [Header("NeverUnload/Characters : General Useful Variables")]
    //public GameObject MainCamera; //Goes with "Main Camera" (neverUnload)
    [SerializeField] private NavMeshAgent boyNavMeshAgent;
    [SerializeField] private NavMeshAgent girlNavMeshAgent;
    [SerializeField] private GameObject playerUI; //The player UI Holder GameObject.
    [SerializeField] private bool stopAllActions; //Stop to attack, move, everything !
    [SerializeField] private bool stopCamera; //To move Camera freely with a transform without worrying about the "Automatic" camera.
    [SerializeField] private ScreenFader fader;

    [SerializeField] private GameObject boyUIGameObject;
    [SerializeField] private GameObject girlUIGameObject;

    [Header("NeverUnload/Characters : HealthControler")]
    [SerializeField] private GameObject _BoySlider; //Goes with "BoySlider" (neverUnload)
    [SerializeField] private GameObject _GirlSlider; //Goes with "GirlSlider" (neverUnload)
    [SerializeField] private GameObject _BoyDeathAnim; //Goes with "BoyDeath" (neverUnload)
    [SerializeField] private GameObject _GirlDeathAnim; //Goes with "GirlDeath" (neverUnload)
    [SerializeField] private GameObject _BoyCastReviveGameobject; //Goes with "BoyReviving" (neverUnload)
    [SerializeField] private GameObject _GirlCastReviveGameobject; //Goes with "GirlReviving" (neverUnload)
    [SerializeField] private Slider _BoyReviveSlider; //Goes with "BoyReviving" (neverUnload)
    [SerializeField] private Slider _GirlReviveSlider; //Goes with "GirlReviving" (neverUnload)
    [SerializeField] private Text _BoyReviveTextTimer; //Goes with "BoyCastingTimer" (neverUnload)
    [SerializeField] private Text _GirlReviveTextTimer; //Goes with "GirlCastingTimer" (neverUnload)
    [SerializeField] private ParticleSystem _DeathBoyParticle; //Goes with "DeathBoyParticle" (neverUnload)
    [SerializeField] private ParticleSystem _DeathGirlParticle; //Goes with "DeathGirlParticle" (neverUnload)
    [SerializeField] private ParticleSystem _ReviveBoyParticle; //Goes with "ReviveBoyParticle" (neverUnload)
    [SerializeField] private ParticleSystem _ReviveGirlParticle; //Goes with "ReviveGirlParticle" (neverUnload)


    #region Properties

    #region NeverUnload/Characters : General Useful Variables
    public NavMeshAgent BoyNavMeshAgent { get { return boyNavMeshAgent; } }
    public NavMeshAgent GirlNavMeshAgent { get { return girlNavMeshAgent; } }
    public GameObject PlayerUI { get { return playerUI; } }
    public bool StopAllActions { get { return stopAllActions; } }
    public bool StopCamera { get { return stopCamera; } }
    public ScreenFader Fader { get { return fader; } }

    public GameObject BoyUIGameObject { get { return boyUIGameObject; } }
    public GameObject GirlUIGameObject { get { return girlUIGameObject; } }



    #endregion

    #region NeverUnload/Characters : HealthControler
    public GameObject BoySlider { get { return _BoySlider; } } //Goes with "BoySlider" (neverUnload)
    public GameObject GirlSlider { get { return _GirlSlider; } } //Goes with "GirlSlider" (neverUnload)
    public GameObject BoyDeathAnim { get { return _BoyDeathAnim; } } //Goes with "BoyDeath" (neverUnload)
    public GameObject GirlDeathAnim { get { return _GirlDeathAnim; } } //Goes with "GirlDeath" (neverUnload)
    public GameObject BoyCastReviveGameobject { get { return _BoyCastReviveGameobject; } } //Goes with "BoyReviving" (neverUnload)
    public GameObject GirlCastReviveGameobject { get { return _GirlCastReviveGameobject; } } //Goes with "GirlReviving" (neverUnload)
    public Slider BoyReviveSlider { get { return _BoyReviveSlider; } } //Goes with "BoyReviving" (neverUnload)
    public Slider GirlReviveSlider { get { return _GirlReviveSlider; } } //Goes with "GirlReviving" (neverUnload)
    public Text BoyReviveTextTimer{ get { return _BoyReviveTextTimer; } } //Goes with "BoyCastingTimer" (neverUnload)
    public Text GirlReviveTextTimer{ get { return _GirlReviveTextTimer; } } //Goes with "GirlCastingTimer" (neverUnload)
    public ParticleSystem DeathBoyParticle{ get { return _DeathBoyParticle; } } //Goes with "DeathBoyParticle" (neverUnload)
    public ParticleSystem DeathGirlParticle{ get { return _DeathGirlParticle; } } //Goes with "DeathGirlParticle" (neverUnload)
    public ParticleSystem ReviveBoyParticle{ get { return _ReviveBoyParticle; } } //Goes with "ReviveBoyParticle" (neverUnload)
    public ParticleSystem ReviveGirlParticle{ get { return _ReviveGirlParticle; } } //Goes with "ReviveGirlParticle" (neverUnload)

    #endregion
    
    #endregion

    private void Awake()
    {
        Queue<GameObject> gameObjectsQueue = new Queue<GameObject>();
        neverUnload = SceneManager.GetSceneByName("NeverUnload");
        neverUnload.GetRootGameObjects(neverUnloadRootObjects);

        foreach (GameObject x in neverUnloadRootObjects)
        {
            gameObjectsQueue.Enqueue(x);
        }

        //while (gameObjectsQueue.Count > 0)
        //{
        //    GameObject x = gameObjectsQueue.Dequeue();
        //    gameObjectDictionary.Add(x.name, x);
        //    foreach (Transform child in x.transform)
        //    {
        //        gameObjectsQueue.Enqueue(child.gameObject);
        //    }
        //}
        //Debug.Log("KEEGAN TEST: " + boy.ToString());
    }
  
}
