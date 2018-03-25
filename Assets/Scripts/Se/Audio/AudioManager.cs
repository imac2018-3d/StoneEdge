using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using Utils;

namespace Se {

	public class AudioManager : MonoBehaviour {
		
		public AudioResource AmbientSound;
		public AudioResource MusicSound;
		public AudioResource ActionSound;

		public enum Action { Walk, Jump, BasicAttack, MagnetImpact, JumpQuake, Dodge }
		[Serializable]
		public struct ActionSample {
			public Action action;
			public AudioClip sample;
		}
		public List<ActionSample>  ActionSamples;

		public enum Music { Menu }
		[Serializable]
		public struct MusicSample {
			public Music music;
			public AudioClip sample;
		}
		public List<MusicSample>  MusicSamples;

		public enum Ambient { Forest, Canyon, Desert, Wind }
		[Serializable]
		public struct AmbientSample {
			public Ambient ambient;
			public AudioClip sample;
		}
		public List<AmbientSample>  AmbientSamples;

		private static GameObject instance;
		public static AudioManager GetInstance() {
			if (!instance)
				instance = GameObject.FindGameObjectWithTag ("AudioManager");
			if (instance)
				return instance.GetComponent<AudioManager>();
			else
				return null;
		}

		public void Start() {
			MusicSound.SetSource(GetComponents<AudioSource>()[0]);
			AmbientSound.SetSource(GetComponents<AudioSource> () [1]);
			ActionSound.SetSource (GetComponents<AudioSource> () [2]);
		}

		public void PlayMusic(Music sound) {
			AudioClip newSample = MusicSamples.Find (x => x.music.Equals (sound)).sample;
			if(!MusicSound.sample.Equals(newSample)) MusicSound.SetSample (newSample);
			MusicSound.Play ();
		}

		public void PlayMusic() {
			MusicSound.Play ();
		}

		public void PauseMusic() {
			MusicSound.Pause ();
		}

		public void PlayAction(Action sound) {
			ActionSound.SetSample (ActionSamples.Find(x => x.action.Equals(sound)).sample);
			ActionSound.Play ();
		}

		public void PauseAction() {
			ActionSound.Pause ();
		}

		public void PlayAmbient(Ambient sound) {
			AudioClip newSample = AmbientSamples.Find (x => x.ambient.Equals (sound)).sample;
			if(!AmbientSound.sample.Equals(newSample)) AmbientSound.SetSample (newSample);
			AmbientSound.Play ();
		}

		public void PlayAmbient() {
			AmbientSound.Play ();
		}

		public void PauseAmbient() {
			AmbientSound.Pause ();
		}
	}
}