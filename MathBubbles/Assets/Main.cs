using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	private const int MAX = 16;
	private const int MIN = 5;

	public Sprite sprite;

	public Sprite[] sprites;
	private string answer;
	private ArrayList shapes;
	private Color[] colors = new Color[] { Color.blue, Color.green, Color.yellow, Color.red };
	private System.Random rand;
	private int level;
	private int score;
	private float time;
	// Number of correct or incorrect consecutive answers
	// If positive, a number of correct answers has been detected
	// If negative, a number of incorrect answers has been detected
	private int consecutive; 

	// Use this for initialization
	void Start () {
		level = 1;
		score = 0;
		time = 90f;
		consecutive = 0;
		rand = new System.Random ();
		shapes = new ArrayList ();
		CreateShapes ();
	}

	void CreateShapes() {
		bool correctEq = true;

		switch (level) {
		case 1: 
			answer = GenerateFirstLevelAnswer ().ToString ();
			break;
		case 2:
			answer = GenerateSecondLevelAnswer ().ToString ();
			break;
		case 3:
			answer = GenerateThirdLevelAnswer ().ToString ();
			break;
		}

		for (int i = 0; i < Con.Constants.GetNumberOfQuestions(level); i++) {
			GameObject shape = (GameObject) Instantiate(Resources.Load("Circle"), new Vector3(i * 4, 0, 0), Quaternion.identity);
			ShapeScript script = shape.GetComponent<ShapeScript> ();
			script.UploadColor (colors [rand.Next(colors.Length)]);
			double x = 0.4 * rand.NextDouble () + 0.3;
			double y = rand.NextDouble ();
			//Debug.Log ("Height: " + Screen.height);
			//Debug.Log ("Width: " + Screen.width);
			script.UploadPosition (x, y);
			script.UpdateEquation (CreateQuestion (correctEq));
			if (i == 0) {
				script.UpdateEq(new Equation (level, true, int.Parse(answer)));
			} else {
				script.UpdateEq(new Equation (level, false, int.Parse(answer)));
			}
			if (i == 0) {
				script.UpdateCorrectOrNot (true);	
				correctEq = false;
			} else {
				script.UpdateCorrectOrNot (false);
			}
			shapes.Add (shape);
		}
	}

	void DestroyShapes() {
		for (int i = 0; i < shapes.Count; i++) {
			GameObject shape = (GameObject) shapes [i];
			Destroy (shape);
		}
		shapes.Clear ();
	}

	void OnGUI() {
		GUIStyle style = new GUIStyle ();
		style.fontSize = 36;
		style.normal.textColor = Color.white;
		Rect scoreArea = new Rect (10, 0, 100, 100);
		Rect scoreAreaNumber = new Rect (10, 50, 100, 100);
		Rect levelArea = new Rect (10, Screen.height - 100, 100, 100);
		Rect levelAreaNumber = new Rect (10, Screen.height - 50, 100, 100);
		Rect timeArea = new Rect (Screen.width - 150, 0, 10, 10);
		Rect timeAreaNumber = new Rect (Screen.width - 150, 50, 10, 10);
		Rect answerArea = new Rect (Screen.width - 150, Screen.height - 100, 10, 10);
		Rect answerAreaNumber = new Rect (Screen.width - 150, Screen.height - 50, 10, 10);
		GUI.Label (scoreArea, "Score", style);
		GUI.Label (scoreAreaNumber, "" + score, style);
		GUI.Label (levelArea, "Level", style);
		GUI.Label (levelAreaNumber, "" + level, style);
		GUI.Label (timeArea, "Time", style);
		GUI.Label (timeAreaNumber,"" +  Mathf.Round(time), style);
		GUI.Label (answerArea, "Answer", style);
		GUI.Label (answerAreaNumber, "" + answer, style);
	}

	string CreateQuestion(bool correctQuestion) {

		switch (level) {
		case 1:
			return GenerateFirstLevelQuestion (correctQuestion);
		case 2:
			return GenerateSecondLevelQuestion (correctQuestion);
		case 3:
			return GenerateThirdLevelQuestion (correctQuestion);
		}
		return "";
	}

	int GenerateWrongNumber(int original) {
		return original += rand.Next (0, 3);
	}

	string GenerateFirstLevelQuestion(bool correctQuestion) {
		char sign;
		// Generate sign of the equation
		if (rand.Next (2) == 0) {
			sign = '+';
		} else {
			sign = '-';
		}

		int first, second;
		int ans = int.Parse (answer);

		if (sign == '-') {
			first = rand.Next (ans, Con.Constants.LEVEL1_MAX);
			second = Mathf.Abs(ans - first); 
		} else {
			first = rand.Next (Con.Constants.LEVEL1_MIN, ans);
			second = ans - first;
		}

		if (!correctQuestion) {
			int modifyFirstBy = rand.Next (0, 3);
			int modifySecondBy = rand.Next (0, 3);

			while (modifySecondBy == modifyFirstBy) {
				modifySecondBy = rand.Next (0, 3);
			}

			first += modifyFirstBy;
			second += modifySecondBy;
		} 

		return "" + first + sign + second;
	}

	string GenerateSecondLevelQuestion(bool correctQuestion) {
		char sign = '+';
		string final;
		int random = rand.Next (3);
		switch (random) {
		case 0:
			sign = '+';
			break;
		case 1:
			sign = '-';
			break;
		case 2:
			sign = '*';
			break;
		}

		int first, second;
		int ans = int.Parse (answer);

		if (sign == '+') {
			first = rand.Next (Con.Constants.LEVEL2_MIN, ans);
			second = ans - first;
		} else if (sign == '-') {
			first = rand.Next (ans, Con.Constants.LEVEL2_MAX);
			second = Mathf.Abs (ans - first);
		} else {
			if (!Con.Constants.IsNotPrime (ans)) {
				first = ans;
				second = 1;
			} else {
				first = Con.Constants.Divide (ans);
				second = ans / first;
			}
		}

		final = "" + first + sign + second;

		if (!correctQuestion) {
			int changeFirst = GenerateWrongNumber (first);
			int changeSecond = GenerateWrongNumber (second);

			if (changeFirst == changeSecond) {
				changeSecond = GenerateWrongNumber (second);
			}

			first += changeFirst;
			second += changeSecond;
			final = "" + first + sign + second + "INC";
		}

		return final;
	}

	string GenerateThirdLevelQuestion(bool correctQuestion) {
		char sign = '+';
		string final;
		int random = rand.Next (3);
		switch (random) {
		case 0:
			sign = '+';
			break;
		case 1:
			sign = '-';
			break;
		case 2:
			sign = '*';
			break;
		case 3:
			sign = '/';
			break;
		}

		int first, second;
		int ans = int.Parse (answer);

		if (sign == '+') {
			first = rand.Next (Con.Constants.LEVEL3_MIN, ans);
			second = ans - first;
		} else if (sign == '-') {
			first = rand.Next (ans, Con.Constants.LEVEL3_MAX);
			second = Mathf.Abs (ans - first);
		} else if (sign == '*') {
			if (!Con.Constants.IsNotPrime (ans)) {
				first = ans;
				second = 1;
			} else {
				first = Con.Constants.Divide (ans);
				second = ans / first;
			}
		} else {
			if (!Con.Constants.IsNotPrime (ans)) {
				first = ans;
				second = 1;
			} else {
				int multiplier = rand.Next (2, 5);
				first = multiplier * ans;
				second = first / ans;
			}
		}

		final = "" + first + sign + second;

		if (!correctQuestion) {
			int changeFirst = GenerateWrongNumber (first);
			int changeSecond = GenerateWrongNumber (second);

			if (changeFirst == changeSecond) {
				changeSecond = GenerateWrongNumber (second);
			}

			first += changeFirst;
			second += changeSecond;
			final = "" + first + sign + second + "INC";
		}

		return final;
	}
	
	// Update is called once per frame
	void Update () {
		time -= Time.deltaTime;

		if (time < 0f) {
			Debug.Log ("GAME OVER");
		}

		CheckForHits ();
	}
		

	// First level answer is between 5 and 15
	int GenerateFirstLevelAnswer() {
		return rand.Next (Con.Constants.LEVEL1_MIN, Con.Constants.LEVEL1_MAX);
	}

	// Second level answer is between 15 and 30
	int GenerateSecondLevelAnswer() {
		return rand.Next (Con.Constants.LEVEL2_MIN, Con.Constants.LEVEL2_MAX);
	}

	int GenerateThirdLevelAnswer() {
		return rand.Next (Con.Constants.LEVEL3_MIN, Con.Constants.LEVEL3_MAX);
	}

	void LevelUpOrDown(GameObject obj) {
		if (obj.GetComponent<ShapeScript> ().IsCorrect()) {
			if (consecutive <= 0) {
				consecutive = 1;
			} else {
				consecutive++;
			}
		} else {
			if (consecutive >= 0) {
				consecutive = -1;
			} else {
				consecutive--;
			}
		}

		if (consecutive == Con.Constants.CONSECUTIVE_POSITIVE) {
			level++;
			consecutive = 0;
		}

		if (consecutive == Con.Constants.CONSECUTIVE_NEGATIVE) {
			if (level != 1) {
				level--;
			}
			consecutive = 0;
		}
		Debug.Log ("Level: " + level);
	}

	void UpdateScore() {
		score += 4 * level;
	}

	void CheckForHits() {
		
		if( Input.GetMouseButtonDown(0) )
		{
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

			if (hit != null && hit.collider != null) {
				if (hit.transform.gameObject.tag.Equals("Shape")) {
					LevelUpOrDown (hit.transform.gameObject);
					UpdateScore ();
					DestroyShapes ();
					CreateShapes ();
				}
			}
		}
	}
}
