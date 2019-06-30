using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

		// Start is called before the first frame update
		void Start()
		{
			this.charactorController = this.charactor.GetComponent<CharactorController>();
			string charactorExist = this.charactorController == null ? "null" : "nullじゃない";
			Debug.LogWarning("結果画面のキャラクター取得：" + charactorExist);
			beforeStatusOutput();
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
				
				this.charactorController.RankUpTrigger();
				isStatusUpdate = true;

				//			} else if (GamePlayInfo.Result.RankDown == statusResult) {
			} else if (GamePlayInfo.Result.RankDown == GamePlayInfo.QuizResult) {
				this.charactorController.RankDownTrigger();
				isStatusUpdate = true;
			}

			Debug.LogWarning("ステータス更新があるか："+isStatusUpdate);
			// ステータス更新のアニメーションが終わるまで待つ
			if (isStatusUpdate) {
				yield return new WaitForSeconds(2.0f);
			}

			this.isResultAnimEnd = true;
			Debug.LogWarning("画面遷移可能！！");
		}

		// Rank、身分の経験値メーターを更新
		private IEnumerator statusUpdate() {
			// ちょっとずつ更新するための、一度に更新するメータの割合
			float updateStep = 0.04f;

			// 身分クイズ
			if (GamePlayInfo.PlayQuizType == GamePlayInfo.QuizType.CareerQuiz) {

			}

			// レギュラークイズ
			// ランクアップしたとき
			if (GamePlayInfo.BeforeRankStar < GamePlayInfo.AfterRankStar
				|| GamePlayInfo.BeforeRank < GamePlayInfo.AfterRank) {

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

				// TODO メーターが満タンになった演出
				yield return new WaitForSeconds(0.5f);
				AfterRankStatusOutput();

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
		}

		private void beforeStatusOutput() {
			this.rankText.text = "ランク " + GamePlayInfo.BeforeRank;
			Debug.LogWarning("ランク経験値　前："+GamePlayInfo.BeforeRankExpMeter);
			rankMeter.fillAmount = GamePlayInfo.BeforeRankExpMeter;

			this.careerText.text = "身分 " + GamePlayInfo.BeforeCareer;
			Debug.LogWarning("身分経験値　後："+GamePlayInfo.BeforeCareerExpMeter);
			careerMeter.fillAmount = GamePlayInfo.BeforeCareerExpMeter;
		}

		private void AfterRankStatusOutput() {
			this.rankText.text = "ランク " + GamePlayInfo.AfterRank;
			Debug.LogWarning("ランク経験値　前："+GamePlayInfo.AfterRankExpMeter);
			rankMeter.fillAmount = GamePlayInfo.AfterRankExpMeter;
		}
	}
}
