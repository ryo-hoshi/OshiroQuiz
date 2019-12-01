using QuizCollections;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace QuizManagement
{
	public class RegularQuizMaker: QuizMaker
    {
		const int LOW_TITLE = 0;
		const int COL_TYPE = 0;
		const int COL_QUESTION = 1;
		const int COL_CHOICE_1 = 2;
		const int COL_CHOICE_2 = 3;
		const int COL_CHOICE_3 = 4;
		const int COL_ANSWER = 5;

		// CSVから読み込んだレギュラークイズ全データ
		private Dictionary<int, List<string[]>> regularQuizDatas;
		// クイズ出題状態保持クラス
		QuizOutputState quizOutputState = new QuizOutputState();

		public void QuizDataLoad() {
			SaveData saveData = new SaveData();
			StatusInfo statusInfo = saveData.GetStatusInfo();
			if (statusInfo.Rank < 10)
			{
				quizOutputState.allQuestionType = new int[]{1, 2, 3, 4, 5};
			}
			else
			{
				quizOutputState.allQuestionType = new int[]{1, 2, 3, 4, 5, 6};
			}


			// Resouces配下のCSV読み込み
			TextAsset csvFile = Resources.Load("QuizData") as TextAsset;
			StringReader reader = new StringReader(csvFile.text);

			bool isFirstLow = true;

			regularQuizDatas = new Dictionary<int, List<string[]>>();

			// カンマで分割して1行ずつ読み込み
			while (reader.Peek() != -1)
			{
				string line = reader.ReadLine();
//				Debug.Log("line確認");
//				Debug.Log(line);
				if (isFirstLow) {
					isFirstLow = false;
					continue;
				}
				string[] csvquiz = line.Split(',');
				int type = int.Parse(csvquiz[COL_TYPE]);

				if(regularQuizDatas.ContainsKey(type)) {
					regularQuizDatas[type].Add(csvquiz);
				} else {
					var quizData = new List<string[]>(){
						csvquiz
					};
					regularQuizDatas.Add(type, quizData);
				}
			}
		}

		/**
		 * まだ出題していないtyepの問題をランダムに選択して問題を生成する
		 */
		public Quiz CreateQuiz() {

			Quiz quiz = null;

			try{
				// 次のクイズのtypeをランダムに選択する
				int selectType = quizOutputState.NextQuestionType();

				List<string[]> selectTypeQuizs = this.regularQuizDatas[selectType];

				int index = (int)UnityEngine.Random.Range(0, selectTypeQuizs.Count);

				string[] quizeData = selectTypeQuizs[index];

				quiz = new Quiz(quizeData[COL_QUESTION], 
					quizeData[COL_CHOICE_1], 
					quizeData[COL_CHOICE_2], 
					quizeData[COL_CHOICE_3], 
					int.Parse(quizeData[COL_ANSWER]));

			} catch (Exception e) {
				Debug.LogWarning("クイズの作成失敗");
				Debug.LogWarning(e.Message);
				Debug.LogWarning(e.StackTrace);
			}

			return quiz;
		}

		public bool IsLoadComplete() {
			return regularQuizDatas == null ? false : true;
		}
	}
}