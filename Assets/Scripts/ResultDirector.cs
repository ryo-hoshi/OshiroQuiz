using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace QuizManagement
{
	public class ResultDirector : MonoBehaviour
	{
		[SerializeField]
		private GameObject charactor;

		private CharactorController charactorController;

		private bool isResultAnimEnd = false;

		[SerializeField]
		private Text rankText;
		[SerializeField]
		private Text careerText;
		[SerializeField]
		private Image rankMeter;
		[SerializeField]
		private Image careerMeter;
        [SerializeField]
		private GameObject rankUpParticle;
		[SerializeField]
		private GameObject sound;
		[SerializeField]
		private GameObject statusPanel;

		[SerializeField]
		private Text debugText1;
		[SerializeField]
		private Text debugText2;

		private StatusPanelController statusPanelController;

		private SoundController soundController;

		// ちょっとずつメーターを更新するための、一度に更新するメータの割合
		private float UPDATE_STEP = 0.03f;

		// Start is called before the first frame update
		void Start()
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
			this.statusPanelController = this.statusPanel.GetComponent<StatusPanelController>(); 

			this.charactorController = this.charactor.GetComponent<CharactorController>();
			string charactorExist = this.charactorController == null ? "null" : "nullじゃない";
			Debug.LogWarning("結果画面のキャラクター取得：" + charactorExist);
			beforeStatusOutput();

			soundController = sound.GetComponent<SoundController>();

			StartCoroutine(resultAnimation());

	}

		// Update is called once per frame
		void Update()
		{
			if (this.isResultAnimEnd && Input.GetMouseButtonDown(0)) {
				SceneManager.LoadScene("GameScene");
			}
		}


		private IEnumerator resultAnimation() {

			// 画面ロード直後からアニメーション開始までの時間
			yield return new WaitForSeconds(0.5f);

			this.charactorController.ResultTrigger();

			// ステータス更新演出
			yield return StartCoroutine(statusUpdate());


			yield return new WaitForSeconds(0.3f);
			bool isStatusUpdate = false;

			// ランクまたは身分が上下の時のアニメーション
			if (GamePlayInfo.Result.RankUp == GamePlayInfo.QuizResult) {

				soundController.RankUp();

				this.charactorController.RankUpTrigger();
				isStatusUpdate = true;

			} else if (GamePlayInfo.Result.RankDown == GamePlayInfo.QuizResult) {

				soundController.RankDown();

				this.charactorController.RankDownTrigger();
				isStatusUpdate = true;
			}

			Debug.LogWarning("ステータス更新があるか："+isStatusUpdate);
			// ステータス更新のアニメーションが終わるまで待つ
			// TTODO ちゃんとアニメーションの完了をチェックする
			if (isStatusUpdate) {
				yield return new WaitForSeconds(1.0f);
			}

			// TODO 後で消す　デバッグ用
			outputDebug();

			this.isResultAnimEnd = true;
		}

		/**
		 * Rank、身分の経験値表示を更新
		 */
		private IEnumerator statusUpdate() {
			// *************************** 身分ステータス更新 ***************************
			bool isCareerUpdate = false;
			if (GamePlayInfo.PlayQuizType == GamePlayInfo.QuizType.CareerQuiz) {
				
				// 身分が上がった時
				if ((int)GamePlayInfo.BeforeCareer < (int)GamePlayInfo.AfterCareer) {
					// メーターを満タンまで上げる
					float reminingUpate = 1.0f - GamePlayInfo.BeforeCareerExpMeter;
					while (true) {
						if (reminingUpate - UPDATE_STEP > 0) {
							careerMeter.fillAmount += UPDATE_STEP;
							reminingUpate -= UPDATE_STEP;
						} else {
							careerMeter.fillAmount += reminingUpate;
							break;
						}
						yield return null;
					}

					// TODO 身分上がるときのサウンド

					isCareerUpdate = true;
					/*
					// メーターが満タンになった時のエフェクト
					GameObject particle = Instantiate(rankUpParticle) as GameObject;
					particle.transform.position = new Vector3(0.0f, 1.0f, -0.5f);
					particle.GetComponent<ParticleSystem>().Play();
					*/

					// 身分が下がった時
				} else if ((int)GamePlayInfo.BeforeCareer > (int)GamePlayInfo.AfterCareer) {
					// メーターを0まで下げる
					float reminingUpate = GamePlayInfo.BeforeCareerExpMeter;
					while (true) {
						if (reminingUpate - UPDATE_STEP > 0) {
							careerMeter.fillAmount -= UPDATE_STEP;
							reminingUpate -= UPDATE_STEP;
						} else {
							careerMeter.fillAmount -= reminingUpate;
							break;
						}
						yield return null;
					}
					// TODO ランク落ちるときのサウンド

					isCareerUpdate = true;


					// 現状維持
				} else {
					float updateAmount = GamePlayInfo.AfterCareerExpMeter - GamePlayInfo.BeforeCareerExpMeter;
					float reminingUpdate = updateAmount < 0 ? -updateAmount : updateAmount;
					while (true) {
						if (reminingUpdate - UPDATE_STEP > 0) {
							if (updateAmount > 0) {
								careerMeter.fillAmount += UPDATE_STEP;
							} else {
								careerMeter.fillAmount -= UPDATE_STEP;
							}
							reminingUpdate -= UPDATE_STEP;
						} else {
							if (updateAmount > 0) {
								careerMeter.fillAmount += UPDATE_STEP;
							} else {
								careerMeter.fillAmount -= reminingUpdate;
							}
							break;
						}
						yield return null;
					}
				}
				// 身分とランクのメーター更新の間の待ち
				yield return new WaitForSeconds(1.0f);
			}

            // *************************** ランクステータス更新 ***************************
            // ランクアップしたとき
            if (GamePlayInfo.BeforeRank < GamePlayInfo.AfterRank) {

				// メーターを満タンまで上げる
				float reminingRankUpate = 1.0f - GamePlayInfo.BeforeRankExpMeter;
				while (true) {
					if (reminingRankUpate - UPDATE_STEP > 0) {
						rankMeter.fillAmount += UPDATE_STEP;
						reminingRankUpate -= UPDATE_STEP;
					} else {
						rankMeter.fillAmount += reminingRankUpate;
						break;
					}
					yield return null;
				}
				/*
				// メーターが満タンになった時のエフェクト
				GameObject particle = Instantiate(rankUpParticle) as GameObject;
				particle.transform.position = new Vector3(0.0f, 1.0f, -0.5f);
				particle.GetComponent<ParticleSystem>().Play();
				*/

				// ランク維持
			} else {
				float reminingUpate = GamePlayInfo.AfterRankExpMeter - GamePlayInfo.BeforeRankExpMeter;
				while (true) {
					if (reminingUpate - UPDATE_STEP > 0) {
						rankMeter.fillAmount += UPDATE_STEP;
						reminingUpate -= UPDATE_STEP;
					} else {
						rankMeter.fillAmount += reminingUpate;
						break;
					}
					yield return null;
				}
			}

            // 変更後のステータス表示
			if (GamePlayInfo.Result.RankUp == GamePlayInfo.QuizResult) {

				// メーターが満タンになった時のエフェクト
				GameObject particle = Instantiate(rankUpParticle) as GameObject;
				particle.transform.position = new Vector3(0.0f, 1.0f, -0.5f);
				particle.GetComponent<ParticleSystem>().Play();

				yield return new WaitForSeconds(0.8f);
				AfterStatusOutput();

			} else if (GamePlayInfo.Result.RankDown == GamePlayInfo.QuizResult) {
				yield return new WaitForSeconds(0.8f);
				AfterStatusOutput();

			} else if (GamePlayInfo.AfterCareer == (int)StatusController.Career.大名)
            {
                statusPanelController.CastleDominanceOutput(GamePlayInfo.AfterCastleDominance);
            }
		}

		/**
		 * 更新前のステータス表示
		 */
		private void beforeStatusOutput() {

            statusPanelController.StatusOutput(GamePlayInfo.BeforeRank, 
				GamePlayInfo.BeforeRankExpMeter, 
				GamePlayInfo.BeforeCareer, 
				GamePlayInfo.BeforeCareerExpMeter,
                GamePlayInfo.BeforeCastleDominance);
		}

		private void AfterStatusOutput() {

            statusPanelController.StatusOutput(GamePlayInfo.AfterRank, 
				GamePlayInfo.AfterRankExpMeter, 
				GamePlayInfo.AfterCareer, 
				GamePlayInfo.AfterCareerExpMeter,
                GamePlayInfo.AfterCastleDominance);
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
