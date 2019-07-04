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
		private Image shiroImage;
		[SerializeField]
		private GameObject rankUpParticle;
		[SerializeField]
		private GameObject sound;

		private SoundController soundController;

		// ちょっとずつメーターを更新するための、一度に更新するメータの割合
		private float updateStep = 0.03f;

		// Start is called before the first frame update
		void Start()
		{
			/************************** デバッグ用 ******************************/
			GamePlayInfo.PlayQuizType = GamePlayInfo.QuizType.CareerQuiz;
			GamePlayInfo.QuizResult = GamePlayInfo.Result.RankUp;
			//			GamePlayInfo.QuizResult = GamePlayInfo.Result.RankDown;

			GamePlayInfo.BeforeRankStar = 0;
			GamePlayInfo.BeforeRank = 1;
			GamePlayInfo.BeforeRankExpMeter = 0.3f;
			GamePlayInfo.BeforeCareer = (int)StatusController.Career.足軽;
			GamePlayInfo.BeforeCareerExpMeter = 0.5f;

			GamePlayInfo.AfterRankStar = 1;
			GamePlayInfo.AfterRank = 2;
			GamePlayInfo.AfterRankExpMeter = 0.2f;
			GamePlayInfo.AfterCareer =  (int)StatusController.Career.足軽組頭;
			GamePlayInfo.AfterCareerExpMeter = 0.3f;
			/********************************************************************/
			shiroImage.enabled = false;
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
				SceneManager.LoadScene("TitleScene");
			}
		}


		private IEnumerator resultAnimation() {

			// 画面ロード直後からアニメーション開始までの時間
			yield return new WaitForSeconds(0.5f);

			this.charactorController.ResultTrigger();
			//			long resultWait = 0.0f;

			// ステータス更新演出
			yield return StartCoroutine(statusUpdate());


			yield return new WaitForSeconds(0.3f);
			bool isStatusUpdate = false;

			// ランク(ウデマエ)上下の時のアニメーション
			//			if (GamePlayInfo.Result.RankUp == statusResult) {
			if (GamePlayInfo.Result.RankUp == GamePlayInfo.QuizResult) {

				soundController.RankUp();

				this.charactorController.RankUpTrigger();
				isStatusUpdate = true;

				//			} else if (GamePlayInfo.Result.RankDown == statusResult) {
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

			this.isResultAnimEnd = true;
			Debug.LogWarning("画面遷移可能！！");
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
						if (reminingUpate - updateStep > 0) {
							careerMeter.fillAmount += updateStep;
							reminingUpate -= updateStep;
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
						if (reminingUpate - updateStep > 0) {
							careerMeter.fillAmount -= updateStep;
							reminingUpate -= updateStep;
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
					float reminingUpate = GamePlayInfo.AfterCareerExpMeter - GamePlayInfo.BeforeCareerExpMeter;
					while (true) {
						if (reminingUpate - updateStep > 0) {
							careerMeter.fillAmount += updateStep;
							reminingUpate -= updateStep;
						} else {
							careerMeter.fillAmount += reminingUpate;
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
			if (GamePlayInfo.BeforeRankStar < GamePlayInfo.AfterRankStar
				|| GamePlayInfo.BeforeRank < GamePlayInfo.AfterRank) {

				// メーターを満タンまで上げる
				float reminingUpate = 1.0f - GamePlayInfo.BeforeRankExpMeter;
				while (true) {
					if (reminingUpate - updateStep > 0) {
						rankMeter.fillAmount += updateStep;
						reminingUpate -= updateStep;
					} else {
						rankMeter.fillAmount += reminingUpate;
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
					if (reminingUpate - updateStep > 0) {
						rankMeter.fillAmount += updateStep;
						reminingUpate -= updateStep;
					} else {
						rankMeter.fillAmount += reminingUpate;
						break;
					}
					yield return null;
				}
			}

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
			}
		}

		/**
		 * 更新前のステータス表示
		 */
		private void beforeStatusOutput() {
			this.rankText.text = "ランク " + GamePlayInfo.BeforeRank;
			Debug.LogWarning("ランク経験値　前："+GamePlayInfo.BeforeRankExpMeter);
			rankMeter.fillAmount = GamePlayInfo.BeforeRankExpMeter;

			if (GamePlayInfo.BeforeRankStar > 0) {
				shiroImage.enabled = true;
				Color viewColor = shiroImage.color;
				viewColor.a = 255f;
				shiroImage.color = viewColor;
			}

			StatusController.Career career = (StatusController.Career)Enum.ToObject(typeof(StatusController.Career), GamePlayInfo.BeforeCareer);
			this.careerText.text = "身分 " + career.ToString();
			Debug.LogWarning("身分経験値　後："+GamePlayInfo.BeforeCareerExpMeter);
			careerMeter.fillAmount = GamePlayInfo.BeforeCareerExpMeter;
		}

		private void AfterStatusOutput() {
			this.rankText.text = "ランク " + GamePlayInfo.AfterRank;
			Debug.LogWarning("ランク経験値　前："+GamePlayInfo.AfterRankExpMeter);
			rankMeter.fillAmount = GamePlayInfo.AfterRankExpMeter;

			if (GamePlayInfo.AfterRankStar > 0) {
				shiroImage.enabled = true;
				Color viewColor = shiroImage.color;
				viewColor.a = 255f;
				shiroImage.color = viewColor;
			}

			StatusController.Career career = (StatusController.Career)Enum.ToObject(typeof(StatusController.Career), GamePlayInfo.AfterCareer);
			this.careerText.text = "身分 " + career.ToString();
			careerMeter.fillAmount = GamePlayInfo.AfterCareerExpMeter;
		}
	}
}
