using System.Collections.Generic;

namespace QuizManagement
{
	// 出題するクイズ情報の引き渡し用クラス
    public class Quiz
    {
        public string Question {get; private set;}

        public Dictionary<int, string> Choices {get; private set;}

        public int Answer {get; private set;}

        private Quiz() 
        {
            // private
        }

        /**
         * 問題情報の生成
         */
		public Quiz(string question, string choice1, string choice2, string choice3, string answer) 
        {
            Question = question;

            Choices = new Dictionary<int, string>()
            {
                {1, choice1},
                {2, choice2},
                {3, choice3}
            };

			Answer = int.Parse(answer);
        }
    }
}