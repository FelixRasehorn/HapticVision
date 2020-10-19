using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMarkerEventListener : MarkerEventListener {
	public AudioClip SoundFile;
	public bool ResetAudioOnLost = true;
	public float Volume = 1.0f;
	public bool Loop = false;

	public bool FadeInAudio = false;
	public float FadeInDuration = 1.0f;

	private AudioSource audioPlayer;

	protected override void Start() {
		if (SoundFile == null) {
			Debug.LogError("No Soundfile set in editor.", gameObject);
			Destroy(gameObject);
		} else {
			InitAudioPlayer();
			base.Start();
		}
	}

	public override void OnTrackingFound() {
		base.OnTrackingFound();
		audioPlayer.Play();
		if (FadeInAudio) {
			StartCoroutine(FadeAudioIn());
		}
	}

	public override void OnTrackingLost() {
		base.OnTrackingLost();
		if (ResetAudioOnLost) {
			audioPlayer.Stop();
		} else {
			audioPlayer.Pause();
		}
		audioPlayer.volume = (FadeInAudio) ? 0 : Volume;
	}

	private void InitAudioPlayer() {
		audioPlayer = GetComponent<AudioSource>();
		if (audioPlayer == null) {
			audioPlayer = gameObject.AddComponent<AudioSource>();
			audioPlayer.playOnAwake = false;
			audioPlayer.clip = SoundFile;
			audioPlayer.loop = Loop;
			audioPlayer.volume = (FadeInAudio) ? 0 : Volume;
		}
	}

	private IEnumerator FadeAudioIn() {
		float startTime = Time.time;

		while (audioPlayer.volume < Volume) {
			audioPlayer.volume = Mathf.Lerp(0f, Volume, (Time.time - startTime) / FadeInDuration);
			yield return null;
		}
	}
}
