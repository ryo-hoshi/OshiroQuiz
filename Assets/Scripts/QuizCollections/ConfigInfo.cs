namespace QuizCollections
{
	public class ConfigInfo
    {
        public ConfigInfo(float masterVolume, float seVolume, float voiceVolume)
        {
			MasterVolume = masterVolume;
			SeVolume = seVolume;
			VoiceVolume = voiceVolume;
        }

		public float MasterVolume {get; private set;}

		public float SeVolume {get; private set;}

		public float VoiceVolume {get; private set;}
    }
}