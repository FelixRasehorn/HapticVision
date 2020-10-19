using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoMarkerEventListener : MarkerEventListener {
	
	private VideoPlayer videoPlayer;

	protected override void Start() {
        videoPlayer = GetComponent<VideoPlayer>();
		base.Start();
	}

	public override void OnTrackingFound() {
		base.OnTrackingFound();
		videoPlayer.Play();
	}

	public override void OnTrackingLost() {
		base.OnTrackingLost();
		if (videoPlayer != null) {
			videoPlayer.Stop();
		}
	}
}
