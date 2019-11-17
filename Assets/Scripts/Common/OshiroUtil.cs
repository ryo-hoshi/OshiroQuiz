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
			if (careerNum < (int)StatusCalcBasis.Career.大名
				&& careerNum >= GamePlayInfo.CareerLimitNum
				&& careerExpMeter >= StatusPanel.Fill_AMOUNT_MAX)
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