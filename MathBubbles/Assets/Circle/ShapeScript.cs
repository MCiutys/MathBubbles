using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeScript : MonoBehaviour {

	public Sprite[] sprites;
	public Sprite sprite;
	private Color color;
	private float velocityX, velocityY;
	private bool right, up;
	private string equation;
	private bool correct;
	private int fontSize;
	private Rect textArea;
	private GUIStyle style;
	private GUIContent content;

	System.Random random;

	// Use this for initialization
	void Start () {
		random = new System.Random ();
		int rand = random.Next (sprites.Length);
		//Debug.Log (rand);
		sprite = sprites [rand];
		GetComponent<SpriteRenderer> ().sprite = sprite;
		float floatRandom = Random.Range (Con.Constants.SMALLEST_SHAPE_SIZE, Con.Constants.LARGEST_SHAPE_SIZE);
		Vector2 temp = transform.localScale;
		temp.x = floatRandom;
		temp.y = floatRandom;
		transform.localScale = temp;
		textArea = new Rect (5, 5, GetComponent<SpriteRenderer> ().bounds.size.x, GetComponent<SpriteRenderer> ().bounds.size.y);
		fontSize = (int) GetComponent<SpriteRenderer> ().bounds.size.x * 20;
		//Debug.Log (GetComponent<SpriteRenderer> ().bounds.size.x);

		if (random.Next (2) == 0) {
			right = true;
		} else {
			right = false;
		}

		if (random.Next (2) == 0) {
			up = true;
		} else {
			up = false;
		}

		velocityX = Random.Range (Con.Constants.VELOCITY_MIN, Con.Constants.VELOCITY_MAX);
		velocityY = Random.Range (Con.Constants.VELOCITY_MIN, Con.Constants.VELOCITY_MAX);
		GetComponent<SpriteRenderer> ().color = color;
	}

	public void UploadColor(Color c) {
		color = c;
	}

	public void UploadSprite(Sprite sprite) {
		//renderer.sprite = sprite;
	}

	public void UploadPosition(double x, double y) {
		Vector2 temp = GetComponent<Rigidbody2D> ().position;
		temp.x = (float) x;
		temp.y = (float) y;
		Debug.Log ("X: " + x);
		if (x < 500 && x > 1000) {
			Debug.Log ("LARGEEEER: " + x);
		}
		Debug.Log ("Y: " + y);
		GetComponent<Rigidbody2D>().position = Camera.main.ViewportToWorldPoint(temp);
	}

	public void UpdateEquation(string eq) {
		equation = eq;
	}

	public void UpdateCorrectOrNot(bool c) {
		correct = c;
	}
		

	void OnGUI() {
		style = new GUIStyle ();
		content = new GUIContent (equation);
		style.fontSize = fontSize;

		Vector2 tempRect = GetComponent<Rigidbody2D> ().position;
		tempRect.x = Mathf.Round (tempRect.x * 100f) / 100f;
		tempRect.y = -GetComponent<Rigidbody2D> ().position.y;
		tempRect.y = Mathf.Round (tempRect.y * 100f) / 100f;

		Vector2 pos = Camera.main.WorldToScreenPoint (tempRect);


		Vector2 styleSize = style.CalcSize (content);
		pos.x -= styleSize.x / 2;
		pos.y -= styleSize.y / 2;
		textArea.position = pos;
		//GetComponent<SpriteRenderer> ().sprite = sprite;
		style.normal.textColor = Color.black;
		GUI.Label (textArea, content, style);
	}

	// Check which way object has to go (true -> right, false -> left)
	bool GoToRight(Vector2 t) {
		if (t.x >= 0.8) {
			right = false;
		} else if (t.x <= 0.2) {
			right = true;
		}
		return right;
	}

	// Check which way object has to go (true -> up, false -> down)
	bool GoUp(Vector2 t) {
		if (t.y >= 0.8) {
			up = false;
		} else if (t.y <= 0.2) {
			up = true;
		}
		return up;
	}

	void OnMouseDown() {
		if (correct) {
			Debug.Log ("Correct clicked");
		} else {
			Debug.Log ("incorrect");
		}
	}

	public bool IsCorrect() {
		return correct;
	}
		
	
	// Update is called once per frame
	void Update () {
		Vector2 bodyPosition = GetComponent<Rigidbody2D>().position;
		Vector2 viewportPoint = Camera.main.WorldToViewportPoint (bodyPosition);

		GoToRight (viewportPoint);
		GoUp (viewportPoint);

		if (up) {
			bodyPosition.y += velocityY * Time.smoothDeltaTime;
		} else {
			bodyPosition.y -= velocityY * Time.smoothDeltaTime;
		}

		if (right) {
			bodyPosition.x += velocityX * Time.smoothDeltaTime;
		} else {
			bodyPosition.x -= velocityX * Time.smoothDeltaTime;
		}


		GetComponent<Rigidbody2D> ().MovePosition (bodyPosition);
	}

	void OnCollisionEnter2D() {
		GetComponent<Rigidbody2D> ().freezeRotation = false;
	}
}
