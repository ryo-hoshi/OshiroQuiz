using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuizManagement
{

	public class StatusController
	{
		private const int DAIMYOU_THRESHOLD = 51;
		private const int SYUKUROU_THRESHOLD = 40;
		private const int KAROU_THRESHOLD = 30;
		private const int SAMURAI_DAISYOU_THRESHOLD = 21;
		private const int ASHIGARU_DAISYOU_THRESHOLD = 13;
		private const int ASHIGARU_KUMIGASHIRA_THRESHOLD = 6;

		public enum Career {
			大名 = 7,
			宿老 = 6,
			家老 = 5,
			侍大将 = 4,
			足軽大将 = 3,
			足軽組頭 = 2,
			足軽 = 1,
		}


		private Dictionary<int, int> nextCareerUpExps = new Dictionary<int, int>()
		{
			{(int)Career.宿老, DAIMYOU_THRESHOLD},
			{(int)Career.家老, SYUKUROU_THRESHOLD},
			{(int)Career.侍大将, KAROU_THRESHOLD},
			{(int)Career.足軽大将, SAMURAI_DAISYOU_THRESHOLD},
			{(int)Career.足軽組頭, ASHIGARU_DAISYOU_THRESHOLD},
			{(int)Career.足軽, ASHIGARU_KUMIGASHIRA_THRESHOLD}
		};

		/*
		public const string DAIMYOU = "大名";
		public const string JYOUSYU = "城主";
		public const string JYOUDAI = "城代";
		public const string KAROU = "家老";
		public const string SAMURAI_DAISYOU = "侍大将";
		public const string ASHIGARU_DAISYOU = "足軽大将";
		public const string ASHIGARU_KUMIGASHIRA = "足軽組頭";
		public const string ASHIGARU = "足軽";
		*/

		private const int RANK_CALC_INIT = 5;
		private const int RANK_CALC_STAR_ADD = 15;
		private const int RANK_CALC_STEP = 7;

		private int nowRankStar;
		private int nowRank;
		private int nowRankExp;

		private int nowCareer;
		private int nowCareerExp;

		private int nextRankUpExp;
		//private int nextCarrerUpExp;

		private float nowRankMeter;
		private float nowCareerMeter;

		private int nowCarrerkokudaka;

		SaveData saveData = new SaveData();

		/**
		 * セーブデータ取得
		 */
		private void loadStatus() {

			StatusInfo statusInfo = saveData.GetStatusInfo();

			this.nowRankStar = statusInfo.RankStar;
			this.nowRank = statusInfo.Rank;
			this.nowRankExp = statusInfo.RankExp;
			this.nowRankMeter = statusInfo.RankMeter;
			this.nowCareer = statusInfo.Career;
			this.nowCareerExp = statusInfo.CareerExp;
			this.nowCareerMeter = statusInfo.CareerMeter;
			this.nowCarrerkokudaka = statusInfo.CareerKokudaka;

			Debug.LogWarning("現在のステータス情報ロード直後");
			Debug.LogWarning("nowRankStar:" + this.nowRankStar);
			Debug.LogWarning("nowRank:" + this.nowRank);
			Debug.LogWarning("nowRankExp:" + this.nowRankExp);
			Debug.LogWarning("RankExpMeterの値（バックアップ前）:" + statusInfo.RankMeter);
			Debug.LogWarning("nowCareer:" + this.nowCareer);
			Debug.LogWarning("nowCareerExp:" + this.nowCareerExp);
			Debug.LogWarning("CareerExpMeterの値（バックアップ前）:" + statusInfo.CareerMeter);
		}


//		public Result StatusUpdate(int correctNum, GameDirector.PlayQuizType playQuizType) {
		public void StatusUpdate(int correctNum) {
			// ステータス情報を取得
			this.loadStatus();

			// リザルト画面でのステータス更新演出用の退避
			this.backupBeforeStatus();

			// 次のランクアップに必要な経験値（ステータス更新前の経験値のメーター算出用）
			this.nextRankUpExp = this.calcNextRankUpExp();

//			Result statusResult;
//			Result careerResult = Result.STAY;

			// ランク情報更新
//			Result rankResult = rankUpdate(correctNum);
			this.rankUpdate(correctNum);

			// 階級挑戦クイズの場合は身分情報を更新
//			if (playQuizType == GameDirector.PlayQuizType.CareerQuiz) {
			if (GamePlayInfo.PlayQuizType == GamePlayInfo.QuizType.CareerQuiz) {
				int correctDiff = (correctNum * 2) - GameDirector.QUIZ_MAX_NUM;
				//careerResult = careerUpdate(correctDiff);

				this.careerUpdate(correctDiff);
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

			this.backupAfterStatus();

			saveData.SaveStatusInfo(this.nowRankStar, 
				this.nowRank, 
				this.nowRankExp, 
				GamePlayInfo.AfterRankExpMeter, 
				this.nowCareer, 
				this.nowCareerExp, 
				GamePlayInfo.AfterCareerExpMeter,
				this.nowCarrerkokudaka);

			Debug.LogWarning("ステータス更新完了時");
			Debug.LogWarning("nowRankStar:" + this.nowRankStar);
			Debug.LogWarning("nowRank:" + this.nowRank);
			Debug.LogWarning("nowRankExp:" + this.nowRankExp);
			Debug.LogWarning("RankExpMeterの値（バックアップ後）:" + GamePlayInfo.AfterRankExpMeter);
			Debug.LogWarning("nowCareer:" + this.nowCareer);
			Debug.LogWarning("nowCareerExp:" + this.nowCareerExp);
			Debug.LogWarning("CareerExpMeterの値（バックアップ後）:" + GamePlayInfo.AfterCareerExpMeter);
		}

		private void rankUpdate(int correctNum) {
//			Result rankResult;

			// 現在の経験値と今回獲得分の合計
			int rankExpSum = this.nowRankExp + correctNum;

			// ランクアップ
			if (this.nextRankUpExp <= rankExpSum) {

				if (this.nowRank < 99) {
					this.nowRank++;
				} else {
					this.nowRankStar++;
					this.nowRank = 1;
				}

				this.nowRankExp = rankExpSum - this.nextRankUpExp;
//				rankResult = Result.RankUp;
				GamePlayInfo.QuizResult = GamePlayInfo.Result.RankUp;

				// 次のランクアップに必要な経験値を更新（ランクアップ後の経験値のメーター算出用）
				this.nextRankUpExp = this.calcNextRankUpExp();

			} else {
				this.nowRankExp = rankExpSum;
//				rankResult = Result.STAY;
				GamePlayInfo.QuizResult = GamePlayInfo.Result.STAY;
			}
			// ランク経験値メーターの更新
			this.nowRankMeter = (float)Math.Round((float)this.nowRankExp / this.nextRankUpExp, 2, MidpointRounding.AwayFromZero);

//			return rankResult;
		}

		/**
		 * キャリア情報更新
		 */
		private void careerUpdate(int correctDiff) {
			/*
			int nextCarrerUpExp = 0;
			// 現在の身分が大名未満の場合
			if (this.nowCareer < (int)Career.大名) {
				nextCarrerUpExp = this.calcNextCareerUpExp();


			}
			*/

//			Result careerResult;
			// 現在の経験値と今回獲得分の合計(0より小さくはならないように)
			this.nowCareerExp = this.nowCareerExp + correctDiff;
			if ( this.nowCareerExp < 0) {
				this.nowCareerExp = 0;
			}

			int nextCareer;
			int nextCarrerUpExp = 0;

			if (DAIMYOU_THRESHOLD < this.nowCareerExp) {
				nextCareer = (int)Career.大名;

			} else if (SYUKUROU_THRESHOLD < this.nowCareerExp) {
				nextCareer = (int)Career.宿老;
				nextCarrerUpExp = nextCareerUpExps[(int)Career.宿老];
				 
			} else if (KAROU_THRESHOLD < this.nowCareerExp) {
				nextCareer = (int)Career.家老;
				nextCarrerUpExp = nextCareerUpExps[(int)Career.家老];

			} else if (SAMURAI_DAISYOU_THRESHOLD < this.nowCareerExp) {
				nextCareer = (int)Career.侍大将;
				nextCarrerUpExp = nextCareerUpExps[(int)Career.侍大将];

			} else if (ASHIGARU_DAISYOU_THRESHOLD < this.nowCareerExp) {
				nextCareer = (int)Career.足軽大将;
				nextCarrerUpExp = nextCareerUpExps[(int)Career.足軽大将];

			} else if (ASHIGARU_KUMIGASHIRA_THRESHOLD < this.nowCareerExp) {
				nextCareer = (int)Career.足軽組頭;
				nextCarrerUpExp = nextCareerUpExps[(int)Career.足軽組頭];

			} else {
				nextCareer = (int)Career.足軽;
				nextCarrerUpExp = nextCareerUpExps[(int)Career.足軽];
			}

			// 身分が上下した場合はランクの計算結果でのアニメーション設定を上書きする
			if (nextCareer != this.nowCareer) {
				this.nowCareer = nextCareer;
				// 身分が上がった時
				if (0 < correctDiff) {
					GamePlayInfo.QuizResult = GamePlayInfo.Result.RankUp;

					// 身分が下がった時
				} else {
					GamePlayInfo.QuizResult = GamePlayInfo.Result.RankDown;
				}
			}

			// 身分の経験値メーターを更新
			// 大名は身分が最大なのでMAXのまま
			if (this.nowCareer == (int)Career.大名) {
				this.nowCareerMeter = 1.0f;

				// 
			} else {
				int nowPoint = 0;
				int nextPoint = 0;

				if (this.nowCareer > (int)Career.足軽) {
					// 前回身分が上がった時の経験値からの差分を取得
					int prevCareerUpExp = nextCareerUpExps[nowCareer - 1];
					nowPoint = this.nowCareerExp - prevCareerUpExp;
					nextPoint = nextCarrerUpExp - prevCareerUpExp;

				} else {
					nowPoint = this.nowCareerExp;
					nextPoint = nextCarrerUpExp;
				}
				this.nowCareerMeter = (float)Math.Round((float)nowPoint / nextPoint, 2, MidpointRounding.AwayFromZero);
			}
				
			Debug.Log("career: " + this.nowCareer);
			Debug.Log("nowCareerExp: " + this.nowCareerExp);

//			careerResult = Result.STAY;
//			return careerResult;
		}

		private void backupBeforeStatus() {
			GamePlayInfo.BeforeRankStar = this.nowRankStar;
			GamePlayInfo.BeforeRank = this.nowRank;
			//GamePlayInfo.BeforeRankExpMeter = (float)Math.Round((float)this.nowRankExp / this.nextRankUpExp, 2, MidpointRounding.AwayFromZero);
			GamePlayInfo.BeforeRankExpMeter = this.nowRankMeter;
			GamePlayInfo.BeforeCareer = this.nowCareer;
//			GamePlayInfo.BeforeCareerExpMeter = (float)Math.Round((float)this.nowCareerExp / this.nextCarrerUpExp, 2, MidpointRounding.AwayFromZero);
			GamePlayInfo.BeforeCareerExpMeter = this.nowCareerMeter;
		}

		private void backupAfterStatus() {
			GamePlayInfo.AfterRankStar = this.nowRankStar;
			GamePlayInfo.AfterRank = this.nowRank;
//			GamePlayInfo.AfterRankExpMeter = (float)Math.Round((float)this.nowRankExp / this.nextRankUpExp, 2, MidpointRounding.AwayFromZero);
			GamePlayInfo.AfterRankExpMeter = this.nowRankMeter;
			GamePlayInfo.AfterCareer = this.nowCareer;
//			GamePlayInfo.AfterCareerExpMeter = (float)Math.Round((float)this.nowCareerExp / this.nextCarrerUpExp, 2, MidpointRounding.AwayFromZero);
			GamePlayInfo.AfterCareerExpMeter = this.nowCareerMeter;
		}

		private int calcNextRankUpExp() {
			return RANK_CALC_INIT + (this.nowRankStar * RANK_CALC_STAR_ADD) + (this.nowRank / RANK_CALC_STEP);
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
