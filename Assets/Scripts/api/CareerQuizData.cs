﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuizManagement.Api
{
	[System.Serializable]
	public class CareerQuizData
    {
		public List<CareerLoadData> value;

    }

	[System.Serializable]
	public class CareerLoadData
	{
		public int type;

		public string question;

		public string choice1;

		public string choice2;

		public string choice3;

		public int answer;
	}
}