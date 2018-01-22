using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public List<Transform> targets;
    public float m_speed = 1.0f;
    public Transform target;

    [SerializeField]
    private int m_number = 0;
    private Vector3 m_new_Pos;

    // Use this for initialization
    void Start()
    {
        target = targets[m_number];
    }

    // Update is called once per frame
    void Update()
    { 
        /**
         *  Keegan's NTS: 
         *  Alex's intention with this if-block is "if the target is the player, etc" so that the behaviour in the block only takes place during normal gameplay circumstances.
         *  It will not behave as such during e.g. a cutscene.
         *  As it is, it is completely useless and will be modified during a future pass.
         */

        if (target)
        {



            m_new_Pos =new Vector3(targets[m_number].position.x, this.transform.position.y, targets[m_number].position.z);

            transform.position = Vector3.Lerp(transform.position, m_new_Pos, m_speed*Time.deltaTime);

            //TODO: Cleanup
            //Earlier incorrect line:
            //transform.position = Vector3.Lerp(transform.position, targets[m_number].position, m_speed*Time.deltaTime) + new Vector3(0, 10f, 0);
            //Slight modification
            //transform.position = Vector3.Lerp(transform.position, targets[m_number].position, m_speed * Time.deltaTime);
            //Alex's proposed fix:

            m_new_Pos = new Vector3(targets[m_number].position.x, this.transform.position.y, targets[m_number].position.z);

            transform.position = Vector3.Lerp(transform.position, m_new_Pos, m_speed * Time.deltaTime);
            
            ChangeCharacters();
        }

    }

    void ChangeCharacters()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (GameObject.FindWithTag("Player").GetComponent<Player_switch>()._Switchplayer == true)
            {
                m_number = 0;
            }
            else if (GameObject.FindWithTag("Player").GetComponent<Player_switch>()._Switchplayer == false)
            {
                m_number = 1;
            }

            target = targets[m_number];

        }
    }
}