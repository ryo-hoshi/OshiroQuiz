using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuizManagement
{
	public class QuizMaker
    {
		const int LOW_TITLE = 0;
		const int COL_TYPE = 0;
		const int COL_QUESTION = 1;
		const int COL_CHOICE_1 = 2;
		const int COL_CHOICE_2 = 3;
		const int COL_CHOICE_3 = 4;
		const int COL_ANSWER = 5;

/*		enum QuestionType {
			MAIN_GEAR = 1,
			WEAPON_STRUCTURE = 2,
			MEAN_QUIZ = 3
		}

		private int[] allQuestionType = {
			(int)QuestionType.MAIN_GEAR,
			(int)QuestionType.WEAPON_STRUCTURE,
			(int)QuestionType.MEAN_QUIZ
		};
*/
		private int[] allQuestionType = {1,2,3,4,5};

		// 出題済クイズタイプ
		private List<int> alreadyQuestionType = new List<int>();

		// CSVから読み込んだ全クイズデータ
		private Dictionary<int, List<string[]>> allQuizDatas = new Dictionary<int, List<string[]>>();


//		private List<string[]> csvQuizDatas;


//		public QuizMaker(List<string[]> csvQuizDatas) 
		//		{
//			this.csvQuizDatas = new List<string[]>();
		//		}

		public void LoadQuizData() {

			// Resouces配下のCSV読み込み
			TextAsset csvFile = Resources.Load("QuizData") as TextAsset;
			StringReader reader = new StringReader(csvFile.text);

			bool isFirstLow = true;
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
				int type = int.Parse(csvquiz[QuizMaker.COL_TYPE]);

				if(allQuizDatas.ContainsKey(type)) {
					allQuizDatas[type].Add(csvquiz);
				} else {
					var quizData = new List<string[]>(){
						csvquiz
					};
					allQuizDatas.Add(type, quizData);
				}

			}
		}

//		public string[] QuizeDataWeaponStructure() {
		//			List<string[]> genreSelectDatas = this.allQuizDatas[(int)QuestionType.WEAPON_STRUCTURE];
//			// TODO Listの件数を取得
//			int index = (int)Random.Range(0, genreSelectDatas.Count);
//			return genreSelectDatas[index];
//		}

		/**
		 * まだ出題していないtyepの問題をランダムに選択して問題を生成する
		 */
		public Quiz CreateQuiz() {

			Quiz quiz = null;

			try{
				// TODO typeをランダムに選択する
				int selectType = nextQuestionType();

				List<string[]> selectTypeQuizs = this.allQuizDatas[selectType];

				int index = (int)UnityEngine.Random.Range(0, selectTypeQuizs.Count);

				string[] quizeData = selectTypeQuizs[index];

				quiz = new Quiz(quizeData[1], quizeData[2], quizeData[3], quizeData[4], quizeData[5]);

			} catch (Exception e){
				Debug.LogError("クイズの作成失敗");
				Debug.LogError(e.Message);
				Debug.LogError(e.StackTrace);
			}

			return quiz;
		}

		/**
		 * 出題するクイズタイプを取得する
		 */
		private int nextQuestionType() {
			Debug.Log("◆◆◆nextQuestionType◆◆◆");
			List<int> restTypeList = this.allQuestionType.Except(alreadyQuestionType).ToList();

			int index = (int)UnityEngine.Random.Range(0, restTypeList.Count);

			Debug.Log("全クイズ種類(allQuestionType.Length）: "+allQuestionType.Length);
			Debug.Log("すでに作成した問題の種類数(alreadyQuestionType.Count）: "+alreadyQuestionType.Count);
			Debug.Log("残りのクイズ種類のリスト数（restTypeList.Count）: "+restTypeList.Count);
			Debug.Log("残りのクイズ種類のリストインデックスの乱数(index）: "+index);
			int nextType = restTypeList[index];

			Debug.Log("使用するクイズ種類(nextType): "+nextType);
			this.alreadyQuestionType.Add(nextType);

			return nextType;
		}
	}
}