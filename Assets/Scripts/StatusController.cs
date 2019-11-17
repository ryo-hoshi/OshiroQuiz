using Common;
using QuizCollections;
using System;
using UnityEngine;

namespace QuizManagement
{
	public class StatusController
	{
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

        SaveData saveData = new SaveData();

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

			// ランク情報更新
			this.rankUpdate(correctNum);

			// 階級挑戦クイズの場合かつ現在上げることができる身分の上限まで達していない場合は身分情報を更新
			if (GamePlayInfo.PlayQuizType == GamePlayInfo.QuizType.CareerQuiz
                && !OshiroUtil.IsCareerLimit(this.beforeCareer, this.beforeCareerMeter)) {

                // 正解不正解の差
                int correctDiff = (correctNum * 2) - GameDirector.QUIZ_MAX_NUM;

                this.careerUpdate(correctDiff);
			}

            this.updateAfterSave();
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

            // 身分情報は変更がないので計算前の値をそのまま設定する
            this.afterCareer = this.beforeCareer;
            this.afterCareerExp = this.beforeCareerExp;
            this.afterCareerMeter = this.beforeCareerMeter;
            this.afterCastleDominance = this.beforeCastleDominance;
            this.afterDaimyouClass = this.beforeDaimyouClass;
        }

        /// <summary>キャリア情報更新
        /// <param name="correctDiff">正解不正解の差</param>  
        /// </summary>
		private void careerUpdate(int correctDiff) {

            // 計算前が大名の場合は城支配数を加算する
            if (this.beforeCareer == (int)StatusCalcBasis.Career.大名)
            {

                this.updateCastleDominance(correctDiff);
            }
            else
            {
                // 計算前が大名ではない場合は身分経験値を加算する
                this.updateCareerExp(correctDiff);
            }

            Debug.Log("beforeDaimyouClass:"+ this.beforeDaimyouClass);
            Debug.Log("afterDaimyouClass:"+ this.afterDaimyouClass);

            // 身分または大名格が上下した場合はランクの計算結果でのアニメーション設定を上書きする
            if (this.beforeCareer < this.afterCareer
                || (this.afterCareer == (int)StatusCalcBasis.Career.大名 && this.beforeDaimyouClass < this.afterDaimyouClass)) {
                // 上がった時
                GamePlayInfo.QuizResult = GamePlayInfo.Result.RankUp;

            } else if (this.beforeCareer > this.afterCareer
                || (this.afterCareer == (int)StatusCalcBasis.Career.大名 && this.beforeDaimyouClass > this.afterDaimyouClass))
            {
                // 下がった時
                GamePlayInfo.QuizResult = GamePlayInfo.Result.RankDown;
            }


            // 身分の経験値メーターを更新
            // 大名は身分が最大なのでメーターはMAXを設定しておく
            if (this.afterCareer == (int)StatusCalcBasis.Career.大名) {
				this.afterCareerMeter = StatusPanel.Fill_AMOUNT_MAX;

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
                    // 現在の身分に上がるときの閾値時点が0で、次の身分に上がる直前がMAXのゲージ値を取得する
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
            this.afterDaimyouClass = (int)StatusCalcBasis.DaimyouClassFromCastleNum(this.afterCastleDominance);
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
            int careerExp = Mathf.Max(0, this.beforeCareerExp + correctDiff);
            // 身分の上限になっている場合は経験値がメーターMAX分よりも多くならないように調整
            if (this.beforeCareer >= GamePlayInfo.CareerLimitNum)
            {
                if (careerExp >= StatusCalcBasis.NextCareerUpExps[this.beforeCareer])
                {
                    careerExp = StatusCalcBasis.NextCareerUpExps[this.beforeCareer] - 1;
                }
            }
            this.afterCareerExp = careerExp;

            // 身分経験値から該当の身分を取得
            this.afterCareer = (int)StatusCalcBasis.CareerFromCareerExp(this.afterCareerExp);
            // 宿老から大名に上がった場合
            if (this.afterCareer == (int)StatusCalcBasis.Career.大名)
            {
                // 大名中は身分経験値は使用しないが、便宜上大名に上がった時の閾値の経験値を設定しておく
                this.afterCareerExp = StatusCalcBasis.NextCareerUpExps[this.afterCareer];

                // 大名に上がったときは城支配数は1からスタートする
                this.afterCastleDominance = 1;
                // 大名格を設定（デフォルト値だが念のため）
                this.afterDaimyouClass = (int)StatusCalcBasis.DaimyouClassFromCastleNum(this.afterCastleDominance);
            }
        }
        

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
            GamePlayInfo.BeforeDaimyouClass = this.beforeDaimyouClass;
            GamePlayInfo.AfterRank = this.afterRank;
            GamePlayInfo.AfterRankExpMeter = this.afterRankMeter;
            GamePlayInfo.AfterCareer = this.afterCareer;
            GamePlayInfo.AfterCareerExpMeter = this.afterCareerMeter;
            GamePlayInfo.AfterCastleDominance = this.afterCastleDominance;
            GamePlayInfo.AfterDaimyouClass = this.afterDaimyouClass;
        }
	}
}
