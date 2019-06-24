using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuizManagement
{

	public class StatusController
	{
		public const int DAIMYOU_THRESHOLD = 60;
		public const int JYOUSYU_THRESHOLD = 45;
		public const int JYOUDAI_THRESHOLD = 35;
		public const int KAROU_THRESHOLD = 26;
		public const int SAMURAI_DAISYOU_THRESHOLD = 18;
		public const int ASHIGARU_DAISYOU_THRESHOLD = 11;
		public const int ASHIGARU_KUMIGASHIRA_THRESHOLD = 5;

		public const string DAIMYOU = "大名";
		public const string JYOUSYU = "城主";
		public const string JYOUDAI = "城代";
		public const string KAROU = "家老";
		public const string SAMURAI_DAISYOU = "侍大将";
		public const string ASHIGARU_DAISYOU = "足軽大将";
		public const string ASHIGARU_KUMIGASHIRA = "足軽組頭";
		public const string ASHIGARU = "足軽";

		private int nowRankStar;
		private int nowRank;
		private int nowRankExp;

		private string nowCareer;
		private int nowCareerExp;

		SaveData saveData = new SaveData();

		/**
		 * セーブデータ取得
		 */
		private void loadStatus() {

			StatusInfo statusInfo = saveData.GetStatusInfo();

			this.nowRankStar = statusInfo.RankStar;
			this.nowRank = statusInfo.Rank;
			this.nowRankExp = statusInfo.RankExp;
			this.nowCareer = statusInfo.Career;
			this.nowCareerExp = statusInfo.CareerExp;
		}


//		public Result StatusUpdate(int correctNum, GameDirector.PlayQuizType playQuizType) {
		public void StatusUpdate(int correctNum) {
			// ステータス情報を取得
			loadStatus();

//			Result statusResult;
//			Result careerResult = Result.STAY;

			// ランク情報更新
//			Result rankResult = rankUpdate(correctNum);
			rankUpdate(correctNum);
			// 身分情報更新
//			if (playQuizType == GameDirector.PlayQuizType.CareerQuiz) {
			if (GamePlayInfo.PlayQuizType == GamePlayInfo.QuizType.CareerQuiz) {

				int correctDiff = (correctNum * 2) - GameDirector.QUIZ_MAX_NUM;
				//careerResult = careerUpdate(correctDiff);
				careerUpdate(correctDiff);
			}
			/*
			if (Result.RankDown == careerResult) {
				statusResult = Result.RankDown;
			} else if (Result.RankUp == careerResult || Result.RankUp == rankResult) {
				statusResult = Result.RankUp;
			} else {
				statusResult = Result.STAY;
			}

			return statusResult;
			*/

			saveData.SaveStatusInfo(this.nowRankStar, this.nowRank, this.nowRankExp, this.nowCareer, this.nowCareerExp);
		}

		private void rankUpdate(int correctNum) {
//			Result rankResult;

			// ランクアップに必要な経験値
			int nextRankUpExp = 5 + (nowRankStar * 15) + (nowRank / 7);
			// 現在の経験値と今回獲得分の合計
			int rankExpSum = nowRankExp + correctNum;

			// ランクアップ
			if (nextRankUpExp <= rankExpSum) {

				if (nowRank < 99) {
					nowRank++;
				} else {
					nowRankStar++;
					nowRank = 1;
				}

				nowRankExp = rankExpSum - nextRankUpExp;
//				rankResult = Result.RankUp;
				GamePlayInfo.QuizResult = GamePlayInfo.Result.RankUp;
			} else {
				nowRankExp = rankExpSum;
//				rankResult = Result.STAY;
				GamePlayInfo.QuizResult = GamePlayInfo.Result.STAY;
			}

//			return rankResult;
		}

		private void careerUpdate(int correctDiff) {
//			Result careerResult;
/*
			this.nowCareerExp = this.nowCareerExp + correctDiff;
			this.nowCareerExp = this.nowCareerExp < 0 ? 0 : this.nowCareerExp;
			string nextCareer;

			if (DAIMYOU_THRESHOLD < this.nowCareerExp) {
				nextCareer = DAIMYOU;

			} else if (JYOUSYU_THRESHOLD < this.nowCareerExp) {
				nextCareer = JYOUSYU;
				 
			} else if (JYOUDAI_THRESHOLD < this.nowCareerExp) {
				nextCareer = JYOUDAI;
 
			} else if (KAROU_THRESHOLD < this.nowCareerExp) {
				nextCareer = KAROU;

			} else if (SAMURAI_DAISYOU_THRESHOLD < this.nowCareerExp) {
				nextCareer = SAMURAI_DAISYOU;

			} else if (ASHIGARU_DAISYOU_THRESHOLD < this.nowCareerExp) {
				nextCareer = ASHIGARU_DAISYOU;

			} else if (ASHIGARU_KUMIGASHIRA_THRESHOLD < this.nowCareerExp) {
				nextCareer = ASHIGARU_KUMIGASHIRA;

			} else {
				nextCareer = ASHIGARU;
				
			}

			// 身分が上下した場合
			if (nextCareer != this.nowCareer) {
				this.nowCareer = nextCareer;

				if (0 < correctDiff) {
					result = Result.RankUp;
				} else {
					result = Result.RankDown;
				}
			} else {
				result = Result.STAY;
			}

			saveData.SaveCareerData(this.nowCareer, this.nowCareerExp);

			Debug.Log("career: " + this.nowCareer);
			Debug.Log("nowCareerExp: " + this.nowCareerExp);
*/
//			careerResult = Result.STAY;
//			return careerResult;
		}
		/*
		public enum Result
		{
			RankUp,
			RankDown,
			STAY,
		}
		*/
		/*
		public class CareerData
		{
			public string Career {get; private set;}
			public int Exp {get; private set;}

			public CareerData(string career, int exp)
			{
				Career = career;
				Exp = exp;
			}
		}
		*/
	}
}
