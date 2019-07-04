using System.Collections.Generic;

namespace QuizManagement
{
	public class GamePlayInfo
    {
		public static Result QuizResult = Result.STAY;

		public static QuizType PlayQuizType = QuizType.RegularQuiz;

		public static  int BeforeRankStar;
		public static  int BeforeRank;
		public static  float BeforeRankExpMeter;
		public static  int BeforeCareer;
		public static  float BeforeCareerExpMeter;

		public static  int AfterRankStar;
		public static  int AfterRank;
		public static  float AfterRankExpMeter;
		public static  int AfterCareer;
		public static  float AfterCareerExpMeter;

		public enum Result
		{
			RankUp,
			RankDown,
			STAY,
		}

		public enum QuizType
		{
			RegularQuiz,
			CareerQuiz
		}
    }
}