using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("Plane size: " + GetComponent<Renderer> ().bounds.size);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
