using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equation {
	private char sign;
	private int first;
	private int second;
	private bool correctOrNot;
	private int answer;
	private int level;
	private System.Random rand;
	private List<int> numbers;

	public Equation(int level, bool correct, int ans) {
		this.level = level;
		correctOrNot = correct;
		answer = ans;
		rand = new System.Random ();
		first = 0;
		second = 0;
		sign = '+';
		numbers = new List<int> ();
	}

	public void ChangeLevel(int level) {
		this.level = level;
	}

	public int GetLevel() {
		return level;
	}

	public void ChangeCorrect(bool c) {
		correctOrNot = c;
	}

	public bool GetCorrectness() {
		return correctOrNot;
	}

	public void ChangeAnswer(int ans) {
		answer = ans;
	}

	public int GetAnswer() {
		return answer;
	}
	// Level 1 - (+ or -)
	// Level 2 - (+, - or *)
	// Level 3 - (+, -, * or /)
	// Level 4 - (+, -, *, / or root)
	// Level 5 - (+, -, *, /, root, square)
	private void GenerateSign() { 
		int random = 0;
		switch (level) {
		case 1:
			random = rand.Next (5);
			break;
		case 2:
			random = rand.Next (5);
			break;
		case 3:
			random = rand.Next (5);
			break;
		case 4:
			random = rand.Next (5);
			break;
		}
		sign = Con.Constants.SIGNS [random];
	}

	public void GenerateQuestion() {
		GenerateSign ();
		GenerateNumbers ();
		if (!correctOrNot) {
			first = GenerateWrongNumber (first);
			//Debug.Log ("First: " + first);
			do {
				second = GenerateWrongNumber (second);
			} while (first == second);
		}
	}

	private void GenerateNumbers() {
		switch (sign) {
		case '+':
			GenerateSum ();
			break;
		case '-':
			GenerateDifference ();
			break;
		case '*':
			GenerateMultiplication ();
			break;
		case '/':
			GenerateDivision ();
			break;
		case Con.Constants.ROOT:
			GenerateRoot ();
			break;
		}
	}

	// Generates correct sum
	private void GenerateSum() {
		first = rand.Next (Con.Constants.LEVEL_MIN_MAX[level - 1, 0], answer);

		second = answer - first;
	}

	// Generates correct difference
	private void GenerateDifference() {
	//	if (answer > Con.Constants.LEVEL_MIN_MAX [level, 1])
	//		return;
		first = rand.Next (answer, Con.Constants.LEVEL_MIN_MAX[level - 1, 1]);
		second = Mathf.Abs(answer - first);
		numbers.Add (first);
		numbers.Add (second);
	}

	// Generates correct multiplication
	private void GenerateMultiplication() {
		if (!Con.Constants.IsNotPrime (answer)) {
			first = answer;
			second = 1;
		} else {
			first = Con.Constants.Divide (answer);
			second = answer / first;
		}
		numbers.Add (first);
		numbers.Add (second);
	}

	// Generates correct division
	private void GenerateDivision() {
		if (!Con.Constants.IsNotPrime (answer)) {
			first = answer;
			second = 1;
		} else {
			int multiplier = rand.Next (2, 5);
			first = multiplier * answer;
			second = first / answer;
		}
		numbers.Add (first);
		numbers.Add (second);
	}

	// Generates correct square root
	private void GenerateRoot() {
		second = 0; // will not be used
		first = answer * answer;
		numbers.Add (first);
	}

	// Generates correct square (if impossible, return false)
	private bool GenerateSquare() {
		second = 0; // will not be used
		int counter = 1;

		while (answer >= counter) {
			if (counter * counter == answer) {
				first = counter;
				numbers.Add (first);
				return true;
			}
			counter++;
		}
		return false;
	}

	// Generates wrong number to create incorrect equation
	private int GenerateWrongNumber(int number) {
		int random = rand.Next (Con.Constants.LEVEL_INCORRECTNESS [level - 1, 0], Con.Constants.LEVEL_INCORRECTNESS [level - 1, 1]);
		number += random;
		return number;
	}

	// Returns the length of a whole equation
	public int GetLength() {
		string f = first.ToString ();
		string s = second.ToString ();
		// 1 is added for sign
		if (sign != Con.Constants.ROOT && sign != Con.Constants.SQUARE) {
			return f.Length + 1 + s.Length;
		} else {
			return 1 + f.Length;
		}

		// To be uncommented in the future
		/*int sum = 0;
		foreach (int number in numbers) {
			string numb = numbers.ToString();
			// Add 1 for sign
			sum += numb.Length + 1;
		}*/
		// Minus 1 because there is no sign at the end

	}

	public override string ToString ()
	{
		if (correctOrNot) {
			if (sign != Con.Constants.ROOT) {
				return first + "" + sign + second;
			} else {
				return "" + Con.Constants.ROOT + first;
			}
		} else {
			if (sign != Con.Constants.ROOT) {
				return first + "" + sign + second + "I";
			} else {
				return "" + Con.Constants.ROOT + first + "I";
			}
		}
	}

}
