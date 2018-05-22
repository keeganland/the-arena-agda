using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IncreaseMoneyScript : MonoBehaviour {

	public GameObject icon;
	public TextMeshProUGUI text;
	public float initialQuantity;

	public void NumberIncreaseCaller (float quantity, float delay){
		StartCoroutine	(NumberIncrease (quantity, delay));
	}

	IEnumerator NumberIncrease (float quantity, float delay){
		yield return new WaitForSeconds (delay);
		iTween.PunchScale (icon, new Vector2 (.15f,.15f), 0.2f);
		initialQuantity += quantity;
		text.text = initialQuantity.ToString () + ",00$";
	}
}
