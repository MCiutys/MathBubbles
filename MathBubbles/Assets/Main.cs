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

	private double[] GetRandomPositions() {
		double x = 0.4 * rand.NextDouble () + 0.3;
		double y = rand.NextDouble ();
		double[] pos = new double[2];
		pos [0] = x;
		pos [1] = y;
		return pos;
	}

	// Checks if new position passes requirements
	private bool IsNewPositionValid(double[,] positions, double[] newPos) {
		//Debug.Log ("Inside check new position");
		for (int i = 0; i < positions.GetLength (0); i++) {
			double diff1 = Mathf.Abs ((float) (positions [i, 0] - newPos [0]));
			double diff2 = Mathf.Abs ((float) (positions [i, 1] - newPos [1]));

			if (diff1 < 0.2 && diff2 < 0.2)
				return false;
		}
		return true;
	}

	void CreateShapes() {

		answer = GenerateAnswer (level).ToString ();
			
		int numberOfShapes = Con.Constants.GetNumberOfQuestions (level);
		double[,] positions = new double[numberOfShapes,2];



		for (int i = 0; i < numberOfShapes; i++) {
			GameObject shape = (GameObject) Instantiate(Resources.Load("Circle"), new Vector3(i * 4, 0, 0), Quaternion.identity);
			ShapeScript script = shape.GetComponent<ShapeScript> ();
			script.UploadColor (colors [rand.Next(colors.Length)]);

			script.UploadSprite (Con.Constants.SPRITE_NAMES[rand.Next (Con.Constants.SPRITE_NAMES.GetLength(0))]);

			double[] newPos = new double[2];
			newPos = GetRandomPositions ();

			while (!IsNewPositionValid(positions, newPos)) {
				newPos = GetRandomPositions ();
			}


			positions [i, 0] = newPos [0];
			positions [i, 1] = newPos [1];

			script.UploadPosition (newPos [0], newPos [1]);

			if (i == 0) {
				script.UpdateEqu(level, true, int.Parse(answer));
			} else {
				script.UpdateEqu(level, false, int.Parse(answer));
			}
				
			shapes.Add (shape);
			Debug.Log ("Create shapes method finished");
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

	// Update is called once per frame
	void Update () {
		time -= Time.deltaTime;

		if (time < 0f) {
			Debug.Log ("GAME OVER");
		}

		CheckForHits ();
	}

	int GenerateAnswer(int level) {
		return rand.Next (Con.Constants.LEVEL_MIN_MAX [level - 1, 0], Con.Constants.LEVEL_MIN_MAX [level - 1, 1]);
	}
		

	// First level answer is between 5 and 15
	/*int GenerateFirstLevelAnswer() {
		return rand.Next (Con.Constants.LEVEL_MIN_MAX[0, 0], Con.Constants.LEVEL_MIN_MAX[0, 1]);
	}

	// Second level answer is between 15 and 30
	int GenerateSecondLevelAnswer() {
		return rand.Next (Con.Constants.LEVEL_MIN_MAX[1, 0], Con.Constants.LEVEL_MIN_MAX[1, 1]);
	}

	int GenerateThirdLevelAnswer() {
		return rand.Next (Con.Constants.LEVEL_MIN_MAX[2, 0], Con.Constants.LEVEL_MIN_MAX[2, 1]);
	}

	int GenerateFourthLevelAnswer() {
		return rand.Next (Con.Constants.LEVEL_MIN_MAX[3, 0], Con.Constants.LEVEL_MIN_MAX[3, 1]);
	}*/

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
		Debug.Log ("Level Up or Down Finished");
	}

	void UpdateScore() {
		score += 4 * level;
		Debug.Log ("Update score finished");
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
