using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveElectricBeam : MonoBehaviour {
 
	public Transform Target;
	public Transform Startpoint;

	public float Speed;

	private void Start()
	{
		transform.position = Startpoint.position;
	}

	private void FixedUpdate()
	{
        transform.position = Vector3.Lerp(transform.position, Target.position, Speed * Time.fixedDeltaTime);
	} 

	public void SetLineValues(Transform Beginning, Transform End)
	{
		Startpoint = Beginning;
		Target = End;
	}
}
