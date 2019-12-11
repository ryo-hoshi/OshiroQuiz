using UnityEngine;
using System;
using System.Collections.Generic;

namespace Common
{
	public class AppInfos : MonoBehaviour
    {
		public static AppInfos instance;

		public Dictionary<SeAudioType, AudioClip> seAudioClips;

		void Awake()
		{
			if (instance == null)
			{

				instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		void Start()
		{
			if (seAudioClips == null) {
				seAudioClips = new Dictionary<SeAudioType, AudioClip>();

				foreach (SeAudioType seAudio in Enum.GetValues(typeof(SeAudioType)))
				{
					string seAudioName = Enum.GetName(typeof(SeAudioType), seAudio);

					AudioClip audioClip = Resources.Load<AudioClip>(Constants.SePath + seAudioName + Constants.EXTENSION_WAV);

					seAudioClips.Add(seAudio, audioClip);
				}
			}
		}
    }
}