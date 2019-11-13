using QuizCollections;
using UnityEngine;
using UnityEngine.Audio;

namespace Common
{
	public class SoundController : MonoBehaviour
	{
		private enum ExposedParameters
		{
			MasterVolume,
			SeVolume,
			VoiceVolume,
		}

		[SerializeField]
		private AudioClip rankUp;
		[SerializeField]
		private AudioClip rankDown;
		[SerializeField]
		private AudioClip meterUp;
		[SerializeField]
		private AudioClip meterDown;
		[SerializeField]
		private AudioClip meterCrack;
		[SerializeField]
		private AudioClip quizStart;
		[SerializeField]
		private AudioClip decision;
		[SerializeField]
		private AudioClip cancel;
		[SerializeField]
		private AudioClip option;
		[SerializeField]
		private AudioClip tapToStart;
		[SerializeField]
		private AudioClip correctAnswer;
		[SerializeField]
		private AudioClip inCorrectAnswer;
		[SerializeField]
		private AudioClip fireworks1;
		[SerializeField]
		private AudioClip fireworks2;

		[SerializeField]
		private AudioMixerGroup masterGroup;
		[SerializeField]
		private AudioMixerGroup seGroup;
		[SerializeField]
		private AudioMixerGroup voiceGroup;
		[SerializeField]
		private AudioMixer audioMixer;

		private AudioSource audioSource;

		public static SoundController instance;

		void Awake()
		{
			if (instance == null)
			{
				instance = this;
				audioSource = GetComponent<AudioSource>();

				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		// Start is called before the first frame update
		void Start()
		{
			init();
		}

		private void init()
		{
			SaveData saveData = new SaveData();
			ConfigInfo configInfo = saveData.GetConfigData();
			SetAudioMixerVolume(configInfo.MasterVolume, configInfo.SeVolume, configInfo.VoiceVolume);
		}

		public void RankUp() {
			audioSource.outputAudioMixerGroup = voiceGroup;
			audioSource.PlayOneShot(rankUp);
		}

		public void RankDown() {
			audioSource.outputAudioMixerGroup = voiceGroup;
			audioSource.PlayOneShot(rankDown);
		}

		public void MeterUp() {
			audioSource.outputAudioMixerGroup = seGroup;
			audioSource.PlayOneShot(meterUp);
		}

		public void MeterDown() {
			audioSource.outputAudioMixerGroup = seGroup;
			audioSource.PlayOneShot(meterDown);
		}

		public void MeterCrack() {
			audioSource.outputAudioMixerGroup = seGroup;
			audioSource.PlayOneShot(meterCrack);
		}

		public void QuizStart() {
			audioSource.outputAudioMixerGroup = seGroup;
			audioSource.PlayOneShot(quizStart);
		}

		public void Decision() {
			audioSource.outputAudioMixerGroup = seGroup;
			audioSource.PlayOneShot(decision);
		}
		
		public void Cancel() {
			audioSource.outputAudioMixerGroup = seGroup;
			audioSource.PlayOneShot(cancel);
		}
		
		public void Option() {
			audioSource.outputAudioMixerGroup = seGroup;
			audioSource.PlayOneShot(option);
		}
		
		public void TapToStart() {
			audioSource.outputAudioMixerGroup = seGroup;

			float masterVol;
			bool masterBool = audioMixer.GetFloat(ExposedParameters.MasterVolume.ToString(), out masterVol);
			Debug.Log("!!!!!!!!!!masterGroup.name[]:"+ExposedParameters.MasterVolume.ToString()+"["+masterVol+"]["+masterBool+"]");
			float seVol;
			bool seBool = audioMixer.GetFloat(ExposedParameters.SeVolume.ToString(), out seVol);
			Debug.Log("!!!!!!!!!!seGroup.name[]:"+ExposedParameters.SeVolume.ToString()+"["+seVol+"]["+seBool+"]");
			float voiceVol;
			bool voiceBool = audioMixer.GetFloat(ExposedParameters.VoiceVolume.ToString(), out voiceVol);
			Debug.Log("!!!!!!!!!!voiceGroup.name[]:"+ExposedParameters.VoiceVolume.ToString()+"["+voiceVol+"]["+voiceBool+"]");

			audioSource.PlayOneShot(tapToStart);
		}
		
		public void CorrectAnswer() {
			audioSource.outputAudioMixerGroup = seGroup;
			audioSource.PlayOneShot(correctAnswer);
		}
		
		public void InCorrectAnswer() {
			audioSource.outputAudioMixerGroup = seGroup;
			audioSource.PlayOneShot(inCorrectAnswer);
		}
		
		public void Fireworks1() {
			audioSource.outputAudioMixerGroup = seGroup;
			audioSource.PlayOneShot(fireworks1);
		}
		
		public void Fireworks2() {
			audioSource.outputAudioMixerGroup = seGroup;
			audioSource.PlayOneShot(fireworks2);
		}
		
		public void SetAudioMixerVolume(float masterVolume, float seVolume, float voiceVolume)
		{
			Debug.Log("!!!!!!!!!!パラメータ:["+masterVolume+"]["+seVolume+"]["+voiceVolume+"]");

			bool masterBool = audioMixer.SetFloat(ExposedParameters.MasterVolume.ToString(), convertAudioMixerValue(masterVolume));
			bool seBool = audioMixer.SetFloat(ExposedParameters.SeVolume.ToString(), convertAudioMixerValue(seVolume));
			bool voiceBool = audioMixer.SetFloat(ExposedParameters.VoiceVolume.ToString(), convertAudioMixerValue(voiceVolume));
		}

		private float convertAudioMixerValue(float value)
		{
			float thresholdVal = 20.0f;

			if (value > -thresholdVal)
			{
				return value;
			}
			else
			{
				float convertvalue = value + thresholdVal;
				convertvalue*= 6;
				convertvalue-= thresholdVal;

				return convertvalue;
			}

		}
	}
}
