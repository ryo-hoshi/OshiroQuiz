using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace QuizManagement
{
	public class StatusPanelController : MonoBehaviour
	{
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

		// Start is called before the first frame update
		void Start()
		{
			/************************** デバッグ用 ******************************/
			/*
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
			*/
			/********************************************************************/
			shiroImage.enabled = false;
	}

		/**
		 * ステータス表示
		 */
		public void StatusOutput(int rankStar, int rank, float rankExpMeter, int careerNum, float careerExpMeter) {
			this.rankText.text = "お城好きレベル：" + rank;
			Debug.LogWarning("お城好きレベル経験値：" + rankExpMeter);
			rankMeter.fillAmount = rankExpMeter;

			if (rankStar > 0) {
				shiroImage.enabled = true;
				Color viewColor = shiroImage.color;
				viewColor.a = 255f;
				shiroImage.color = viewColor;
			}

			StatusController.Career career = (StatusController.Career)Enum.ToObject(typeof(StatusController.Career), careerNum);
			this.careerText.text = "お城好き階級：" + career.ToString();
			Debug.LogWarning("階級経験値：" + careerExpMeter);
			careerMeter.fillAmount = careerExpMeter;
		}
	}
}
