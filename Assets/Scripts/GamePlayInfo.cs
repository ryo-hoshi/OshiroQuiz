using System.Collections.Generic;

namespace QuizManagement
{
	public class GamePlayInfo
    {
		public static Result QuizResult = Result.STAY;

		public static QuizType PlayQuizType = QuizType.RegularQuiz;

		public static  int BeforeRank;
		public static  float BeforeRankExpMeter;
		public static  int BeforeCareer;
		public static  float BeforeCareerExpMeter;
        public static  int BeforeCastleDominance;
        public static int BeforeDaimyouClass;

        public static  int AfterRank;
		public static  float AfterRankExpMeter;
		public static  int AfterCareer;
		public static  float AfterCareerExpMeter;
        public static int AfterCastleDominance;
        public static int AfterDaimyouClass;

        public enum Result
		{
			RankUp,
			RankDown,
			STAY,
		}

		public enum QuizType
		{
			RegularQuiz = 1,
			CareerQuiz = 2
		}
    }
}