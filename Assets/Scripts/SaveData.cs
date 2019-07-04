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
				PlayerPrefs.GetInt(quizCareerName, (int)StatusController.Career.足軽),
				PlayerPrefs.GetInt(quizCareerExp, 0)
			);
		}

		public void SaveRankInfo(int rankStar, int rank, int rankExp)
		{
			PlayerPrefs.SetInt(quizRankStar, rankStar);
			PlayerPrefs.SetInt(quizRank, rank);
			PlayerPrefs.SetInt(quizRankExp, rankExp);

			PlayerPrefs.Save();
		}

		public void SaveStatusInfo(int rankStar, int rank, int rankExp, int career, int careerExp)
		{
			PlayerPrefs.SetInt(quizRankStar, rankStar);
			PlayerPrefs.SetInt(quizRank, rank);
			PlayerPrefs.SetInt(quizRankExp, rankExp);
			PlayerPrefs.SetInt(quizCareerName, career);
			PlayerPrefs.SetInt(quizCareerExp, careerExp);

			PlayerPrefs.Save();
		}

		public void ClearStatusInfo()
		{
			// TODO 定数から取得
			SaveStatusInfo(0, 1, 0, (int)StatusController.Career.足軽, 0);

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
