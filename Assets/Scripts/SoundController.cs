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
		private AudioClip tap1;
		[SerializeField]
		private AudioClip tap2;
		[SerializeField]
		private AudioClip tapStart;

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

		public void Tap1() {
			audioSource.outputAudioMixerGroup = seGroup;
			audioSource.PlayOneShot(tap1);
		}

		public void Tap2() {
			audioSource.outputAudioMixerGroup = seGroup;
			audioSource.PlayOneShot(tap2);
		}
		
		public void TapStart() {
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

			audioSource.PlayOneShot(tapStart);
		}

		public void SetAudioMixerVolume(float masterVolume, float seVolume, float voiceVolume)
		{
			bool masterBool = audioMixer.SetFloat(ExposedParameters.MasterVolume.ToString(), masterVolume);
			Debug.Log("!!!!!!!!!!masterGroup.name[]:"+ExposedParameters.MasterVolume.ToString()+"["+masterVolume+"]["+masterBool+"]");
			bool seBool = audioMixer.SetFloat(ExposedParameters.SeVolume.ToString(), seVolume);
			Debug.Log("!!!!!!!!!!seGroup.name[]:"+ExposedParameters.SeVolume.ToString()+"["+seVolume+"]["+seBool+"]");
			bool voiceBool = audioMixer.SetFloat(ExposedParameters.VoiceVolume.ToString(), voiceVolume);
			Debug.Log("!!!!!!!!!!voiceGroup.name[]:"+ExposedParameters.VoiceVolume.ToString()+"["+voiceVolume+"]["+voiceBool+"]");
		}
	}
}
