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
		private const string quizRankMeter = "quiz_rank_meter";

		private const string quizCareerName = "quiz_career_name";
		private const string quizCareerExp = "quiz_career_exp";
		private const string quizCareerMeter = "quiz_career_meter";

		public StatusInfo GetStatusInfo()
		{
			return new StatusInfo(
				PlayerPrefs.GetInt(quizRankStar, 0),
				PlayerPrefs.GetInt(quizRank, 1),
				PlayerPrefs.GetInt(quizRankExp, 0),
				PlayerPrefs.GetFloat(quizRankMeter, 0.0f),
				PlayerPrefs.GetInt(quizCareerName, (int)StatusController.Career.足軽),
				PlayerPrefs.GetInt(quizCareerExp, 0),
				PlayerPrefs.GetFloat(quizCareerMeter, 0.0f)
			);
		}

		public void SaveRankInfo(int rankStar, int rank, int rankExp)
		{
			PlayerPrefs.SetInt(quizRankStar, rankStar);
			PlayerPrefs.SetInt(quizRank, rank);
			PlayerPrefs.SetInt(quizRankExp, rankExp);

			PlayerPrefs.Save();
		}

		public void SaveStatusInfo(int rankStar, int rank, int rankExp, float rankMeter, int career, int careerExp, float careerMeter)
		{
			PlayerPrefs.SetInt(quizRankStar, rankStar);
			PlayerPrefs.SetInt(quizRank, rank);
			PlayerPrefs.SetInt(quizRankExp, rankExp);
			PlayerPrefs.SetFloat(quizRankMeter, rankMeter);
			PlayerPrefs.SetInt(quizCareerName, career);
			PlayerPrefs.SetInt(quizCareerExp, careerExp);
			PlayerPrefs.SetFloat(quizCareerMeter, careerMeter);

			PlayerPrefs.Save();
		}

		public void ClearStatusInfo()
		{
			// TODO 定数から取得
			SaveStatusInfo(0, 1, 0, 0.0f, (int)StatusController.Career.足軽, 0, 0.0f);

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
