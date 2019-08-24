using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using QuizManagement.Api;

namespace QuizManagement
{
	public class GameDirector : MonoBehaviour
	{
		[SerializeField]
		private Text questionText;
		[SerializeField]
		private Button choice1;

		[SerializeField]
		private Button choice2;

		[SerializeField]
		private Button choice3;

		[SerializeField]
		private GameObject charactor;
		[SerializeField]
		private Text numberOfQuestionsText;
		[SerializeField]
		private Text timeLimitText;

		[SerializeField]
		private Text correctAnswersText;

		[SerializeField]
		private Image timeLimitMeter;

		private CharactorController charactorController;
		public const int QUIZ_MAX_NUM = 5;

		private QuizMaker quizMaker;
		private Quiz currentQuiz;

		// クイズの回答待ちかどうか
		private bool isAnswerWait = false;

		// 出題済クイズ数
		private int alreadyQuizNum = 0;

		private int correctAnswerNum = 0;

		private bool isQuizEnd = false;

		private const int TIME_OVER = 9;

		private IEnumerator timeLimitCoroutine;

		[SerializeField]
		private GameObject api;
		private ApiController apiController;

		[SerializeField]
		private GameObject gameUIPanel;
		[SerializeField]
		private GameObject selectUIPanel;
		[SerializeField]
		private GameObject questionPanel;
		[SerializeField]
		private GameObject statusPanel;
		private StatusPanelController statusPanelController;

		// クイズ出題状態の初期化
		private QuizOutputStatus quizOutputStatus = QuizOutputStatus.BeforeQuiz;

		private float REACH_WARMUP_TIME = 8.0f;

		private float selectTypeElapsedTime = 0.0f;

		private string idleTag = CharactorController.AnimationTag.Idle.ToString();

		private enum QuizOutputStatus
		{
			BeforeQuiz,
			QuizLoad,
			PossibleOutput,
//			QuizOutput,
			AnswerWait,
			QuizEnd,
		}

		// Start is called before the first frame update
		void Start()
		{
			this.selectUIPanel.SetActive(true);
			this.statusPanel.SetActive(true);
			this.gameUIPanel.SetActive(false);
			this.questionPanel.SetActive(false);

			this.charactorController = this.charactor.GetComponent<CharactorController>(); 
			this.apiController = this.api.GetComponent<ApiController>(); 
			this.timeLimitCoroutine = timeLimitCheck();

			this.statusPanelController = this.statusPanel.GetComponent<StatusPanelController>();

            statusOutput();

        }

        // Update is called once per frame
        void Update()
		{
            // クイズ開始前に放置していたときの暇そうなアニメーション
			if (quizOutputStatus == QuizOutputStatus.BeforeQuiz) {
				
				if (charactorController.IsAnimation(idleTag)) {
					this.selectTypeElapsedTime += Time.deltaTime;

					if (this.selectTypeElapsedTime > REACH_WARMUP_TIME) {
						this.selectTypeElapsedTime = 0.0f;

						int numRandom = (int)UnityEngine.Random.Range(1, 3);

						if (numRandom == 1) {
							this.charactorController.WarmUp1Trigger();
						} else {
							this.charactorController.WarmUp2Trigger();
						}
					}
				}
			}
		}


		private void statusOutput() {
			SaveData saveData = new SaveData();
			StatusInfo statusInfo = saveData.GetStatusInfo();

			statusPanelController.StatusOutput(statusInfo.RankStar, 
				statusInfo.Rank, 
				statusInfo.RankMeter, 
				statusInfo.Career, 
				statusInfo.CareerMeter);
		}

		/**
		 * クイズ種類選択
		 */
		public void SelectQuizType(int selectType) {
			// パネルを切り替え
			this.selectUIPanel.SetActive(false);
			this.statusPanel.SetActive(false);
			this.gameUIPanel.SetActive(true);
			this.questionPanel.SetActive(true);

			// ロード中状態に変更
			this.quizOutputStatus = QuizOutputStatus.QuizLoad;

			// 選択したクイズ種類を設定
			if ((int)GamePlayInfo.QuizType.RegularQuiz == selectType) {
				// レギュラークイズ
				GamePlayInfo.PlayQuizType = GamePlayInfo.QuizType.RegularQuiz;
				quizMaker = new RegularQuizMaker();

				// クイズ情報ロード
				((RegularQuizMaker)quizMaker).QuizDataLoad();

			} else {
				// 身分クイズ
				GamePlayInfo.PlayQuizType = GamePlayInfo.QuizType.CareerQuiz;
				quizMaker = new CareerQuizMaker();

				// クイズ情報ロード
				StartCoroutine(this.apiController.CareerQuizLoad((CareerQuizMaker)quizMaker));
			}

			// 出題状況チェックしてクイズを作成
			StartCoroutine(quizOutputCheck());

			this.charactorController.QuizStartTrigger();
		}

		/**
		 * クイズ出題状況をチェックして出題可能ならクイズを作成
		 */
		private IEnumerator quizOutputCheck() {

			// 演出としてゲーム画面表示後、問題を出題するまで少し待つ
			if (this.quizOutputStatus == QuizOutputStatus.QuizLoad) {

				while (true) {
					yield return new WaitForSeconds(0.5f);

					if (quizMaker.IsLoadComplete()) {
						break;
					}
				}

				//TODO 初回はクイズスタート！のような文言を出したい
				yield return new WaitForSeconds(0.5f);
			}


			Debug.Log("クイズ出題判定");
			// まだクイズ上限数まで出題していない
			if (this.alreadyQuizNum < QUIZ_MAX_NUM) {
				// 回答待ち状態ではない
				if (this.quizOutputStatus == QuizOutputStatus.QuizLoad
					|| this.quizOutputStatus == QuizOutputStatus.PossibleOutput) {

					if (this.quizOutputStatus == QuizOutputStatus.PossibleOutput) {
						// アニメーションがアイドル状態になるまで待つ

						// アニメーションが回答後にすぐに切り替わってないかもしれないので少し待つ
						yield return new WaitForSeconds(0.5f);
						string answerTag = CharactorController.AnimationTag.Answer.ToString();
						if (charactorController.IsAnimation(answerTag)) {
							yield return new WaitForSeconds(0.2f);
							while (charactorController.IsAnimation(answerTag)) {
								yield return new WaitForSeconds(0.2f);
							}
						} else {
							// 回答後のアニメーションが取得できないので頃合いを見て抜ける(合計2秒待つ)
							Debug.Log("回答後のアニメーションが取得できないので頃合いを見て抜ける");
							yield return new WaitForSeconds(1.5f);
						}
					}

					// クイズ出題
					this.quizOutput();
				}
			} else {
				Debug.Log("クイズは終了です！！！");
				StartCoroutine(quizEnd());
				this.quizOutputStatus = QuizOutputStatus.QuizEnd;
			}
		}

		/**
	 * クイズの出題
	 */
		private void quizOutput()
		{
			this.charactorController.FaceChange("default");

			this.currentQuiz = quizMaker.CreateQuiz();

			if (currentQuiz == null) {
				Debug.Log("クイズの作成に失敗しました！！！");
				StartCoroutine(quizEnd());
				return;
			}
			// ボタンの状態を初期化
			buttonStateChange(0);
			this.questionText.text = currentQuiz.Question;
			this.choice1.GetComponentInChildren<Text>().text = currentQuiz.Choices[1];
			this.choice2.GetComponentInChildren<Text>().text = currentQuiz.Choices[2];
			this.choice3.GetComponentInChildren<Text>().text = currentQuiz.Choices[3];

			this.alreadyQuizNum++;

			this.numberOfQuestionsText.text = "出題数：" + alreadyQuizNum + " / " + QUIZ_MAX_NUM;

			this.charactorController.Wait();

			Debug.Log("クイズ出題！！！");
			Debug.Log("クイズ出題状況：" + quizOutputStatus.ToString());
			Debug.Log("クイズ出題数: " + alreadyQuizNum);
			Debug.Log("クイズ最大数: " + QUIZ_MAX_NUM);

			// TODO 回答をタップ可能にする

			// 回答待ち状態にする
			this.quizOutputStatus = QuizOutputStatus.AnswerWait;

			StartCoroutine(timeLimitCoroutine);
		}

		/**
		 * 回答
		 */
		public void AnswerChoice(int choiceNo)
		{
			Debug.Log("回答時間制限オーバー");
			if (choiceNo == TIME_OVER) {
				this.charactorController.InCorrectAnswerTrigger();
			} else {
				StopCoroutine(timeLimitCoroutine);
			}

			// 再度実行できるように再取得する
			timeLimitCoroutine = timeLimitCheck();

			Debug.Log("回答ボタン押下！！！");
			Debug.Log("クイズ出題状況：" + quizOutputStatus.ToString());
			Debug.Log("クイズ出題数: " + alreadyQuizNum);
			Debug.Log("クイズ最大数: " + QUIZ_MAX_NUM);
			if (this.quizOutputStatus == QuizOutputStatus.AnswerWait 
				&& alreadyQuizNum <= QUIZ_MAX_NUM) {

				// 回答待ち状態から出題可能状態に変更
				this.quizOutputStatus = QuizOutputStatus.PossibleOutput;

				// ボタンの状態を更新
				buttonStateChange(choiceNo);

				if (choiceNo == currentQuiz.Answer) {
					Debug.Log("正解しました！");

					if (this.alreadyQuizNum == QUIZ_MAX_NUM && this.correctAnswerNum >= 2) {
						// 最終問題かつ正解が多い場合はアニメーションを変える
						this.charactorController.CorrectAnswerAnotherTrigger();
						this.charactorController.FaceChange("smile2");
					} else {
						this.charactorController.CorrectAnswerTrigger();
					}

					this.correctAnswerNum++;
					this.correctAnswersText.text = "正解数：" + this.correctAnswerNum;

				} else {
					Debug.Log("不正解です！");

					if (this.alreadyQuizNum == QUIZ_MAX_NUM && (this.alreadyQuizNum - this.correctAnswerNum) >= 3) {
						// 最終問題かつ不正解が多い場合はアニメーションを変える
						this.charactorController.InCorrectAnswerAnotherTrigger();
						this.charactorController.FaceChange("confuse");
					} else {
						this.charactorController.InCorrectAnswerTrigger();
					}
				}
				// 出題状況チェックしてクイズを作成
				StartCoroutine(quizOutputCheck());
			}
		}

		/**
		 * 時間制限チェック
		 */
		private IEnumerator timeLimitCheck() {
			Debug.LogWarning("時間制限チェック開始");
			float timeLimit = 15.0f;

			while (true) {
				timeLimit -= Time.deltaTime;

				// 制限時間オーバー
				if (timeLimit < 0.0f) {
					
					AnswerChoice(TIME_OVER);
					yield break;
				} else {
					// FillAmountが0-1なのでその中に納まるように調整
					float meterVal = timeLimit / 15;
					Debug.Log("時間制限メーター値：" + meterVal);
					timeLimitMeter.fillAmount = meterVal;
				}
				yield return null;
			}
		}

		private void buttonStateChange(int no) {
			
			if (no == 1) {
				this.choice1.enabled = false;
				this.choice2.interactable = false;
				this.choice3.interactable = false;
			} else if (no == 2) {
				this.choice1.interactable = false;
				this.choice2.enabled = false;
				this.choice3.interactable = false;
			} else if (no == 3) {
				this.choice1.interactable = false;
				this.choice2.interactable = false;
				this.choice3.enabled = false;

				// 初期化
			} else if (no == 0) {
				this.choice1.enabled = true;
				this.choice2.enabled = true;
				this.choice3.enabled = true;
				this.choice1.interactable = true;
				this.choice2.interactable = true;
				this.choice3.interactable = true;
			}
		}

		private IEnumerator quizEnd() {
			

			// ##### クイズ完了後のステータス更新 #####
			StatusController statusController = new StatusController();
			statusController.StatusUpdate(correctAnswerNum);

			// アニメーションが回答後にすぐに切り替わってないかもしれないので少し待つ
			yield return new WaitForSeconds(0.5f);

			// 回答後のアニメーションが終わるまで待つ
			string answerTag = CharactorController.AnimationTag.Answer.ToString();
			if (charactorController.IsAnimation(answerTag)) {
				yield return new WaitForSeconds(0.2f);
				while (charactorController.IsAnimation(answerTag)) {
					yield return new WaitForSeconds(0.2f);
				}
			} else {
				// 回答後のアニメーションが取得できないので頃合いを見て抜ける(合計2秒待つ)
				Debug.Log("回答後のアニメーションが取得できないので頃合いを見て抜ける");
				yield return new WaitForSeconds(1.5f);

			}
//			// アニメーション完了後の余韻の待機
//			yield return new WaitForSeconds(0.2f);

			SceneManager.LoadScene("ResultScene");
		}
	}
}
