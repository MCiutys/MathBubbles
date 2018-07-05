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

	public Equation(int level, bool correct, int ans) {
		this.level = level;
		correctOrNot = correct;
		answer = ans;
		rand = new System.Random ();
		GenerateQuestion ();
	}
	// Level 1 - (+ or -)
	// Level 2 - (+, - or *)
	// Level 3 - (+, -, * or /)
	private void GenerateSign() { 
		int random = 0;
		switch (level) {
		case 1:
			random = rand.Next (2);
			break;
		case 2:
			random = rand.Next (3);
			break;
		case 3:
			random = rand.Next (4);
			break;
		}
		sign = Con.Constants.SIGNS [random];
	}

	private void GenerateQuestion() {
		GenerateSign ();
		GenerateNumbers ();
		if (!correctOrNot) {
			Debug.Log ("----------------------------");
			first = GenerateWrongNumber (first);
			do {
				second = GenerateWrongNumber (second);
			} while (first == second);
			Debug.Log ("----------------------------");
		}
	}

	private void GenerateNumbers() {
		switch (sign) {
		case '+':
			GenerateSum (4);
			break;
		case '-':
			GenerateDifference (20);
			break;
		case '*':
			GenerateMultiplication ();
			break;
		case '/':
			GenerateDivision ();
			break;
		}
	}

	// Generates correct sum
	private void GenerateSum(int min) {
		first = rand.Next (min, answer);
		second = answer - first;
	}

	// Generates correct difference
	private void GenerateDifference(int max) {
		first = rand.Next (answer, max);
		second = Mathf.Abs(answer - first);
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
	}

	// Generates wrong number to create incorrect equation
	private int GenerateWrongNumber(int number) {
		int random = rand.Next (1, 5);
		Debug.Log ("random: " + random);
		number += random;
		return number;
	}

	public override string ToString ()
	{
		return first + "" + sign + second;
	}

}
