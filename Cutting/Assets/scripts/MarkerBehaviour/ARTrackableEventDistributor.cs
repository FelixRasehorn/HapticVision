using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;

public class ARTrackableEventDistributor : MonoBehaviour, ITrackableEventHandler {

	public bool ManuallyAssignEvents = false;

	[SerializeField]
	public UnityEvent OnTrackingFound;
	[SerializeField]
	public UnityEvent OnTrackingLost;
	
	protected TrackableBehaviour mTrackableBehaviour;

	protected void Start () {
		if (!ManuallyAssignEvents) {
			AutomaticallyAddTrackingEventListeners();
		}

		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour) {
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		}
	}

	private void AutomaticallyAddTrackingEventListeners() {
		MarkerEventListener[] markerEventHandlers = GetComponentsInChildren<MarkerEventListener>();
		foreach (MarkerEventListener handler in markerEventHandlers) {
			OnTrackingFound.AddListener(handler.OnTrackingFound);
			OnTrackingLost.AddListener(handler.OnTrackingLost);
			handler.RegisterEventDistributor(this);
		}
	}
	
	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus) {
		
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
			newStatus == TrackableBehaviour.Status.TRACKED ||
			newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
			
			OnTrackingFound.Invoke();
		} else {
			OnTrackingLost.Invoke();
		}
	}
}

