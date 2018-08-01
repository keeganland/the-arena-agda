using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveElectricBeam : MonoBehaviour {
 
	public Transform Startpoint;
    public Vector3 Target;


	public float Speed;

    private Rigidbody rb;

    private void Awake()
    {
        transform.position = GameObject.Find("Boy").transform.position;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    float time;
    private void FixedUpdate()
	{
        if (time <= 0.8f)
        {
            Vector3 newPos = transform.position + Target * Time.deltaTime * Speed;
            rb.MovePosition(newPos);
        }
        time += Time.deltaTime;
	} 

    public void SetLineValues(Transform Beginning, Vector3 End)
	{
		Startpoint = Beginning;
		Target = End;
	}
}
