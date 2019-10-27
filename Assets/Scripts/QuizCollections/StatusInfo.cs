namespace QuizCollections
{
	public class StatusInfo
    {
        public StatusInfo(int rank, int rankExp, float rankMeter, 
            int career, int careerExp, float careerMeter, int castleDominance, int daimyouClass)
        {
			Rank = rank;
			RankExp = rankExp;
			RankMeter = rankMeter;
			Career = career;
			CareerExp = careerExp;
			CareerMeter = careerMeter;
			CastleDominance = castleDominance;
            DaimyouClass = daimyouClass;
        }

		public int Rank {get; private set;}

		public int RankExp {get; private set;}

		public float RankMeter {get; private set;}

		public int Career {get; private set;}

		public int CareerExp {get; private set;}

		public float CareerMeter {get; private set;}

		public int CastleDominance {get; private set;}

        public int DaimyouClass {get; private set; }
    }
}