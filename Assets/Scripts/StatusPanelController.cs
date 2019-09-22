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
        private Text castleDominance;

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
//			shiroImage.enabled = false;
	}

        /**
		 * ステータス表示
		 */
        public void StatusOutput(int rank, float rankExpMeter, int careerNum, float careerExpMeter, int castleDominance)
        {
            this.rankText.text = "お城好きレベル　" + rank;
			Debug.LogWarning("お城好きレベル経験値メーター：" + rankExpMeter);
			this.rankMeter.fillAmount = rankExpMeter;

            StatusCalcBasis.Career career = StatusCalcBasis.CareerFromNum(careerNum);
            if (castleDominance > 0 && StatusCalcBasis.Career.大名 == career)
            {
                this.CastleDominanceOutput(castleDominance);
            }
            else {
                this.castleDominance.text = "";
            }
            this.careerText.text = "お城好き階級　" + career.ToString();
            Debug.LogWarning("お城好き階級:" + career);
			Debug.LogWarning("階級経験値メーター:" + careerExpMeter);
			this.careerMeter.fillAmount = careerExpMeter;
		}

		/// <summary>城支配数と大名格表示
		/// <param name="castleNum">城支配数</param>
        /// </summary>
        public void CastleDominanceOutput(int castleNum)
        {
            StatusCalcBasis.DaimyouClass daimyouClass = StatusCalcBasis.DaimyouClassFromCastleNum(castleNum);
            
            this.castleDominance.text = "城支配数:" + castleNum + "  " + daimyouClass.ToString() + "レベル";
        }

		/// <summary>城支配数と大名格表示
		/// <param name="castleNum">城支配数</param>
        /// </summary>
        public void CastleDominanceOutput(int castleNum, string daimyouClass)
        {            
            this.castleDominance.text = "城支配数:" + castleNum + "  " + daimyouClass + "レベル";
        }

		/// <summary>更新対象のメーター値取得
		/// <param name="quizType">クイズ種別</param>
        /// <returns>メーター表示割合</returns>
        /// </summary>
		public float getFillAmount(GamePlayInfo.QuizType quizType)
		{
			return (GamePlayInfo.QuizType.CareerQuiz == quizType) ? careerMeter.fillAmount : rankMeter.fillAmount;
		}

		/// <summary>更新対象のメーター値設定
		/// <param name="quizType">クイズ種別</param>
		/// <param name="fillAmount">メーター表示割合</param>
        /// </summary>
		public void setFillAmount(GamePlayInfo.QuizType quizType, float fillAmount)
		{
			if (GamePlayInfo.QuizType.CareerQuiz == quizType)
			{
				careerMeter.fillAmount = fillAmount;
			}
			else
			{
				rankMeter.fillAmount = fillAmount;
			}
		}
    }
}
