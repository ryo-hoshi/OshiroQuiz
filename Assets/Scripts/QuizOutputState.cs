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
	public class QuizOutputState
    {
		// 出題するクイズ問題を区別するためのtype値
		public int[] allQuestionType  {private get; set;}

		// 出題済クイズタイプ
		private List<int> alreadyQuestionType = new List<int>();

		/**
		 * 出題するクイズタイプを取得する
		 */
		public int NextQuestionType() {
			Debug.Log("◆◆◆nextQuestionType◆◆◆");
			// 全クイズ種別と出題済クイズ種別との差集合を取得
			List<int> restTypeList = this.allQuestionType.Except(alreadyQuestionType).ToList();
			// 種別を確定するためにListのインデックスを取得
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