using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuizManagement
{
	public class SaveData
	{
		private const string quizRank = "quiz_rank";
		private const string quizRankExp = "quiz_rank_exp";
		private const string quizRankMeter = "quiz_rank_meter";

		private const string quizCareerName = "quiz_career_name";
		private const string quizCareerExp = "quiz_career_exp";
		private const string quizCareerMeter = "quiz_career_meter";
		private const string quizCastleDominance = "quiz_castle_dominance";
        private const string quizDaimyouClass = "quiz_daimyou_class";

        public StatusInfo GetStatusInfo()
		{
			return new StatusInfo(
				PlayerPrefs.GetInt(quizRank, 1),
				PlayerPrefs.GetInt(quizRankExp, 0),
				PlayerPrefs.GetFloat(quizRankMeter, 0.0f),
				PlayerPrefs.GetInt(quizCareerName, (int)StatusCalcBasis.Career.足軽),
				PlayerPrefs.GetInt(quizCareerExp, 0),
				PlayerPrefs.GetFloat(quizCareerMeter, 0.0f),
				PlayerPrefs.GetInt(quizCastleDominance, 0),
                PlayerPrefs.GetInt(quizDaimyouClass, (int)StatusCalcBasis.DaimyouClass.滅亡危機)
            );
		}

        public void SaveStatusInfo(int rank, int rankExp, float rankMeter, 
            int career, int careerExp, float careerMeter, int castleDominance, int daimyouClass)
        {
			PlayerPrefs.SetInt(quizRank, rank);
			PlayerPrefs.SetInt(quizRankExp, rankExp);
			PlayerPrefs.SetFloat(quizRankMeter, rankMeter);
			PlayerPrefs.SetInt(quizCareerName, career);
			PlayerPrefs.SetInt(quizCareerExp, careerExp);
			PlayerPrefs.SetFloat(quizCareerMeter, careerMeter);
			PlayerPrefs.SetInt(quizCastleDominance, castleDominance);
            PlayerPrefs.SetInt(quizDaimyouClass, daimyouClass);

            PlayerPrefs.Save();
		}

		public void ClearStatusInfo()
		{
			// TODO 定数から取得
			SaveStatusInfo(1, 0, 0.0f, (int)StatusCalcBasis.Career.足軽, 0, 0.0f, 0, (int)StatusCalcBasis.DaimyouClass.滅亡危機);

			PlayerPrefs.Save();
		}
	}
}
