using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraAngle : MonoBehaviour
{

    public float NewCameraAngle;
    public float OldCameraAngle;
    public float Speed;

    [SerializeField] private bool CameraChangeAngle = false;
    [SerializeField] private float Counter;

    private PublicVariableHolderneverUnload publicVariableHolderneverUnload;
    private GameObject MainCamera;
    private Collider collider;

    private GameObject boySpriteGameobject;
    private GameObject girlSpriteGameobject;
    private float m_newAngle;
    private float m_oldAngle;
    private float m_anglestoLerpto;
    [SerializeField] private float x;

    // Use this for initialization
    void Start()
    {
        publicVariableHolderneverUnload = GameObject.Find("/PublicVariableHolderNeverUnload").GetComponent<PublicVariableHolderneverUnload>();

        girlSpriteGameobject = GameObject.FindGameObjectWithTag("Sprite/Girl");
        boySpriteGameobject = GameObject.FindGameObjectWithTag("Sprite/Boy");

        MainCamera = publicVariableHolderneverUnload.MainCamera;
        collider = GetComponent<Collider>();

        m_newAngle = NewCameraAngle;
        m_oldAngle = OldCameraAngle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == ("Girl"))
        {
            //Debug.Log(Mathf.Abs(x));
            //Debug.Log(Mathf.Abs(NewCameraAngle + 1f));
            if (Mathf.Abs(x) >= Mathf.Abs(NewCameraAngle))
            {
                CameraChangeAngle = false;
                Counter = 0;
            }
            else if (Mathf.Abs(x) <= Mathf.Abs(OldCameraAngle))
            {
                CameraChangeAngle = true;
                Counter = 0;
            }

        }
    }

    private void Update()
    {
        if (CameraChangeAngle)
        {
            Counter += Speed * Time.deltaTime;
            x = Mathf.Lerp(m_oldAngle, m_newAngle, Counter);
            MainCamera.transform.localRotation = Quaternion.Euler(0, 0, x);
            boySpriteGameobject.transform.localRotation = Quaternion.Euler(90, 0, x);
            girlSpriteGameobject.transform.localRotation = Quaternion.Euler(90, 0, x);
        }
        else if (!CameraChangeAngle)
        {
            Counter += Speed * Time.deltaTime;
            x = Mathf.Lerp(m_newAngle, m_oldAngle, Counter);
            MainCamera.transform.localRotation = Quaternion.Euler(0, 0, x);
            boySpriteGameobject.transform.localRotation = Quaternion.Euler(90, 0, x);
            girlSpriteGameobject.transform.localRotation = Quaternion.Euler(90, 0, x);
        }
    }
}
