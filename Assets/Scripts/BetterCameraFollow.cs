using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Alex : Refactored 01/13 - 
//TODO:Keegan can you valid the changes for the Camera script Refactoring?

public class BetterCameraFollow : MonoBehaviour
{
    //Config
    [SerializeField] private int cameraTargetIndex = 0; //The camera choose between the List<Transform> targets;

    [SerializeField] private float _Speed = 1.0f;
    [SerializeField] private float _FieldOfViewMin = 2.1f;
    [SerializeField] private float _FieldOfViewMax = 3f;

    //State
    private bool m_manualCamera;
    private bool cutSceneMode = false;
    private bool targetsTooFar; //variable created to carry the "neutral area" value

    //Cached component  references
    public PublicVariableHolderneverUnload _PublicVariableHolder;
    [SerializeField] private List<Transform> targets;
    [SerializeField] private List<CameraTarget> potentialCameraTargetList;
    private List<GameObject> m_currentcameraTargets = new List<GameObject>(); //Array that carry the "actual" targets
    private List<GameObject> m_currentcameraTargetsTooFar = new List<GameObject>(); //Array that carry the targets that are not centered

    [SerializeField] private Transform target;
    private Camera m_cam;

    //Messages then methods

    private void Awake()
    {
        EventManager.StartListening("camTargetRefresh", ReinitializePotentialTargets);
    }

    private void OnDisable()
    {
        EventManager.StopListening("camTargetRefresh", ReinitializePotentialTargets);
    }

    private void Start()
    {
        //m_potentialcameraTargets = (CameraTarget[])Object.FindObjectsOfType(typeof(CameraTarget));

        ReinitializePotentialTargets();

        target = targets[cameraTargetIndex];
        m_cam = GetComponent<Camera>();
    }

    public void ReinitializePotentialTargets()
    {
        CameraTarget[] potentialCameraTargetArray = (CameraTarget[])Object.FindObjectsOfType(typeof(CameraTarget));
        m_currentcameraTargets.Clear();
        potentialCameraTargetList = new List<CameraTarget>();
        for (int i = 0; i < potentialCameraTargetArray.Length; i++)
        {
            //Debug.Log("Potential camera target # " + i + ": " + potentialCameraTargetArray[i].name);
            potentialCameraTargetList.Add(potentialCameraTargetArray[i]);
        }
    }

    private void Update()
    {
        /*
         *  Keegan's NTS: 
         *  Alex's intention with this if-block is "if the target is the player, etc" so that the behaviour in the block only takes place during normal gameplay circumstances.
         *  It will not behave as such during e.g. a cutscene.
         *  As it is, it is completely useless and will be modified during a future pass.
         */

        if (target && !_PublicVariableHolder.StopCamera)
        {
            ManualCamera();
            ChangeCharacters();
            AutomaticCamera();

            if (m_currentcameraTargetsTooFar.Count >=1) //if more at least 1 enemy is too far from the center
            {
                StartCoroutine(ChangeFieldofView(_FieldOfViewMax));//Zoom out, needs to be in Update as "Mathf.Lerp" has to be updated every frame
                targetsTooFar = true; //changes the boolian that carries in the "neutral area" to true.
            }
            else
            {
                StartCoroutine(ChangeFieldofView(_FieldOfViewMin)); //Zoom in
                targetsTooFar = false;
            }
        }

    }

    void ChangeCharacters()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cameraTargetIndex = 0;
            TargetChangeInRange();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            cameraTargetIndex = 1;
            TargetChangeInRange();
        }

        target = targets[cameraTargetIndex];
    }
    void TargetChangeInRange()
    {
        targets.ForEach(x =>                                                   //hard to explain but basically an if function in a foreach loop... it triggers the movement if one target in not in view
        { if (IsInView(m_cam.gameObject, x.gameObject) == false)                //DOESN'T work if two targets are on screen but one is out of view (it will move no matter what).
               transform.position = new Vector3(targets[cameraTargetIndex].position.x, this.transform.position.y, targets[cameraTargetIndex].position.z);  
        });
    }
    void AutomaticCamera()
    {
        //Alex's old version.
        /*
            for (int i = 0; i < m_potentialcameraTargets.Length; i++) //for each potential enemies, check if they are in view.
            {
                bool m_isinview = IsInView(m_cam.gameObject, m_potentialcameraTargets[i].gameObject);

                if (m_isinview == true && !m_currentcameraTargets.Contains(m_potentialcameraTargets[i].gameObject)) //if yes, add then to current targets
                {
                    m_currentcameraTargets.Add(m_potentialcameraTargets[i].gameObject);                
                }
                else if (m_isinview == false && m_currentcameraTargets.Contains(m_potentialcameraTargets[i].gameObject)) //if no, remove them from current targets
                {
                    m_currentcameraTargets.Remove(m_potentialcameraTargets[i].gameObject);
                }
            }
        */
        
        //KEEGAN WIP: REPLICATE ALEX'S CODE ABOVE
        foreach (CameraTarget ct in potentialCameraTargetList)
        {
            if (ct == null)
            {
                //Debug.Log("null element in potentialCameraTargetList???");
                throw new System.Exception("A null element was found in potentialCameraTargetList");
                //break;
            }
            else
            {
                bool m_isinview = IsInView(m_cam.gameObject, ct.gameObject);

                if (m_isinview == true && !m_currentcameraTargets.Contains(ct.gameObject))
                {
                    m_currentcameraTargets.Add(ct.gameObject);
                }
                else if (m_isinview == false && m_currentcameraTargets.Contains(ct.gameObject))
                {
                    m_currentcameraTargets.Remove(ct.gameObject);
                }
            }
        }

        AdjustCamera(m_currentcameraTargets);
        ClearTargetTooFar();

    }
    void ManualCamera()
    {
            if (Input.GetKey(KeyCode.Space))
            {
                m_manualCamera = true;
            }

            else if (Input.GetKeyUp(KeyCode.Space)) //cancel the lock in for the camera and switch back to automatic camera
            {
                m_manualCamera = false;
            }
    }

    private bool IsInView(GameObject origin, GameObject toCheck) //check if the enemy is viewed in the screen, return a bool.
    {
        Vector3 pointOnScreen = m_cam.WorldToScreenPoint(toCheck.GetComponentInChildren<Renderer>().bounds.center);

        // Is in FOV
        if ((pointOnScreen.x < 0) || (pointOnScreen.x > Screen.width) ||
        (pointOnScreen.y < 0) || (pointOnScreen.y > Screen.height))
        {
            //Debug.Log("OutOfBounds: " + toCheck.name);
            return false;
        }

        //RaycastHit hit;
        //Vector3 heading = toCheck.transform.position - origin.transform.position;
        //Vector3 direction = heading.normalized;// / heading.magnitude;

        //if (Physics.Linecast(cam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, out hit))
        //{
        //    if (hit.transform.name != toCheck.name)
        //    {
        //        /* -->
        //        Debug.DrawLine(cam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, Color.red);
        //        Debug.LogError(toCheck.name + " occluded by " + hit.transform.name);
        //        */
        //        Debug.Log(toCheck.name + " occluded by " + hit.transform.name);
        //        return false;
        //    }
        //}
        return true;
    }
    private bool IsTooFar(GameObject origin, GameObject toCheck) //Check if the enemy is on the border of the screen
    {
        Vector3 pointOnScreen = m_cam.WorldToScreenPoint(toCheck.GetComponentInChildren<Renderer>().bounds.center); 


        if ((pointOnScreen.x < Screen.width * 0.15f) || (pointOnScreen.x > Screen.width * 0.85f) || //if the enemy if within 10% on the edge of the screen, return "too far"
            (pointOnScreen.y < Screen.height * 0.15f) || (pointOnScreen.y > Screen.height * 0.85f))
        {
            return true;
        }
        else if((pointOnScreen.x > Screen.width * 0.3f) && (pointOnScreen.x < Screen.width * 0.70f) && //if the enemy if within 75% inside the screen, return "no too far"
                (pointOnScreen.y > Screen.height * 0.3f) && (pointOnScreen.y < Screen.height * 0.7f))
        {
            return false;
        }
        else return targetsTooFar; //neutral area that allows the coroutine to operate without problem
    }
    private void AdjustCamera(List<GameObject> currentTargets) //set the camera in the middle of all "viewed enemies" (using vector calculations)
    {
            Vector3 CameraNewpos = new Vector3(0, this.transform.position.y, 0);
            int count = 0; 
            for (int i = 0; i < currentTargets.Count; i++)
            {
            if (currentTargets[i] == null)
            {
                count += 1;
            }
            else
            {
                CameraNewpos.x += currentTargets[i].transform.position.x;
                CameraNewpos.z += currentTargets[i].transform.position.z;
            }
            }

        CameraNewpos.x /= (currentTargets.Count - count);
        CameraNewpos.z /= (currentTargets.Count - count);

            if (m_currentcameraTargets.Count == 0 || m_manualCamera == true)
            {
                Vector3 m_pos = new Vector3(targets[cameraTargetIndex].transform.position.x, this.transform.position.y, targets[cameraTargetIndex].transform.position.z);
                transform.position = Vector3.Lerp(transform.position, m_pos, _Speed * Time.deltaTime);
                return;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, CameraNewpos, _Speed * Time.deltaTime);
            }
    }
    private void ClearTargetTooFar()
    {
        if (m_currentcameraTargets.Count >= 2) //if there is a least two current targets
        {
            for (int i = 0; i < m_currentcameraTargets.Count; i++)
            {
                bool isFar = IsTooFar(m_cam.gameObject, m_currentcameraTargets[i]);

                if (isFar == true && !m_currentcameraTargetsTooFar.Contains(m_currentcameraTargets[i])) //if at least one of them is too far, add them to the "Far" array
                {
                    m_currentcameraTargetsTooFar.Add(m_currentcameraTargets[i]);
                }
                else if (isFar == false && m_currentcameraTargetsTooFar.Contains(m_currentcameraTargets[i])) //if not, remove them from the "Far" array
                {
                    m_currentcameraTargetsTooFar.Remove(m_currentcameraTargets[i]);
                }
            }
        }
    }

    private IEnumerator ChangeFieldofView(float _FieldofViewValue) //Coroutine to zoom in or out
    {

        m_cam.orthographicSize = Mathf.Lerp(m_cam.orthographicSize, _FieldofViewValue, _Speed * Time.fixedDeltaTime);
        yield return new WaitWhile(() => ((m_cam.orthographicSize <= .9f * _FieldofViewValue || m_cam.orthographicSize>= 1.1f*_FieldofViewValue))); //Wait a certain "value" before canceling reuturning 
        //we can't wait the size to feet perfectly the new value because Mathf.Lerp will never reach it (but tend to reach it... thanks taylor series...).
    }

    /* Creates a LIST (not an array) of every potential target
     * 
     * Not especially space efficient but shouldn't be a huge issue.
     * Takes advantage of the fact that FindObjectsOfType is an existing library func that returns arrays. Just reallocates the 
     */

    /*
     * Keegan NTS: the below might never be used? 
     * 
     */

    public bool GetCutsceneMode()
    {
        return cutSceneMode;
    }

    public void SetCutsceneMode(bool x)
    {
        cutSceneMode = x;
    }

    public void SetFieldOfView(float fieldOfViewMin, float fieldOfViewMax)
    {
        _FieldOfViewMin = fieldOfViewMin;
        _FieldOfViewMax = fieldOfViewMax;
    }
}