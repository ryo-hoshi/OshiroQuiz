using QuizCollections;

namespace QuizManagement
{
	public interface QuizMaker
    {
		/**
		 * まだ出題していないtyep値の問題をランダムに選択して問題を生成する
		 */
		Quiz CreateQuiz();

		/**
		 * クイズの問題ロード完了確認
		 */
		bool IsLoadComplete();
	}
}