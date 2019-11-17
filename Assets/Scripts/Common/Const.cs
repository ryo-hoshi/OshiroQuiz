namespace Common
{
	public class Const
    {

		public const string SePath = "Audios/SE/";

		public const string EXTENSION_WAV = "wav";


    }

	public class StatusPanel
	{
		// ちょっとずつメーターを更新するための、一度に更新するメータの割合
		public const float Fill_AMOUNT_UPDATE_STEP = 0.02f;
		// メータ最大値
		public const float Fill_AMOUNT_MAX = 1.0f;
		// メータ最小値
		public const float Fill_AMOUNT_MIN = 0.0f;
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