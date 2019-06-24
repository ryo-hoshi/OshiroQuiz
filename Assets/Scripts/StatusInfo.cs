using System.Collections.Generic;

namespace QuizManagement
{
	public class StatusInfo
    {
		public StatusInfo(int rankStar, int rank, int rankExp, string career, int careerExp) {
			RankStar = rankStar;
			Rank = rank;
			RankExp = rankExp;
			Career = career;
			CareerExp = careerExp;
		}

		public int RankStar {get; private set;}

		public int Rank {get; private set;}

		public int RankExp {get; private set;}

		public string Career {get; private set;}

		public int CareerExp {get; private set;}
		/*
		public static GamePlayInfo.Result updateResult = StatusController.Result.STAY;

		public static GamePlayInfo.Result getResult() {
			return updateResult;
		}

		public static void setResult(StatusController.Result result) {
			updateResult = result;
		}
		*/
    }
}