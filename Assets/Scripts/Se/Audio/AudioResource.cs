using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName="AudioEvent")] // we can instantiate this asset in the inspector
public class AudioResource : ScriptableObject {
	public AudioClip sample;
	public float volume;
	public void Play(AudioSource source)
	{
		source.clip = sample;
		source.volume = volume;
		source.Play();
	}
}