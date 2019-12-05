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
				&& careerNum >= GamePlayInfo.CareerLimitNum
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
    }
}