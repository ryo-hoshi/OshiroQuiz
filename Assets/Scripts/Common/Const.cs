namespace Common
{
	public class Const
    {

		public const string SePath = "Audios/SE/";

		public const string EXTENSION_WAV = "wav";

#if RELEASE
		public const string Environment = "本番環境";
		public const string QUIZ_LOAD_URL = "https://asia-northeast1-rmuapp-release.cloudfunctions.net/careerQuizLoad";
		public const bool IsDevelop = false;
#elif DEVELOP
		public const string Environment = "開発環境";
		public const string QUIZ_LOAD_URL = "https://asia-northeast1-rmuapp-develop.cloudfunctions.net/careerQuizLoad";
		public const bool IsDevelop = true;
#endif
    }

	public class StatusPanel
	{
		// ちょっとずつメーターを更新するための、一度に更新するメータの割合
		public const float Fill_AMOUNT_UPDATE_STEP = 0.03f;
		// メータ最大値
		public const float Fill_AMOUNT_MAX = 1.0f;

		// メーター更新完了状態のMAX表示（メーターが埋まっているのにレベルアップしないように見えてしまうことの考慮）
		public const float Fill_AMOUNT_BEFORE_UP = 0.99f;

		// メータ最小値
		public const float Fill_AMOUNT_MIN = 0.0f;

		// メーター更新完了状態のMIN表示
		public const float Fill_AMOUNT_BEFORE_DOWN = 0.01f;
	}

	public enum SeAudioType
	{
		CareerCrack,
		MeterDown,
		MeterUp,
		Tap1,
		Tap2,
	}
}