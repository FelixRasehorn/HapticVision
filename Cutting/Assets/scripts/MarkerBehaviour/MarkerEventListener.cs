using UnityEngine;
using Vuforia;

public class MarkerEventListener : MonoBehaviour {
	private ARTrackableEventDistributor eventDistributor;

	protected virtual void Start () {
		// Init as Lost
		OnTrackingLost();
	}

	public virtual void OnTrackingFound() {
		// Enable lights
		Light[] lights = GetComponentsInChildren<Light>();
		foreach (Light l in lights) {
			l.enabled = true;
		}
	}

	public virtual void OnTrackingLost() {
		// Disable lights
		Light[] lights = GetComponentsInChildren<Light>();
		foreach (Light l in lights) {
			l.enabled = false;
		}
	}

	protected virtual void OnApplicationPause(bool pause) {
		if (pause) {
			OnTrackingLost();
		}
	}

	public void RegisterEventDistributor(ARTrackableEventDistributor d) {
		eventDistributor = d;
	}

	// Unregister from Parent
	protected virtual void OnDestroy(){
		if (eventDistributor != null) {
			eventDistributor.OnTrackingFound.RemoveListener(OnTrackingFound);
			eventDistributor.OnTrackingLost.RemoveListener(OnTrackingLost);	
		}
	}
}
