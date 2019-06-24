using System.Collections.Generic;

namespace QuizManagement
{
	public class GamePlayInfo
    {
		public static Result QuizResult = Result.STAY;

		public static QuizType PlayQuizType = QuizType.RegularQuiz;


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