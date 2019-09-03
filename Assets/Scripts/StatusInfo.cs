using System.Collections.Generic;

namespace QuizManagement
{
	public class StatusInfo
    {
        public StatusInfo(int rank, int rankExp, float rankMeter, int career, int careerExp, float careerMeter, int castleDominance)
        {
			Rank = rank;
			RankExp = rankExp;
			RankMeter = rankMeter;
			Career = career;
			CareerExp = careerExp;
			CareerMeter = careerMeter;
			CastleDominance = castleDominance;
		}

		public int Rank {get; private set;}

		public int RankExp {get; private set;}

		public float RankMeter {get; private set;}

		public int Career {get; private set;}

		public int CareerExp {get; private set;}

		public float CareerMeter {get; private set;}

		public int CastleDominance {get; private set;}
    }
}