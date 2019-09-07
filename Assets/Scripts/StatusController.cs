using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace QuizManagement
{

	public class StatusController
	{
        /*
        // 該当の身分になるために必要な経験値
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
        */

		private int beforeRank;
		private int beforeRankExp;
        private float beforeRankMeter;
        private int beforeCareer;
		private int beforeCareerExp;
        private float beforeCareerMeter;
        private int beforeCastleDominance;
        private int beforeDaimyouClass;

        private int afterRank;
        private int afterRankExp;
        private float afterRankMeter;
        private int afterCareer;
        private int afterCareerExp;
        private float afterCareerMeter;
        private int afterCastleDominance;
        private int afterDaimyouClass;

        //        private int nextRankUpExp;

        SaveData saveData = new SaveData();
        /*
        private const int REIWA_BAKUFU_THRESHOLD = 600;
        private const int TENGABITO_THRESHOLD = 450;
        private const int JYOURAKU_THRESHOLD = 300;
        private const int CHIHOU_TOUITSU_THRESHOLD = 150;
        private const int MEIMON_DAIMYOU_THRESHOLD = 60;
        private const int IKKOKU_TOUITSU_THRESHOLD = 30;
        private const int SYOU_DAIMYOU_THRESHOLD = 5;

        public enum DaimyouClass
        {
            令和幕府 = 8,
            天下人 = 7,
            上洛 = 6,
            地方統一 = 5,
            名門大名 = 4,
            一国統一 = 3,
            小大名 = 2,
            滅亡危機 = 1,
        }

        private Dictionary<int, int> nextDaimyouClassUpCastles = new Dictionary<int, int>()
        {
            {(int)DaimyouClass.天下人, REIWA_BAKUFU_THRESHOLD},
            {(int)DaimyouClass.上洛, TENGABITO_THRESHOLD},
            {(int)DaimyouClass.地方統一, JYOURAKU_THRESHOLD},
            {(int)DaimyouClass.名門大名, CHIHOU_TOUITSU_THRESHOLD},
            {(int)DaimyouClass.一国統一, MEIMON_DAIMYOU_THRESHOLD},
            {(int)DaimyouClass.小大名, IKKOKU_TOUITSU_THRESHOLD},
            {(int)DaimyouClass.滅亡危機, SYOU_DAIMYOU_THRESHOLD}
        };
        */

        /**
		 * セーブデータ取得
		 */
        private void loadStatus() {

			StatusInfo statusInfo = saveData.GetStatusInfo();

			this.beforeRank = statusInfo.Rank;
			this.beforeRankExp = statusInfo.RankExp;
			this.beforeRankMeter = statusInfo.RankMeter;
			this.beforeCareer = statusInfo.Career;
			this.beforeCareerExp = statusInfo.CareerExp;
			this.beforeCareerMeter = statusInfo.CareerMeter;
			this.beforeCastleDominance = statusInfo.CastleDominance;
            this.beforeDaimyouClass = statusInfo.DaimyouClass;

            Debug.LogWarning("現在のステータス情報ロード直後");
			Debug.LogWarning("nowRank:" + this.beforeRank);
			Debug.LogWarning("nowRankExp:" + this.beforeRankExp);
			Debug.LogWarning("RankExpMeterの値（バックアップ前）:" + this.beforeRankMeter);
			Debug.LogWarning("nowCareer:" + this.beforeCareer);
			Debug.LogWarning("nowCareerExp:" + this.beforeCareerExp);
			Debug.LogWarning("CareerExpMeterの値（バックアップ前）:" + this.beforeCareerMeter);
		}

		public void StatusUpdate(int correctNum) {
			// ステータス情報を取得
			this.loadStatus();

            // リザルト画面でのステータス更新演出用の、更新前ステータス退避
//            this.backupBeforeStatus();

//			// 次のランクアップに必要な経験値（ステータス更新前の経験値のメーター算出用）
//			this.nextRankUpExp = this.calcNextRankUpExp();
//            Debug.LogWarning("次のランクアップに必要な経験値：" + this.nextRankUpExp);


			// ランク情報更新
			this.rankUpdate(correctNum);

			// 階級挑戦クイズの場合は身分情報を更新
			if (GamePlayInfo.PlayQuizType == GamePlayInfo.QuizType.CareerQuiz) {

                //careerResult = careerUpdate(correctDiff);

                // 正解不正解の差
                int correctDiff = (correctNum * 2) - GameDirector.QUIZ_MAX_NUM;

                this.careerUpdate(correctDiff);
			}

            this.updateAfterSave();
            /*
            //			this.backupAfterStatus();
            this.backupResultStatusInfo();

            saveData.SaveStatusInfo(this.afterRank, 
				this.afterRankExp,
                this.afterRankMeter, 
				this.afterCareer, 
				this.afterCareerExp,
                this.afterCareerMeter,
				this.afterCastleDominance);

			Debug.LogWarning("ステータス更新完了時");
			Debug.LogWarning("afterRank:" + this.afterRank);
			Debug.LogWarning("afterRankExp:" + this.afterRankExp);
			Debug.LogWarning("RankExpMeterの値（バックアップ後）:" + this.afterRankMeter);
			Debug.LogWarning("afterCareer:" + this.afterCareer);
			Debug.LogWarning("afterCareerExp:" + this.afterCareerExp);
			Debug.LogWarning("CareerExpMeterの値（バックアップ後）:" + this.afterCareerMeter);
            */
		}

        /// <summary>ランク情報更新
        /// <param name="correctNum">正解数</param>  
        /// </summary>
        private void rankUpdate(int correctNum) {

			// 現在の経験値と今回獲得分の合計
			int rankExpSum = this.beforeRankExp + correctNum;

            // 次のランクアップに必要な経験値（ステータス更新前の経験値のメーター算出用）
            int nextRankUpExp = StatusCalcBasis.CalcNextRankUpExp(this.beforeRank);

            int nextRank = this.beforeRank;

            // ランクアップの場合
            if (nextRankUpExp <= rankExpSum) {

                nextRank++;

                this.afterRankExp = rankExpSum - nextRankUpExp;
				GamePlayInfo.QuizResult = GamePlayInfo.Result.RankUp;

				// 次のランクアップに必要な経験値を更新（ランクアップ後の経験値のメーター算出用）
				nextRankUpExp = StatusCalcBasis.CalcNextRankUpExp(this.afterRank);

			} else {
				this.afterRankExp = rankExpSum;
				GamePlayInfo.QuizResult = GamePlayInfo.Result.STAY;
			}
            // 更新後のランク
            this.afterRank = nextRank;
            // ランク更新後の経験値メーターを作成
            this.afterRankMeter = StatusCalcBasis.CalcMeter(this.afterRankExp, nextRankUpExp);
        }

        /// <summary>キャリア情報更新
        /// <param name="correctDiff">正解不正解の差</param>  
        /// </summary>
		private void careerUpdate(int correctDiff) {

            // 計算後の身分、身分経験値、城支配数
            //int nextCareer = 0;
 //           int nextCarrerUpExp = 0;
//            int nextCastleDominance = 0;

            // 計算前が大名の場合は城支配数を加算する
            if (this.beforeCareer == (int)StatusCalcBasis.Career.大名)
            {

                this.updateCastleDominance(correctDiff);
                /*
                // 現在の城支配数にクイズ結果の値を加算
                nextCastleDominance = this.beforeCastleDominance + castleDominanceUpdateAmount(correctDiff);

                // 城支配数がなくなったときは身分を落とす
                if (nextCastleDominance < 1)
                {
                    // 身分
                    this.afterCareer = (int)Career.宿老;
                    // 身分が上がるために必要な経験値
                    nextCarrerUpExp = nextCareerUpExps[(int)Career.宿老];
                    // 身分が上がるために必要な経験値から少し減らした状態を設定（好成績を残せばすぐに上がれるくらい）
                    this.afterCareerExp = nextCareerUpExps[(int)Career.宿老] - 4;
                    nextCastleDominance = 0;
                }
                else
                {
                    this.afterCareer = (int)Career.大名;
                    // 大名中は身分経験値は使用しないが、便宜上閾値の経験値を設定しておく
                    this.afterCareerExp = DAIMYOU_THRESHOLD;
                }
                */
            }
            else
            {
                // 計算前が大名ではない場合は身分経験値を加算する

                this.updateCareerExp(correctDiff);
                /*
                // 現在の身分経験値と今回獲得分を合計(0より小さくはならないように)
                this.afterCareerExp += correctDiff;
                if (this.afterCareerExp < 0)
                {
                    this.afterCareerExp = 0;
                }

                // このケースは宿老から大名に上がった場合のみのはず
                if (DAIMYOU_THRESHOLD < this.afterCareerExp)
                {
                    this.afterCareer = (int)Career.大名;
                    // 大名中は身分経験値は使用しないが、便宜上閾値の経験値を設定しておく
                    this.afterCareerExp = DAIMYOU_THRESHOLD;
                    // 大名に上がったときは城支配数は1からスタートする
                    nextCastleDominance = 1;

                }
                else if (SYUKUROU_THRESHOLD < this.afterCareerExp)
                {
                    this.afterCareer = (int)Career.宿老;
                    nextCarrerUpExp = nextCareerUpExps[(int)Career.宿老];

                }
                else if (KAROU_THRESHOLD < this.afterCareerExp)
                {
                    this.afterCareer = (int)Career.家老;
                    nextCarrerUpExp = nextCareerUpExps[(int)Career.家老];

                }
                else if (SAMURAI_DAISYOU_THRESHOLD < this.afterCareerExp)
                {
                    this.afterCareer = (int)Career.侍大将;
                    nextCarrerUpExp = nextCareerUpExps[(int)Career.侍大将];

                }
                else if (ASHIGARU_DAISYOU_THRESHOLD < this.afterCareerExp)
                {
                    this.afterCareer = (int)Career.足軽大将;
                    nextCarrerUpExp = nextCareerUpExps[(int)Career.足軽大将];

                }
                else if (ASHIGARU_KUMIGASHIRA_THRESHOLD < this.afterCareerExp)
                {
                    this.afterCareer = (int)Career.足軽組頭;
                    nextCarrerUpExp = nextCareerUpExps[(int)Career.足軽組頭];

                }
                else
                {
                    this.afterCareer = (int)Career.足軽;
                    nextCarrerUpExp = nextCareerUpExps[(int)Career.足軽];
                }
                */
            }

            Debug.LogError("beforeDaimyouClass:"+ this.beforeDaimyouClass);
            Debug.LogError("afterDaimyouClass:"+ this.afterDaimyouClass);

            // 身分または大名格が上下した場合はランクの計算結果でのアニメーション設定を上書きする
            if (this.afterCareer != this.beforeCareer
                || ((this.afterCareer == (int)StatusCalcBasis.Career.大名) && (this.beforeDaimyouClass != this.afterDaimyouClass))) {
//				this.beforeCareer = nextCareer;
				// 上がった時
				if (0 < correctDiff) {
					GamePlayInfo.QuizResult = GamePlayInfo.Result.RankUp;

					// 下がった時
				} else {
					GamePlayInfo.QuizResult = GamePlayInfo.Result.RankDown;
                }
			}

//            // 城支配数更新
//            this.afterCastleDominance = nextCastleDominance;

            // 身分の経験値メーターを更新
            // 大名は身分が最大なのでメーターはMAXを設定しておく
            if (this.afterCareer == (int)StatusCalcBasis.Career.大名) {
				this.afterCareerMeter = 1.0f;

			} else {
                // 現在のメーターの数値
                int nowMeterPoint = 0;
                // メーターMAXの時の数値
				int maxMeterPoint = 0;

                // 次の身分に上がるために必要な経験値
                int nextCarrerUpExp = StatusCalcBasis.NextCareerUpExps[this.afterCareer];

                // 一番下の身分より一つでも上がっている場合
                if (this.afterCareer > (int)StatusCalcBasis.Career.足軽) {
					// 今の身分に上がる時の閾値だった経験値を取得
					int prevCareerUpExp = StatusCalcBasis.NextCareerUpExps[afterCareer - 1];
                    // 現在の身分に上がるときの閾値時点が0で、次の身分に上がる時がMAXのゲージ値を取得する
                    nowMeterPoint = this.afterCareerExp - prevCareerUpExp;
                    maxMeterPoint = nextCarrerUpExp - prevCareerUpExp;

				} else {
                    nowMeterPoint = this.afterCareerExp;
                    maxMeterPoint = nextCarrerUpExp;
				}
                // 次に経験値が上がるためのMAX値からの割合値を取得（小数点第2位まで）
                this.afterCareerMeter = StatusCalcBasis.CalcMeter(nowMeterPoint, maxMeterPoint);
			}
				
			Debug.Log("career: " + this.afterCareer);
			Debug.Log("nowCareerExp: " + this.afterCareerExp);

		}

        /// <summary>大名の城支配数を更新
        /// <param name="correctDiff">正解不正解の差</param>
        /// </summary>
        private void updateCastleDominance(int correctDiff)
        {
            // 現在の城支配数にクイズ結果の値を加算
            int nextCastleDominance = this.beforeCastleDominance + castleDominanceUpdateAmount(correctDiff);

            // 城支配数がなくなったときは身分を落とす
            if (nextCastleDominance < 1)
            {
                // 身分
                this.afterCareer = (int)StatusCalcBasis.Career.宿老;
//                // 身分が上がるために必要な経験値
//                nextCarrerUpExp = nextCareerUpExps[(int)Career.宿老];
                // 身分が上がるために必要な経験値から少し減らした状態を設定（好成績を残せばすぐに上がれるくらい）
                this.afterCareerExp = StatusCalcBasis.NextCareerUpExps[this.afterCareer] - 4;
                nextCastleDominance = 0;
            }
            else
            {
                this.afterCareer = (int)StatusCalcBasis.Career.大名;
                // 大名中は身分経験値は使用しないが、便宜上閾値の経験値を設定しておく
                this.afterCareerExp = StatusCalcBasis.NextCareerUpExps[this.afterCareer];
            }

            // 城支配数更新
            this.afterCastleDominance = nextCastleDominance;
            // 大名格更新
            this.afterDaimyouClass = (int)StatusCalcBasis.DaimyouClassFromCastleDominance(this.afterCastleDominance);
        }

        /// <summary>城支配数の更新値を計算
        /// <param name="correctDiff">正解不正解の差</param>
        /// </summary>
        private int castleDominanceUpdateAmount(int correctDiff)
        {
            int updateAmount = Math.Abs(correctDiff);

            // 更新値が1と3の場合は値を散らすために確率で+1する
            if (updateAmount < GameDirector.QUIZ_MAX_NUM)
            {
                updateAmount += (int)UnityEngine.Random.Range(0, 2);
            }

            if (correctDiff < 0)
            {
                updateAmount *= -1;
            }

            return updateAmount;
        }

        /// <summary>宿老以下の身分経験値を更新
        /// <param name="correctDiff">正解不正解の差</param>
        /// </summary>
        private void updateCareerExp(int correctDiff)
        {
            // 現在の身分経験値と今回獲得分を合計(0より小さくはならないように)
            this.afterCareerExp += correctDiff;
            if (this.afterCareerExp < 0)
            {
                this.afterCareerExp = 0;
            }

            // 身分経験値から該当の身分を取得
            this.afterCareer = (int)StatusCalcBasis.CareerFromCareerExp(this.afterCareerExp);
            // 宿老から大名に上がった場合
            if (this.afterCareer == (int)StatusCalcBasis.Career.大名)
            {
                // 大名中は身分経験値は使用しないが、便宜上大名に上がった時の閾値の経験値を設定しておく
                this.afterCareerExp = StatusCalcBasis.NextCareerUpExps[this.afterCareer];

                //                this.afterCareerExp = CareerStatusBasis.NextCareerUpExps[(int)CareerStatusBasis.Career.宿老]; 
                // 大名に上がったときは城支配数は1からスタートする
                this.afterCastleDominance = 1;
                // 大名格を設定（デフォルト値だが念のため）
                this.afterDaimyouClass = (int)StatusCalcBasis.DaimyouClassFromCastleDominance(this.afterCastleDominance);
            }

            /*
            // 宿老から大名に上がった場合
            if (DAIMYOU_THRESHOLD < this.afterCareerExp)
            {
                this.afterCareer = (int)Career.大名;
                // 大名中は身分経験値は使用しないが、便宜上閾値の経験値を設定しておく
                this.afterCareerExp = DAIMYOU_THRESHOLD;
                // 大名に上がったときは城支配数は1からスタートする
                this.afterCastleDominance = 1;

            }
            else if (SYUKUROU_THRESHOLD < this.afterCareerExp)
            {
                this.afterCareer = (int)Career.宿老;
//                nextCarrerUpExp = nextCareerUpExps[(int)Career.宿老];

            }
            else if (KAROU_THRESHOLD < this.afterCareerExp)
            {
                this.afterCareer = (int)Career.家老;
                //                nextCarrerUpExp = nextCareerUpExps[(int)Career.家老];

            }
            else if (SAMURAI_DAISYOU_THRESHOLD < this.afterCareerExp)
            {
                this.afterCareer = (int)Career.侍大将;
                //                nextCarrerUpExp = nextCareerUpExps[(int)Career.侍大将];

            }
            else if (ASHIGARU_DAISYOU_THRESHOLD < this.afterCareerExp)
            {
                this.afterCareer = (int)Career.足軽大将;
                //            nextCarrerUpExp = nextCareerUpExps[(int)Career.足軽大将];

            }
            else if (ASHIGARU_KUMIGASHIRA_THRESHOLD < this.afterCareerExp)
            {
                this.afterCareer = (int)Career.足軽組頭;
                //          nextCarrerUpExp = nextCareerUpExps[(int)Career.足軽組頭];

            }
            else
            {
                this.afterCareer = (int)Career.足軽;
                //    nextCarrerUpExp = nextCareerUpExps[(int)Career.足軽];
            }
            */
        }

        /*
        /// <summary>更新前ステータスバックアップ
        /// </summary>
        private void backupBeforeStatus(int rank, float rankMeter, int career, float careerMeter, int castleDominance)
        {
			GamePlayInfo.BeforeRank = rank;
			GamePlayInfo.BeforeRankExpMeter = rankMeter;
			GamePlayInfo.BeforeCareer = career;
			GamePlayInfo.BeforeCareerExpMeter = careerMeter;
            GamePlayInfo.BeforeCastleDominance = castleDominance;
        }

        /// <summary>更新後ステータスバックアップ
        /// </summary>
        private void backupAfterStatus(int rank, float rankMeter, int career, float careerMeter, int castleDominance)
        {
            GamePlayInfo.AfterRank = rank;
            GamePlayInfo.AfterRankExpMeter = rankMeter;
            GamePlayInfo.AfterCareer = career;
            GamePlayInfo.AfterCareerExpMeter = careerMeter;
            GamePlayInfo.AfterCastleDominance = castleDominance;
        }
        */

        /// <summary>アップデート完了後の保存処理
        /// </summary>
        private void updateAfterSave()
        {
            // 結果画面で利用するためにステータス更新前後の値を保存
            this.backupResultStatusInfo();

            // 更新後のステータス情報をローカルに保存
            // TODO 身分もローカルに保存するか要検討
            saveData.SaveStatusInfo(this.afterRank,
                this.afterRankExp,
                this.afterRankMeter,
                this.afterCareer,
                this.afterCareerExp,
                this.afterCareerMeter,
                this.afterCastleDominance,
                this.afterDaimyouClass);

            Debug.LogWarning("ステータス更新完了時");
            Debug.LogWarning("afterRank:" + this.afterRank);
            Debug.LogWarning("afterRankExp:" + this.afterRankExp);
            Debug.LogWarning("RankExpMeterの値（バックアップ後）:" + this.afterRankMeter);
            Debug.LogWarning("afterCareer:" + this.afterCareer);
            Debug.LogWarning("afterCareerExp:" + this.afterCareerExp);
            Debug.LogWarning("CareerExpMeterの値（バックアップ後）:" + this.afterCareerMeter);
        }

        /// <summary>ステータス更新前後の値を保存
        /// </summary>
        private void backupResultStatusInfo()
        {
            GamePlayInfo.BeforeRank = this.beforeRank;
            GamePlayInfo.BeforeRankExpMeter = this.beforeRankMeter;
            GamePlayInfo.BeforeCareer = this.beforeCareer;
            GamePlayInfo.BeforeCareerExpMeter = this.beforeCareerMeter;
            GamePlayInfo.BeforeCastleDominance = this.beforeCastleDominance;
            GamePlayInfo.AfterRank = this.afterRank;
            GamePlayInfo.AfterRankExpMeter = this.afterRankMeter;
            GamePlayInfo.AfterCareer = this.afterCareer;
            GamePlayInfo.AfterCareerExpMeter = this.afterCareerMeter;
            GamePlayInfo.AfterCastleDominance = this.afterCastleDominance;
        }
	}
}
