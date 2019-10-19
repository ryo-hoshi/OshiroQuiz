using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
	public class SoundController : MonoBehaviour
	{
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
			
		}

		public void RankUp() {
			audioSource.PlayOneShot(rankUp);
		}

		public void RankDown() {
			audioSource.PlayOneShot(rankDown);
		}

		public void MeterUp() {
			audioSource.PlayOneShot(meterUp);
		}

		public void MeterDown() {
			audioSource.PlayOneShot(meterDown);
		}

		public void MeterCrack() {
			audioSource.PlayOneShot(meterCrack);
		}

		public void Tap1() {
			audioSource.PlayOneShot(tap1);
		}

		public void Tap2() {
			audioSource.PlayOneShot(tap2);
		}
		
		public void TapStart() {
			audioSource.PlayOneShot(tapStart);
		}
	}
}
