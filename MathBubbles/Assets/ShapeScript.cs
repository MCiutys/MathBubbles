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
	private double shapeLength;
	public Equation eq;
	int counter = 0;

	System.Random random;

	void Awake() {
		//Debug.Log ("START CALLED");
		random = new System.Random ();
		int rand = random.Next (sprites.Length);
		//Debug.Log (rand);
		//sprite = sprites [rand];
		//GetComponent<SpriteRenderer> ().sprite = sprite;
		float floatRandom = Random.Range (Con.Constants.SMALLEST_SHAPE_SIZE, Con.Constants.LARGEST_SHAPE_SIZE);
		/*Vector2 temp = transform.localScale;
		temp.x = floatRandom;
		temp.y = floatRandom;
		transform.localScale = temp;*/
		eq = new Equation (0, true, 0);

		textArea = new Rect (5, 5, GetComponent<SpriteRenderer> ().bounds.size.x, GetComponent<SpriteRenderer> ().bounds.size.y);
		//fontSize = (int) (GetComponent<SpriteRenderer> ().bounds.size.x);

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

	// Use this for initialization
	void Start () {
		//Debug.Log ("START CALLED");
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

		shapeLength = floatRandom;
		//eq = new Equation (1, false, 1);
		//Debug.Log ("Initialized ID: " + this.GetHashCode());

		textArea = new Rect (5, 5, GetComponent<SpriteRenderer> ().bounds.size.x, GetComponent<SpriteRenderer> ().bounds.size.y);
		//fontSize = (int) GetComponent<SpriteRenderer> ().bounds.size.x;
		fontSize = (int) (shapeLength * 3);
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

	public void UploadSprite(string spriteName) {
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (spriteName);
	}

	public double UploadFontSize() {
		double firstMultiplier = 0;
		switch (sprite.name) {
		case "Circle":
			firstMultiplier = 1.2;
			break;
		case "Triangle":
			firstMultiplier = 0.8;
			break;
		case "Square":
			firstMultiplier = 1.5;
			break;
		}
		return firstMultiplier;
	}

	public Equation GetEquation() {
		return eq;
	}

	public float GetX() {
		return GetComponent<Rigidbody2D> ().position.x;
	}

	public float GetY() {
		return GetComponent<Rigidbody2D> ().position.y;
	}

	public void UploadPosition(double x, double y) {
		Vector3 temp = new Vector3 (0, 0, 1);//GetComponent<Rigidbody2D> ().position;
		temp.x = (float) x;
		temp.y = (float) y;
		//GetComponent<Rigidbody2D> ().position = /*new Vector3 (-5, 3, 0); //*/Camera.main.ViewportToWorldPoint(temp);
		//GetComponent<Rigidbody2D>().MovePosition(Camera.main.ViewportToWorldPoint(temp));
		//GetComponent<Rigidbody2D>().MovePosition(new Vector2(6, 3));
		//GetComponent<Rigidbody2D>().position = temp;
		transform.position = Camera.main.ViewportToWorldPoint(temp);
	}

	public void UpdateEquation(string e) {
		equation = e;
	}

	public void UpdateEq(Equation e) {
		eq = e;
	}

	public void UpdateEqu(int level, bool corr, int answer) {
		eq.ChangeLevel (level);
		eq.ChangeCorrect (corr);
		eq.ChangeAnswer (answer);
		eq.GenerateQuestion ();
	}

	public void UpdateCorrectOrNot(bool c) {
		correct = c;
	}

	public double GetShapeLength() {
		return shapeLength;
	}

	void OnGUI() {
		style = new GUIStyle ();
		if (eq != null) {
			content = new GUIContent (eq.ToString ());
		} else {
			content = new GUIContent ("NONE");
		}
		style.fontSize = (int) (fontSize * UploadFontSize () * GetEqLengthProportion () * 5);

		Vector2 tempRect = GetComponent<Rigidbody2D> ().position;
		tempRect.x = Mathf.Round (tempRect.x * 100f) / 100f;
		tempRect.y = -GetComponent<Rigidbody2D> ().position.y;
		tempRect.y = Mathf.Round (tempRect.y * 100f) / 100f;

		Vector2 pos = Camera.main.WorldToScreenPoint (tempRect);

		Vector2 styleSize = style.CalcSize (content);
		pos.x -= styleSize.x / 2;
		pos.y -= styleSize.y / 2;
		textArea.position = pos;
		style.normal.textColor = Color.black;
		GUI.Label (textArea, content, style);
	}

	private double GetEqLengthProportion() {
		switch(eq.GetLength()) {
		case 1:
		case 2:
		case 3:
		case 4:
			return 1;
		case 5:
			return 0.9;
		case 6:
			return 0.8;
		case 7:
			return 0.6;
		}
		return 1;
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
		return eq.GetCorrectness();
	}

	public string GetSpriteName() {
		return sprite.name;
	}
		
	
	// Update is called once per frame
	void Update () {
		//Vector2 bodyPosition = GetComponent<Rigidbody2D>().position;
		Vector2 bodyPosition = transform.position;
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
