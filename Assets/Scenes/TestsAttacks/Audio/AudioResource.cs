using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName="AudioEvent")] // we can instantiate this asset in the inspector
public class AudioResource : ScriptableObject {
	public AudioClip sample;
	public float volume;
	public AudioSource source;
	public bool paused;
	public void Play(AudioSource newSource) {
		source = newSource;
		source.clip = sample;
		source.volume = volume;
		source.Play();
		paused = false;
	}

	public void Play() {
		if (source) {
			if (paused)
				source.UnPause ();
			else
				source.Play ();
		}
		paused = false;
	}

	public void Pause() {
		if(source) source.Pause ();
		paused = true;
	}

	public void SetVolume(float newVolume) {
		if(source) source.volume = newVolume;
	}

	public void SetSource(AudioSource newSource) {
		source = newSource;
		source.clip = sample;
		source.volume = volume;
	}

	public void SetSample(AudioClip clip) {
		sample = clip;
		if(source) source.clip = sample;
	}

	public void Stop() {
		if(source) source.Stop ();
		paused = false;
	}

	public bool IsPaused() {
		return paused;
	}
}