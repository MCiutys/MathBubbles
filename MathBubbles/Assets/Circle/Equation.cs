using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equation {
	private List<int> numbers;
	private char sign;
	private int answer;

	public Equation() {
		numbers = new List<int>();
		sign = '+';
	}

	public Equation(char s, bool correctOrNot) {
		// Create numbers and sign somehow
		numbers = new List<int>();
		sign = s;
		// Depending on boolean, create answer
	}

	public int getFirstNumber() {
		return numbers [0];
	}

	public int getSecondNumber() {
		return numbers [1];
	}

	public char getSign() {
		return sign;
	}
}
