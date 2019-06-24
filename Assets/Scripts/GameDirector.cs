﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

namespace QuizManagement
{
	public class GameDirector : MonoBehaviour
	{
		[SerializeField]
		private Text questionText;
		[SerializeField]
		private Button choice1;
//		private Text choiceText1;
		[SerializeField]
		private Button choice2;
//		private Text choiceText2;
		[SerializeField]
		private Button choice3;
//		private Text choiceText3;
		[SerializeField]
		private GameObject charactor;
		[SerializeField]
		private Text numberOfQuestionsText;
		[SerializeField]
		private Text timeLimitText;
//		[SerializeField]
		//		private GameObject resultPanel;

		[SerializeField]
		private Text correctAnswersText;

		private CharactorController charactorController;
		//	private const int QUIZ_MAX_NUM = 8;
		public const int QUIZ_MAX_NUM = 5;

		//	private TextAsset csvFile;
		private List<string[]> csvDatas = new List<string[]>();
		private QuizMaker quizMaker;
		private Quiz currentQuiz;

		// クイズの回答待ちかどうか
		private bool isAnswerWait = false;

		// 出題済クイズ数
		private int alreadyQuizNum = 0;

		private int correctAnswerNum = 0;

		private bool isQuizEnd = false;

//		private PlayQuizType playQuizType;

//		private bool isResultEnd = false;
/*
		public enum PlayQuizType
		{
			RegularQuiz,
			CareerQuiz
		}
*/
		private QuizOutputStatus quizOutputStatus = QuizOutputStatus.BeforeQuiz;

		public enum QuizOutputStatus
		{
			BeforeQuiz,
			PossibleOutput,
//			QuizOutput,
			AnswerWait,
			QuizEnd,
		}

		// Start is called before the first frame update
		void Start()
		{
			//        this.questionText = GameObject.Find("Question");
			// クイズ情報ロード
			quizMaker = new QuizMaker();
			quizMaker.LoadQuizData();

			// とりあえずデフォルトでレギュラークイズを行う
//			playQuizType = PlayQuizType.RegularQuiz;
			GamePlayInfo.PlayQuizType = GamePlayInfo.QuizType.RegularQuiz;

			this.charactorController = this.charactor.GetComponent<CharactorController>(); 

			StartCoroutine(quizOutputCheck());
//			this.resultPanel.SetActive(false);
		}

		// Update is called once per frame
		void Update()
		{
//			if (this.isQuizEnd == false) {
			/*
			if (this.quizOutputStatus != QuizOutputStatus.QuizEnd) {
				StartCoroutine(quizOutputCheck());
			}
			*/
			/*
			if (this.isResultEnd && charactorController.IsTransitionPossible()) {
				if (Input.GetMouseButtonDown(0)) {
					SceneManager.LoadScene("TitleScene");
				}
			}
			*/
		}

		private IEnumerator quizOutputCheck() {
			Debug.Log("クイズ出題判定");
			// まだクイズ上限数まで出題していない
			if (this.alreadyQuizNum < QUIZ_MAX_NUM) {
				// 回答待ち状態ではない
//				if (this.isAnswerWait == false) 
				if (this.quizOutputStatus == QuizOutputStatus.BeforeQuiz
					|| this.quizOutputStatus == QuizOutputStatus.PossibleOutput) {

					if (this.quizOutputStatus == QuizOutputStatus.PossibleOutput) {
						string idleTag = CharactorController.AnimationTag.Idle.ToString();
						// アニメーションがアイドル状態になるまで待つ

						string answerTag = CharactorController.AnimationTag.Answer.ToString();
						if (charactorController.IsAnimation(answerTag)) {
							yield return new WaitForSeconds(0.2f);
							while (charactorController.IsAnimation(answerTag)) {
								yield return new WaitForSeconds(0.2f);
							}
						} else {
							// 回答後のアニメーションが取得できないので頃合いを見て抜ける(合計2秒待つ)
							Debug.Log("回答後のアニメーションが取得できないので頃合いを見て抜ける");
							yield return new WaitForSeconds(2.0f);
						}
						/*
						while (charactorController.IsAnimation(idleTag) == false) {
							yield return new WaitForSeconds(0.2f);
						}
						*/
						// 回答後のアニメーション完了後の余韻
						yield return new WaitForSeconds(0.5f);
					}

					// クイズ出題状態にする
					//this.isAnswerWait = true;
//					this.quizOutputStatus = QuizOutputStatus.QuizOutput;

					// クイズ出題
					this.quizOutput();
				}
			} else {
				Debug.Log("クイズは終了です！！！");
				StartCoroutine(quizEnd());
//				this.isQuizEnd = true;
				this.quizOutputStatus = QuizOutputStatus.QuizEnd;
			}

			/*
			if (this.isAnswerWait == false) 
			{
				//				this.isAnswerWait = true;
				if (this.alreadyQuizNum < QUIZ_MAX_NUM)
				{
					this.quizOutput();
					this.isAnswerWait = true;
				} else 
				{
					Debug.Log("クイズは終了です！！！");

					//					this.changeUi();
					// TODOゲームを終了してリザルト画面に遷移する
					// クイズ終了処理
					StartCoroutine(quizEnd());
					this.isQuizEnd = true;
				}
			}
			*/
		}

		/**
	 * クイズの出題
	 */
		private void quizOutput()
		{
			this.currentQuiz = quizMaker.CreateQuiz();

			if (currentQuiz == null) {
//				UnityEditor.EditorUtility.DisplayDialog("エラー", "クイズの作成に失敗しました", "OK");
				Debug.Log("クイズの作成に失敗しました！！！");
				StartCoroutine(quizEnd());
				return;
			}
			// ボタンの様態を初期化
			buttonStateChange(0);
			this.questionText.text = currentQuiz.Question;
//			this.choiceText1.text = currentQuiz.Choices[1];
//			this.choiceText2.text = currentQuiz.Choices[2];
//			this.choiceText3.text = currentQuiz.Choices[3];
			this.choice1.GetComponentInChildren<Text>().text = currentQuiz.Choices[1];
			this.choice2.GetComponentInChildren<Text>().text = currentQuiz.Choices[2];
			this.choice3.GetComponentInChildren<Text>().text = currentQuiz.Choices[3];

			this.alreadyQuizNum++;

			this.numberOfQuestionsText.text = "No：" + alreadyQuizNum + " / " + QUIZ_MAX_NUM;

			this.charactorController.Wait();

			Debug.Log("クイズ出題！！！");
//			Debug.Log("回答待ち状態かどうか: " + isAnswerWait);
			Debug.Log("クイズ出題状況：" + quizOutputStatus.ToString());
			Debug.Log("クイズ出題数: " + alreadyQuizNum);
			Debug.Log("クイズ最大数: " + QUIZ_MAX_NUM);

			// TODO 回答をタップ可能にする

			// 回答待ち状態にする
			this.quizOutputStatus = QuizOutputStatus.AnswerWait;

		}

		public void AnswerChoice(int choiceNo)
		{
			Debug.Log("回答ボタン押下！！！");
//			Debug.Log("回答待ち状態かどうか: " + isAnswerWait);
			Debug.Log("クイズ出題状況：" + quizOutputStatus.ToString());
			Debug.Log("クイズ出題数: " + alreadyQuizNum);
			Debug.Log("クイズ最大数: " + QUIZ_MAX_NUM);
//			if (this.isAnswerWait && alreadyQuizNum <= QUIZ_MAX_NUM)
			if (this.quizOutputStatus == QuizOutputStatus.AnswerWait 
				&& alreadyQuizNum <= QUIZ_MAX_NUM) {

				// 回答待ち状態から出題可能状態に変更
				this.quizOutputStatus = QuizOutputStatus.PossibleOutput;

				// ボタンの状態を更新
				buttonStateChange(choiceNo);

				if (choiceNo == currentQuiz.Answer) {
					Debug.Log("正解しました！");

					this.charactorController.CorrectAnswerTrigger();

					this.correctAnswerNum++;
					this.correctAnswersText.text = "正解数：" + this.correctAnswerNum;

				} else {
					Debug.Log("不正解です！");

					this.charactorController.InCorrectAnswerTrigger();

				}
//				this.isAnswerWait = false;
				StartCoroutine(quizOutputCheck());
			}
		}

		private void buttonStateChange(int no) {
			
			if (no == 1) {
				this.choice1.interactable = false;
				this.choice2.enabled = false;
				this.choice3.enabled = false;
			} else if (no == 2) {
				this.choice1.enabled = false;
				this.choice2.interactable = false;
				this.choice3.enabled = false;
			} else if (no == 3) {
				this.choice1.enabled = false;
				this.choice2.enabled = false;
				this.choice3.interactable = false;
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

//			charactorController.ResultTrigger();
//			Debug.Log("結果アニメーション！");
//			long resultWait = 0.0f;
			/*
			while (true) {
				// 結果アニメーションで待機
				if (charactorController.IsResultAnimation()) {
//					Debug.Log("WaitForSecond 0.1");
//					resultWait += Time.deltaTime;
//					yield return null;
					yield return new WaitForSeconds(2.0f);
					Debug.Log("wwhile 抜ける");
					break;
				} else {
					yield return new WaitForSeconds(0.1f);
				}
			}
			*/

			// ##### クイズ完了後のステータス更新 #####
//			int correctDiff = (this.correctAnswerNum * 2) - QUIZ_MAX_NUM;
			StatusController statusController = new StatusController();
			//StatusController.Result statusResult = statusController.StatusUpdate(correctAnswerNum, playQuizType);
			statusController.StatusUpdate(correctAnswerNum);
			/*
			// ランク上下の時のアニメーション
//			if (GamePlayInfo.Result.RankUp == statusResult) {
			if (GamePlayInfo.Result.RankUp == GamePlayInfo.QuizResult) {
				this.charactorController.CareerUpTrigger();

//			} else if (GamePlayInfo.Result.RankDown == statusResult) {
			} else if (GamePlayInfo.Result.RankDown == GamePlayInfo.QuizResult) {
				this.charactorController.CareerDownTrigger();
			}

//			this.result();
			this.isResultEnd = true;
			*/
//			while (charactorController.IsAnswerAnimation()) {

			// アニメーションが回答後にすぐに切り替わってないかもしれないので少し待つ
			yield return new WaitForSeconds(0.4f);
			string answerTag = CharactorController.AnimationTag.Answer.ToString();
			if (charactorController.IsAnimation(answerTag)) {
				yield return new WaitForSeconds(0.2f);
				while (charactorController.IsAnimation(answerTag)) {
					yield return new WaitForSeconds(0.2f);
				}
			} else {
				// 回答後のアニメーションが取得できないので頃合いを見て抜ける(合計2秒待つ)
				Debug.Log("回答後のアニメーションが取得できないので頃合いを見て抜ける");
				yield return new WaitForSeconds(1.6f);

			}
			// アニメーション完了後の余韻の待機
			yield return new WaitForSeconds(0.5f);

			SceneManager.LoadScene("ResultScene");
		}
		/*
		private void result() {


			int ptn = (int)UnityEngine.Random.Range(1, 3);

			if (ptn == 1) {
				charactorController.CareerUpTrigger();
			} else {
				charactorController.CareerDownTrigger();
			}

			this.isResultEnd = true;
		}
		*/
		/*
		private void changeUi() {

			if (this.questionText.enabled) {
				
				this.questionText.enabled = false;

				this.choice1.gameObject.SetActive(false);

				this.choice2.gameObject.SetActive(false);

				this.choice3.gameObject.SetActive(false);

				this.resultPanel.SetActive(true);
			}
		}
		*/
	}
}
