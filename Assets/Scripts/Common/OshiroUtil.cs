using System;
using UnityEngine;

namespace Common
{
	public class OshiroUtil
    {

		/// <summary>現在上げることができる階級の上限まで達しているかどうかのチェック
		/// <param name="careerNum">身分</param>
		/// <param name="careerExpMeter">身分経験値</param>
        /// <returns>true:上限になっている、false:上限になっていない</returns>
        /// </summary>
		public static bool IsCareerLimit(int careerNum, float careerExpMeter)
		{
			// 身分上限のメーターチェックはメーター止まった状態の最大値と比較する
			if (careerNum < (int)StatusCalcBasis.Career.大名
				&& careerNum >= OshiroRemoteConfig.Instance().CareerUpLimit
				&& careerExpMeter >= StatusPanel.Fill_AMOUNT_BEFORE_UP)
			{
				return true;
			}
			else 
			{
				return false;
			}
		}

		/// <summary>経験値メーター値の調整
		/// <param name="expMeter">メーター値</param>
        /// <returns>調整後のメーター値</returns>
        /// </summary>
		public static float AdjustExpMeter(float expMeter)
		{
			// 更新中以外のメーター値はMAXとMINから少し補正する
			// MAX値補正
			if (expMeter > StatusPanel.Fill_AMOUNT_BEFORE_UP)
			{
				return StatusPanel.Fill_AMOUNT_BEFORE_UP;
			}
			// MIN値補正
			if (expMeter < StatusPanel.Fill_AMOUNT_BEFORE_DOWN)
			{
				return StatusPanel.Fill_AMOUNT_BEFORE_DOWN;
			}

			return expMeter;
		}

		/// <summary>階級挑戦問題が解放されているかどうかのチェック
		/// <param name="rank">お城好きレベル</param>
        /// <returns>true:解放されている、false:解放されていない</returns>
        /// </summary>
		public static bool IsCareerQuestionRelease(int rank)
		{
			// 身分上限のメーターチェックはメーター止まった状態の最大値と比較する
			if (rank >= 5)
			{
				return true;
			}
			else 
			{
				return false;
			}
		}

		/// <summary>強制アップデートの対象かどうか
        /// <returns>true:アップデート対象、false:対象ではない</returns>
        /// </summary>
		public static bool IsForceUpdate()
		{
			string currentVersion = Application.version;
			Debug.Log("!!!currentVersion:"+currentVersion);
			string forceUpdateVersion = OshiroRemoteConfig.Instance().ForceUpdateVersion;
			Debug.Log("!!!forceUpdateVersion:"+forceUpdateVersion);

			int[] currentVersions = Array.ConvertAll(currentVersion.Split('.'), int.Parse);
			int[] forceUpdateVersions = Array.ConvertAll(forceUpdateVersion.Split('.'), int.Parse);

			for (var i = 0; i < forceUpdateVersions.Length; i++)
			{
				if (currentVersions[i] < forceUpdateVersions[i])
				{
					return true;
				}
				if (currentVersions[i] > forceUpdateVersions[i])
				{
					return false;
				}
			}
			return false;
		}
    }
}