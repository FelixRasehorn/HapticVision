using UnityEngine;
using UnityEngine.Audio;
using Vuforia;

public class SetAudioSpeed : DefaultTrackableEventHandler
{
    public GameObject objA;
    public GameObject objB;
    public AudioSource audioSource;
    public AudioClip clip;
    public float DefaultPitchDistance = 0.2f;
    public float DefaultVolumeDistance = 0.3f; //volume
    public float MinPitch = 0.5f;
    public float MaxPitch = 1.5f;
    public float VolumeMultiplyer = 2.0f;
    public bool CloserIsFaster = true;
    public bool turnOffAudioIfNotTracked = true;

    AudioMixerGroup pitchShifter = null;

    private bool tracked = false;

    private void Awake()
    {
        if (turnOffAudioIfNotTracked)
        {
            StatusFilter = TrackingStatusFilter.Tracked;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if (mTrackableBehaviour == null)
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>() ?? GetComponentInParent<TrackableBehaviour>();

            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterOnTrackableStatusChanged((status) =>
                {
                    if (status.NewStatus == TrackableBehaviour.Status.DETECTED ||
                        status.NewStatus == TrackableBehaviour.Status.TRACKED ||
                        status.NewStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
                    {
                        OnTrackingFound();
                    }
                    else if (status.PreviousStatus == TrackableBehaviour.Status.TRACKED &&
                             status.NewStatus == TrackableBehaviour.Status.NO_POSE)
                    {
                        OnTrackingLost();
                    }
                    else
                    {
                        // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
                        // Vuforia is starting, but tracking has not been lost or found yet
                        // Call OnTrackingLost() to hide the augmentations
                        OnTrackingLost();
                    }
                });
            }
            else
            {
                Debug.LogError("The TrackableBehaviour component needs to be assigned to the object itself or parent");
            }
        }

        pitchShifter = audioSource.outputAudioMixerGroup;
    }

    // Update is called once per frame
    void Update()
    {
        if (turnOffAudioIfNotTracked && tracked == false)
        {
            pitchShifter.audioMixer.SetFloat("Volume", 0f);
        }
        else
        {
            float distance = Vector3.Distance(objA.transform.position, objB.transform.position);

            float normDistance = distance / DefaultPitchDistance;
            float pitchFactor = 1 + normDistance; // math, e.g. = 1 + normDistance * 0.5f;

            float volumeFactor = distance / DefaultVolumeDistance;

            pitchFactor = Mathf.Clamp(pitchFactor, MinPitch, MaxPitch);

            if (CloserIsFaster)
            {
                pitchShifter.audioMixer.SetFloat("MasterPitch", 1.0f / pitchFactor);
                pitchShifter.audioMixer.SetFloat("PitchShifterPitch", pitchFactor);
            }
            else
            {
                pitchShifter.audioMixer.SetFloat("MasterPitch", pitchFactor);
                pitchShifter.audioMixer.SetFloat("PitchShifterPitch", 1.0f / pitchFactor);
            }

            pitchShifter.audioMixer.SetFloat("Volume", volumeFactor * VolumeMultiplyer);

            Debug.Log("Distance " + distance);
        }
    }

    public void ToggleCloserIsFaster()
    {
        CloserIsFaster = !CloserIsFaster;
    }

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();

        tracked = true;
    }
    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();

        tracked = false;
    }
}
