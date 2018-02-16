using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName="AudioEvent")] // we can instantiate this asset in the inspector
public class AudioResource : ScriptableObject {
	public AudioClip sample;
	public float volume;
	public AudioSource source;
	public void Play(AudioSource newSource)
	{
		source = newSource;
		source.clip = sample;
		source.volume = volume;
		source.Play();
	}

	public void SetVolume(float newVolume) {
		source.volume = newVolume;
	}
}