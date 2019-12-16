using Common;
using QuizCollections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx.Async;

namespace QuizManagement
{
	public class ResultDirector : MonoBehaviour
	{
		[SerializeField]
		private CharactorController charactorController;

		private bool isResultAnimEnd = false;

		[SerializeField]
		private Text rankText;
		[SerializeField]
		private Text careerText;
		// [SerializeField]
		// private Image rankMeter;
		// [SerializeField]
		// private Image careerMeter;
        [SerializeField]
		private Particle burstParticle;

		[SerializeField]
		private Text debugText1;
		[SerializeField]
		private Text debugText2;
		[SerializeField]
		private StatusPanelController statusPanelController;

		// Start is called before the first frame update
		async UniTask Start()
		{
			/************************** デバッグ用 ******************************/
			/*
			GamePlayInfo.PlayQuizType = GamePlayInfo.QuizType.CareerQuiz;
			GamePlayInfo.QuizResult = GamePlayInfo.Result.RankUp;
			//			GamePlayInfo.QuizResult = GamePlayInfo.Result.RankDown;

			GamePlayInfo.BeforeRankStar = 0;
			GamePlayInfo.BeforeRank = 99;
			GamePlayInfo.BeforeRankExpMeter = 0.3f;
			GamePlayInfo.BeforeCareer = (int)StatusController.Career.足軽;
			GamePlayInfo.BeforeCareerExpMeter = 0.4f;

			GamePlayInfo.AfterRankStar = 1;
			GamePlayInfo.AfterRank = 1;
			GamePlayInfo.AfterRankExpMeter = 0.1f;
			GamePlayInfo.AfterCareer =  (int)StatusController.Career.足軽;
			GamePlayInfo.AfterCareerExpMeter = 0.8f;
			*/
			/********************************************************************/

			string charactorExist = this.charactorController == null ? "null" : "nullじゃない";
			Debug.LogWarning("結果画面のキャラクター取得：" + charactorExist);
			beforeStatusOutput();

			await resultAnimation();

		}

		// Update is called once per frame
		void Update()
		{
			if (this.isResultAnimEnd && Input.GetMouseButtonDown(0)) {
				SceneManager.LoadScene("GameScene");
			}
		}


		private async UniTask resultAnimation() {

			this.charactorController.ResultTrigger();

			// 画面ロード直後からアニメーション開始までの時間
			await UniTask.Delay(1000);

			// 更新時の効果音（結果に関係なく一旦共通にする）
			SoundController.instance.MeterUp();
			
			// ステータス更新演出
			await statusDisplayUpdate();

			await UniTask.Delay(300);
			bool isStatusUpdate = false;

			// ランクまたは身分(大名格)が上がった時のキャラクターのアニメーション
			if (GamePlayInfo.Result.RankUp == GamePlayInfo.QuizResult) {
				// 喜ぶ声の再生
				SoundController.instance.RankUp();

				this.charactorController.RankUpTrigger();
				isStatusUpdate = true;

			// 身分(大名格)が下がった時のキャラクターのアニメーション
			} else if (GamePlayInfo.Result.RankDown == GamePlayInfo.QuizResult) {
				// 悲しむ声の再生
				SoundController.instance.RankDown();

				this.charactorController.RankDownTrigger();
				isStatusUpdate = true;
			}

			Debug.LogWarning("ステータス更新があるか："+isStatusUpdate);
			// ステータス更新のアニメーションが終わるまで待つ
			// TTODO ちゃんとアニメーションの完了をチェックする
			if (isStatusUpdate) {
				await UniTask.Delay(1000);
			}

			// TODO 後で消す　デバッグ用
			outputDebug();

			this.isResultAnimEnd = true;
		}

		/**
		 * Rank、身分の経験値表示を更新
		 */
		private async UniTask  statusDisplayUpdate() {
			// 経験値メーターを増減させる演出
			// ランクや身分が上下する場合はメーターをいったん空かMAXで止める

            // ************ ランク表示更新演出 ************
			await rankDisplayUpdate();

			// 身分とランクのメーター更新の間の待ち
			await UniTask.Delay(500);

			// ************ 身分表示更新演出 ************
			// bool isCareerUpdate = false;
			// 身分上限かつ経験値メーターが上限から変更していない場合は対象外にする
			if (GamePlayInfo.PlayQuizType == GamePlayInfo.QuizType.CareerQuiz
				&& !(OshiroUtil.IsCareerLimit(GamePlayInfo.BeforeCareer, GamePlayInfo.BeforeCareerExpMeter)
					&& GamePlayInfo.BeforeCareerExpMeter == GamePlayInfo.AfterCareerExpMeter)) {

				if (GamePlayInfo.BeforeCareer == (int)StatusCalcBasis.Career.大名)
				{
					// 城支配数の更新
					await castleDominanceDisplayUpdate();
				}
				else
				{
					// 身分表示更新
					await careerDisplayUpdate();
				}
			}

			await UniTask.Delay(500);

            // ************ ランクや身分上下時のエフェクト表示と最終的なステータス表示 ************
			if (GamePlayInfo.Result.RankUp == GamePlayInfo.QuizResult
			 || GamePlayInfo.Result.RankDown == GamePlayInfo.QuizResult)
			{
				// ランクまたは身分が上がった場合の花火の演出
				if (GamePlayInfo.Result.RankUp == GamePlayInfo.QuizResult)
				{
					SoundController.instance.Fireworks1();
					
					var particle = Instantiate(this.burstParticle);
					particle.transform.position = new Vector3(0.0f, 1.0f, -0.5f);
					particle.GetComponent<ParticleSystem>().Play();

					await UniTask.Delay(1000);

					SoundController.instance.Fireworks2();
				}
				else
				{
					await UniTask.Delay(800);
				}


				// ランクや身分などに上下があった時の最終的なステータス
				AfterStatusOutput();

				// メーターが割れた演出を戻す（今割れているかどうか気にせず固定で指定）
				statusPanelController.setCareerCrack(false);
			}
			// else if (GamePlayInfo.Result.RankDown == GamePlayInfo.QuizResult)
			// {
			// 	await UniTask.Delay(800);
			// 	AfterStatusOutput();
			// }
			// ステータス表示更新で行う
			// else if (GamePlayInfo.AfterCareer == (int)StatusCalcBasis.Career.大名)
            // {
            //     statusPanelController.CastleDominanceOutput(GamePlayInfo.AfterCastleDominance);
            // }
		}

        /// <summary>身分表示更新
        /// </summary>
		private async UniTask  careerDisplayUpdate() {
			// 身分が上がった時
			if (GamePlayInfo.BeforeCareer < GamePlayInfo.AfterCareer) {

				// // メーターアップ時のサウンド
				// SoundController.instance.MeterUp();

				// メーターを満タンまで上た後、0に戻す
				await meterIncrease(GamePlayInfo.QuizType.CareerQuiz, StatusPanel.Fill_AMOUNT_MAX);
				await UniTask.Delay(100);
				statusPanelController.setFillAmount(GamePlayInfo.QuizType.CareerQuiz, StatusPanel.Fill_AMOUNT_BEFORE_DOWN);

				if (GamePlayInfo.AfterCareer == (int)StatusCalcBasis.Career.大名)
				{
					// TODO 大名に上がった場合　メーターの色を変える
				}

				// メーターを一旦止めた所から最終的な値まで上げる
				await UniTask.Delay(200);
				await meterIncrease(GamePlayInfo.QuizType.CareerQuiz, OshiroUtil.AdjustExpMeter(GamePlayInfo.AfterCareerExpMeter));

				// // TODO 身分上がるときのサウンド
				// SoundController.instance.RankUp();

				// 身分が下がった時
			} else if (GamePlayInfo.BeforeCareer > GamePlayInfo.AfterCareer) {

				// メーターダウン時のサウンド
				// SoundController.instance.MeterDown();
				// // いったんメーターアップと共通にしてみる
				// SoundController.instance.MeterUp();

				// メーターを0まで下げた後、MAXに戻す
				await meterDecrease(GamePlayInfo.QuizType.CareerQuiz, StatusPanel.Fill_AMOUNT_MIN);
				await UniTask.Delay(100);
				statusPanelController.setFillAmount(GamePlayInfo.QuizType.CareerQuiz, StatusPanel.Fill_AMOUNT_BEFORE_UP);

				// メーターを一旦止めた所から最終的な値まで下げる
				await UniTask.Delay(200);
				await meterDecrease(GamePlayInfo.QuizType.CareerQuiz, OshiroUtil.AdjustExpMeter(GamePlayInfo.AfterCareerExpMeter));

				// // TODO 身分落ちるときのサウンド
				// SoundController.instance.RankDown();

				// 身分の変化なし
			} else {
				// メーターが増える場合
				if (GamePlayInfo.BeforeCareerExpMeter < GamePlayInfo.AfterCareerExpMeter)
				{
					// // メーターアップ時のサウンド
					// SoundController.instance.MeterUp();
					await UniTask.Delay(300);

					meterIncrease(GamePlayInfo.QuizType.CareerQuiz, OshiroUtil.AdjustExpMeter(GamePlayInfo.AfterCareerExpMeter)).Forget();
				}
				else if (GamePlayInfo.BeforeCareerExpMeter > GamePlayInfo.AfterCareerExpMeter)
				{
					// メーターダウン時のサウンド
					// SoundController.instance.MeterDown();
					// // いったんメーターアップと共通にしてみる
					// SoundController.instance.MeterUp();
					await UniTask.Delay(300);

					meterDecrease(GamePlayInfo.QuizType.CareerQuiz, OshiroUtil.AdjustExpMeter(GamePlayInfo.AfterCareerExpMeter)).Forget();
				}
			}
		}

        /// <summary>ランク表示更新
        /// </summary>
		private async UniTask rankDisplayUpdate()
		{
    		// ランクアップしたとき
            if (GamePlayInfo.BeforeRank < GamePlayInfo.AfterRank) {

				// メーターを満タンまで上げた後、0に戻す
				await meterIncrease(GamePlayInfo.QuizType.RegularQuiz, StatusPanel.Fill_AMOUNT_MAX);
				await UniTask.Delay(100);
				statusPanelController.setFillAmount(GamePlayInfo.QuizType.RegularQuiz, StatusPanel.Fill_AMOUNT_MIN);

				// メーターを一旦止めた所から最終的な値まで上げる
				await UniTask.Delay(300);
				meterIncrease(GamePlayInfo.QuizType.RegularQuiz, OshiroUtil.AdjustExpMeter(GamePlayInfo.AfterRankExpMeter)).Forget();

				// ランク維持
			} else {
				// ランクは減らないので経験値が増えた場合のみメーターを増やす
				if (GamePlayInfo.BeforeRankExpMeter < GamePlayInfo.AfterRankExpMeter)
				{
					await meterIncrease(GamePlayInfo.QuizType.RegularQuiz, OshiroUtil.AdjustExpMeter(GamePlayInfo.AfterRankExpMeter));
				}
			}
		}

		/// <summary>メーターの値を増やす
		/// <param name="quizType">クイズ種別</param>
		/// <param name="upperLimit">メーターを増やす上限</param>
        /// </summary>
		private async UniTask meterIncrease(GamePlayInfo.QuizType quizType, float upperLimit)
		{
			// メーターは0よりちょっと多いところから表示開始
			float nextAmount = StatusPanel.Fill_AMOUNT_BEFORE_DOWN;
			while (nextAmount < upperLimit) {
				nextAmount = Mathf.Min(upperLimit, statusPanelController.getFillAmount(quizType) + StatusPanel.Fill_AMOUNT_UPDATE_STEP);
				statusPanelController.setFillAmount(quizType, nextAmount);
				await UniTask.DelayFrame(1);
			}
		}

		/// <summary>メーターの値を減らす
		/// <param name="quizType">クイズ種別</param>
		/// <param name="upperLimit">メーターを減らす下限</param>
        /// </summary>
		private async UniTask meterDecrease(GamePlayInfo.QuizType quizType, float lowerLimit)
		{
			// float nextAmount = StatusPanel.Fill_AMOUNT_MAX;
			// メーターが止まった状態のMAX表示は0.95fとするので、メーター更新時に一瞬メーターが増えないように考慮
			float nextAmount = StatusPanel.Fill_AMOUNT_BEFORE_UP;
			while (nextAmount > lowerLimit) {
				nextAmount = Mathf.Max(lowerLimit, statusPanelController.getFillAmount(quizType) - StatusPanel.Fill_AMOUNT_UPDATE_STEP);
				statusPanelController.setFillAmount(quizType, nextAmount);
				await UniTask.DelayFrame(1);
			}
		}


		/// <summary>城支配数の更新
        /// </summary>
		private async UniTask castleDominanceDisplayUpdate()
		{
			int beforeCastleNum = GamePlayInfo.BeforeCastleDominance;
			string beforeDaimyouClass = StatusCalcBasis.DaimyouClassFromCastleNum(beforeCastleNum).ToString();

			int afterCastleNum = GamePlayInfo.AfterCastleDominance;
			string afterDaimyouClass = StatusCalcBasis.DaimyouClassFromCastleNum(afterCastleNum).ToString();

//			StatusCalcBasis.DaimyouClass afterDaimyouClass = StatusCalcBasis.DaimyouClassFromCastleNum(afterNum);
//			StatusCalcBasis.DaimyouClass nextDaimyouClass = StatusCalcBasis.DaimyouClassFromCastleNum(beforeNum);

			// TODO UniTaskの中でUIを操作してもよいのか？

			// 上昇時
			if (beforeCastleNum < afterCastleNum)
			{
				// TODO 支配数増加時の音を鳴らす
				SoundController.instance.MeterUp();

				await UniTask.Delay(300);

				for (int castleNum = beforeCastleNum + 1; castleNum <= afterCastleNum; castleNum++)
				{
					statusPanelController.CastleDominanceOutput(castleNum, beforeDaimyouClass);

					await UniTask.Delay(300);
				}

				// TODO 支配数増加時の音を止める

				// 大名格が上がった場合
				if (!beforeDaimyouClass.Equals(afterDaimyouClass))
				{
					// // TODO 大名格が上がった時の音を鳴らす
					// SoundController.instance.MeterUp();
				}
			}
			else 
			{
				// TODO 支配数減少時の音を鳴らす
				SoundController.instance.MeterDown();

				await UniTask.Delay(300);

				for (int castleNum = beforeCastleNum - 1; castleNum >= afterCastleNum; castleNum--)
				{
					statusPanelController.CastleDominanceOutput(castleNum, beforeDaimyouClass);

					await UniTask.Delay(300);
				}

				// TODO 支配数減少時の音を止める

				// 大名格が下がった場合
				if (!beforeDaimyouClass.Equals(afterDaimyouClass) || GamePlayInfo.AfterCastleDominance == 0)
				{
					await UniTask.Delay(500);
					
					// TODO 大名格が下がった時の音を鳴らす
					SoundController.instance.MeterCrack();

					// メーターが割れた演出
					statusPanelController.setCareerCrack(true);

					await UniTask.Delay(600);
				}
			}
		}

		/// <summary>城支配数を増やす
		/// <param name="startCastleNum">クイズ種別</param>
		/// <param name="upperLimit">城支配数を増やす上限</param>
        /// </summary>
		private async UniTask castleDominanceIncrease(int startCastleNum, int upperLimit)
		{
		
		}

		/// <summary>城支配数を減らす
		/// <param name="startCastleNum">クイズ種別</param>
		/// <param name="lowerLimit">城支配数を減らす上限</param>
		/// </summary>
		private async UniTask castleDominanceDecrease(int startCastleNum, int lowerLimit)
		{
		
		}



		/**
		 * 更新前のステータス表示
		 */
		private void beforeStatusOutput() {
			Debug.Log("■■■GamePlayInfo.BeforeDaimyouClass:"+GamePlayInfo.BeforeDaimyouClass);

            statusPanelController.StatusOutput(GamePlayInfo.BeforeRank, 
				OshiroUtil.AdjustExpMeter(GamePlayInfo.BeforeRankExpMeter), 
				GamePlayInfo.BeforeCareer, 
				OshiroUtil.AdjustExpMeter(GamePlayInfo.BeforeCareerExpMeter),
                GamePlayInfo.BeforeCastleDominance,
				GamePlayInfo.BeforeDaimyouClass);
		}

		private void AfterStatusOutput() {

            statusPanelController.StatusOutput(GamePlayInfo.AfterRank, 
				OshiroUtil.AdjustExpMeter(GamePlayInfo.AfterRankExpMeter), 
				GamePlayInfo.AfterCareer, 
				OshiroUtil.AdjustExpMeter(GamePlayInfo.AfterCareerExpMeter),
                GamePlayInfo.AfterCastleDominance,
				GamePlayInfo.AfterDaimyouClass);
            Debug.LogWarning("城支配数更新値：" + GamePlayInfo.AfterCastleDominance);
		}

		private void outputDebug() {

			SaveData saveData = new SaveData();
			StatusInfo statusInfo = saveData.GetStatusInfo();

			this.debugText1.text = "ランクM(前)："+GamePlayInfo.BeforeRankExpMeter + "　ランクM(後)："+GamePlayInfo.AfterRankExpMeter + "　ランク経験値："+statusInfo.RankExp+ " 次のレベルアップの経験値："+StatusCalcBasis.CalcNextRankUpExp(statusInfo.Rank);

			int prevCareerExp = 0;
            int nextCareerUpExp = StatusCalcBasis.NextCareerUpExps[statusInfo.Career];
            if (statusInfo.Career > (int)StatusCalcBasis.Career.足軽)
            {
                prevCareerExp = StatusCalcBasis.NextCareerUpExps[statusInfo.Career - 1];
                nextCareerUpExp -= prevCareerExp;
            }
			this.debugText2.text = "身分M(前)："+GamePlayInfo.BeforeCareerExpMeter + "　身分M(後)："+GamePlayInfo.AfterCareerExpMeter + "　身分経験値："+(statusInfo.CareerExp - prevCareerExp) + " 次の身分アップの経験値："+nextCareerUpExp;
		}
	}
}
