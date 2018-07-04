using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceFromSprite : MonoBehaviour {

    public float _DistanceFromSprite;

    private SpriteScript2[] Sprite;

	// Use this for initialization
	void Start () {

        Sprite = GameObject.Find("/Characters").GetComponentsInChildren<SpriteScript2>();

        for (int i = 0; i < Sprite.Length; i++)
        {
            Sprite[i]._DistanceFromSprite = _DistanceFromSprite;
        }
	}
}
