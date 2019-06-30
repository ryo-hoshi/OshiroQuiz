using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuizManagement
{
	public class SaveData
	{
		private const string quizRankStar = "quiz_rank_star";
		private const string quizRank = "quiz_rank";
		private const string quizRankExp = "quiz_rank_exp";

		private const string quizCareerName = "quiz_career_name";
		private const string quizCareerExp = "quiz_career_exp";

		public StatusInfo GetStatusInfo()
		{
			return new StatusInfo(
				PlayerPrefs.GetInt(quizRankStar, 0),
				PlayerPrefs.GetInt(quizRank, 1),
				PlayerPrefs.GetInt(quizRankExp, 0),
				PlayerPrefs.GetString(quizCareerName, StatusController.ASHIGARU),
				PlayerPrefs.GetInt(quizCareerExp, 0)
			);
		}

		public void SaveStatusInfo(int rankStar, int rank, int rankExp, string careerName, int careerExp)
		{
			PlayerPrefs.SetInt(quizRankStar, rankStar);
			PlayerPrefs.SetInt(quizRank, rank);
			PlayerPrefs.SetInt(quizRankExp, rankExp);
			PlayerPrefs.SetString(quizCareerName, careerName);
			PlayerPrefs.SetInt(quizCareerExp, careerExp);

			PlayerPrefs.Save();
		}

		/*
		public void SaveCareerData(string career, int exp)
		{
			PlayerPrefs.SetString(quizCareerName, career);
			PlayerPrefs.SetInt(quizCareerExp, exp);

			PlayerPrefs.Save();
		}
		*/

		/*
		public UdemaeInfo GetUdemaeInfo()
		{

		}
		*/
	}
}
