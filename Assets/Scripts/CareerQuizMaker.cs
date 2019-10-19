using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using UnityEngine;
using QuizManagement.Api;

namespace QuizManagement
{
	public class CareerQuizMaker: QuizMaker
    {
		// APIから取得した身分クイズデータ（サーバーで選択肢パターンは絞り込み済）
		private Dictionary<int, CareerLoadData> careerQuizDatas;
		// クイズ出題状態保持クラス
		QuizOutputState quizOutputState = new QuizOutputState();

		// 身分クイズ設定処理（APIから取得した処理）
		public void SetCareerQuizDatas(Dictionary<int, CareerLoadData> careerQuizDatas, int[] types) {

			this.careerQuizDatas = careerQuizDatas;

			quizOutputState.allQuestionType = types;
		}

		/**
		 * まだ出題していないtyepの問題をランダムに選択して問題を生成する
		 */
		public Quiz CreateQuiz() {


			if(GamePlayInfo.PlayQuizType == GamePlayInfo.QuizType.CareerQuiz) {
				Debug.Log("身分クイズのデータ数："+careerQuizDatas.Count);
			}

			Quiz quiz = null;

			try{
				// 次のクイズのtypeをランダムに選択する
				int selectType = quizOutputState.NextQuestionType();
				CareerLoadData quizeData = this.careerQuizDatas[selectType];

				quiz = new Quiz(quizeData.question, 
					quizeData.choice1, 
					quizeData.choice2, 
					quizeData.choice3, 
					quizeData.answer);

			} catch (Exception e) {
				Debug.LogWarning("クイズの作成失敗");
				Debug.LogWarning(e.Message);
				Debug.LogWarning(e.StackTrace);
			}

			return quiz;
		}

		public bool IsLoadComplete() {
			return careerQuizDatas == null ? false : true;
		}
	}
}