using System.Collections.Generic;

namespace QuizManagement
{
	public class StatusInfo
    {
		public StatusInfo(int rankStar, int rank, int rankExp, float rankMeter, int career, int careerExp, float careerMeter) {
			RankStar = rankStar;
			Rank = rank;
			RankExp = rankExp;
			RankMeter = rankMeter;
			Career = career;
			CareerExp = careerExp;
			CareerMeter = careerMeter;
		}

		public int RankStar {get; private set;}

		public int Rank {get; private set;}

		public int RankExp {get; private set;}

		public float RankMeter {get; private set;}

		public int Career {get; private set;}

		public int CareerExp {get; private set;}

		public float CareerMeter {get; private set;}

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