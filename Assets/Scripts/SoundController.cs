using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
	[SerializeField]
	private AudioClip rankUpSound;
	[SerializeField]
	private AudioClip rankDownSound;

	private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
		audioSource = GetComponent<AudioSource>();
    }

	public void RankUp() {
		audioSource.PlayOneShot(rankUpSound);
	}

	public void RankDown() {
		audioSource.PlayOneShot(rankDownSound);
	}
}
