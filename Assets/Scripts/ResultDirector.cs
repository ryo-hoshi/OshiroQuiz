using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx.Async;
using Common;

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

		// ちょっとずつメーターを更新するための、一度に更新するメータの割合
		private const float Fill_AMOUNT_UPDATE_STEP = 0.02f;
		private const float Fill_AMOUNT_MAX = 1.0f;
		private const float Fill_AMOUNT_MIN = 0.0f;

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

			// ステータス更新演出
			await statusDisplayUpdate();

			await UniTask.Delay(300);
			bool isStatusUpdate = false;

			// ランクまたは身分(大名格)が上がった時のキャラクターのアニメーション
			if (GamePlayInfo.Result.RankUp == GamePlayInfo.QuizResult) {

				SoundController.instance.RankUp();

				this.charactorController.RankUpTrigger();
				isStatusUpdate = true;

			// 身分(大名格)が下がった時のキャラクターのアニメーション
			} else if (GamePlayInfo.Result.RankDown == GamePlayInfo.QuizResult) {

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

			// ************ 身分表示更新演出 ************
			// bool isCareerUpdate = false;
			if (GamePlayInfo.PlayQuizType == GamePlayInfo.QuizType.CareerQuiz) {

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

				// 身分とランクのメーター更新の間の待ち
				await UniTask.Delay(1000);
			}

            // ************ ランク表示更新演出 ************
			await rankDisplayUpdate();

            // ************ エフェクト表示と最終的なステータス表示 ************
			if (GamePlayInfo.Result.RankUp == GamePlayInfo.QuizResult
			 || GamePlayInfo.Result.RankDown == GamePlayInfo.QuizResult)
			{
				if (GamePlayInfo.Result.RankUp == GamePlayInfo.QuizResult)
				{
					var particle = Instantiate(this.burstParticle);
					particle.transform.position = new Vector3(0.0f, 1.0f, -0.5f);
					particle.GetComponent<ParticleSystem>().Play();
				}

				await UniTask.Delay(800);

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

				// メーターアップ時のサウンド
				SoundController.instance.MeterUp();

				// メーターを満タンまで上げる
				await meterIncrease(GamePlayInfo.QuizType.CareerQuiz, Fill_AMOUNT_MAX);

				if (GamePlayInfo.AfterCareer == (int)StatusCalcBasis.Career.大名)
				{
					// TODO 大名に上がった場合　メーターの色を変える
				}

				// // TODO 身分上がるときのサウンド
				// SoundController.instance.RankUp();

				// 身分が下がった時
			} else if (GamePlayInfo.BeforeCareer > GamePlayInfo.AfterCareer) {

				// メーターダウン時のサウンド
				// SoundController.instance.MeterDown();
				// いったんメーターアップと共通にしてみる
				SoundController.instance.MeterUp();

				// メーターを0まで下げる
				await meterDecrease(GamePlayInfo.QuizType.CareerQuiz, Fill_AMOUNT_MIN);

				// // TODO 身分落ちるときのサウンド
				// SoundController.instance.RankDown();

				// 身分の変化なし
			} else {
				// メーターが増える場合
				if (GamePlayInfo.BeforeCareerExpMeter < GamePlayInfo.AfterCareerExpMeter)
				{
					// メーターアップ時のサウンド
					SoundController.instance.MeterUp();
					await UniTask.Delay(300);

					await meterIncrease(GamePlayInfo.QuizType.CareerQuiz, GamePlayInfo.AfterCareerExpMeter);
				}
				else if (GamePlayInfo.BeforeCareerExpMeter > GamePlayInfo.AfterCareerExpMeter)
				{
					// メーターダウン時のサウンド
					// SoundController.instance.MeterDown();
					// いったんメーターアップと共通にしてみる
					SoundController.instance.MeterUp();
					await UniTask.Delay(300);

					await meterDecrease(GamePlayInfo.QuizType.CareerQuiz, GamePlayInfo.AfterCareerExpMeter);
				}
			}
		}

        /// <summary>ランク表示更新
        /// </summary>
		private async UniTask rankDisplayUpdate()
		{
    		// ランクアップしたとき
            if (GamePlayInfo.BeforeRank < GamePlayInfo.AfterRank) {

				// メーターを満タンまで上げる
				await meterIncrease(GamePlayInfo.QuizType.RegularQuiz, Fill_AMOUNT_MAX);

				// ランク維持
			} else {
				// ランクは減らないので経験値が増えた場合のみメーターを増やす
				if (GamePlayInfo.BeforeRankExpMeter < GamePlayInfo.AfterRankExpMeter)
				{
					await meterIncrease(GamePlayInfo.QuizType.RegularQuiz, GamePlayInfo.AfterRankExpMeter);
				}
			}
		}

		/// <summary>メーターの値を増やす
		/// <param name="quizType">クイズ種別</param>
		/// <param name="upperLimit">メーターを増やす上限</param>
        /// </summary>
		private async UniTask meterIncrease(GamePlayInfo.QuizType quizType, float upperLimit)
		{
			float nextAmount = Fill_AMOUNT_MIN;
			while (nextAmount < upperLimit) {
				nextAmount = Mathf.Min(upperLimit, statusPanelController.getFillAmount(quizType) + Fill_AMOUNT_UPDATE_STEP);
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
			float nextAmount = Fill_AMOUNT_MAX;
			while (nextAmount > lowerLimit) {
				nextAmount = Mathf.Max(lowerLimit, statusPanelController.getFillAmount(quizType) - Fill_AMOUNT_UPDATE_STEP);
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
				GamePlayInfo.BeforeRankExpMeter, 
				GamePlayInfo.BeforeCareer, 
				GamePlayInfo.BeforeCareerExpMeter,
                GamePlayInfo.BeforeCastleDominance,
				GamePlayInfo.BeforeDaimyouClass);
		}

		private void AfterStatusOutput() {

            statusPanelController.StatusOutput(GamePlayInfo.AfterRank, 
				GamePlayInfo.AfterRankExpMeter, 
				GamePlayInfo.AfterCareer, 
				GamePlayInfo.AfterCareerExpMeter,
                GamePlayInfo.AfterCastleDominance,
				GamePlayInfo.AfterDaimyouClass);
            Debug.LogWarning("城支配数更新値：" + GamePlayInfo.AfterCastleDominance);
		}

		private void outputDebug() {

			SaveData saveData = new SaveData();
			StatusInfo statusInfo = saveData.GetStatusInfo();

			this.debugText1.text = "ランクM(前)："+GamePlayInfo.BeforeRankExpMeter + "　ランクM(後)："+GamePlayInfo.AfterRankExpMeter + "　ランク経験値："+statusInfo.RankExp;
			this.debugText2.text = "身分M(前)："+GamePlayInfo.BeforeCareerExpMeter + "　身分M(後)："+GamePlayInfo.AfterCareerExpMeter + "　身分経験値："+statusInfo.CareerExp;
		}
	}
}
