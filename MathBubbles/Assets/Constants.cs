﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Con
{
	public static class Constants
	{
		public const int LEVEL1_MIN = 5;
		public const int LEVEL1_MAX = 16;
		public const int LEVEL2_MIN = 16;
		public const int LEVEL2_MAX = 50;
		public const int LEVEL3_MIN = 51;
		public const int LEVEL3_MAX = 100;
		public static int[,] LEVEL_MIN_MAX = { { 5, 16 }, { 16, 50 }, { 51, 100 }, { 100, 200 }, { 200, 400 } };
		public static int[,] LEVEL_INCORRECTNESS = { { 1, 2 }, { 2, 4 }, { 4, 8 }, { 8, 16 }, { 16, 32 } };
		public const int CONSECUTIVE_POSITIVE = 3;
		public const int CONSECUTIVE_NEGATIVE = -3;
		public const float VELOCITY_MIN = 1.0f;
		public const float VELOCITY_MAX = 3.0f;
		public const float SMALLEST_SHAPE_SIZE = 1.0f;
		public const float LARGEST_SHAPE_SIZE = 2.0f;
		public const char ROOT = '\u221A';
		public const char SQUARE = '\xB2';
		//public const Color[] COLORS = new Color[] { Color.blue, Color.green, Color.yellow, Color.red };
		public static readonly char[] SIGNS = {'+', '-', '*', '/', ROOT, SQUARE};
		public static readonly string TRIANGLE_SPRITE = "Triangle";
		public static readonly string SQUARE_SPRITE = "Square";
		public static readonly string CIRCLE_SPRITE = "Circle";
		public static readonly string[] SPRITE_NAMES = { TRIANGLE_SPRITE, SQUARE_SPRITE, CIRCLE_SPRITE };

		// Provides a number of questions for a particular level
		public static int GetNumberOfQuestions(int level) {
			return level + 2;
		}

		// Find out if the given number is prime or not
		public static bool IsNotPrime(int number) {
			for (int i = 2; i < number; i++) {
				if (number % i == 0) {
					//Debug.Log ("prime");
					return true;
				}
			}
			return false;
		}

		// Give random number that divides the given number without remainder
		public static int Divide(int number) {
			//Debug.Log (number);

			ArrayList divisible = new ArrayList ();
			System.Random r = new System.Random ();

			for (int i = 2; i < number; i++) {
				if (number % i == 0) {
					Debug.Log ("Added: " + i);
					divisible.Add (i);
				}
			}

			if (divisible.Count != 0) {
				return (int) divisible [r.Next (divisible.Count)];
			}
			return 1;
		}
	}
}

