using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shape : MonoBehaviour {

	private Color color;
	// instance of Equation will be held

	private Sprite sprite;
	private bool right, up;


	public void UploadColor(Color c) {
		color = c;
	}

}
