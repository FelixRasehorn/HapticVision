using UnityEngine;
using Vuforia;

public class SetVibrationStrength : DefaultTrackableEventHandler
{
    public GameObject objA;
    public GameObject objB;
    public SerialControllerCustomDelimiter serialController;

    public byte MinStrength = 1;
    public byte MaxStrength = 255;
    public float Multiplier = 100.0f;
    public bool CloserIsFaster = true;
    public bool turnOffVibrationIfNotTracked = true;

    private bool tracked = false;

    byte lastSendStrength = 0;

    private void Awake()
    {
        if (turnOffVibrationIfNotTracked)
        {
            StatusFilter = TrackingStatusFilter.Tracked;
        }
    }

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

        SetMinValue();
    }

    void Update()
    {
        if (turnOffVibrationIfNotTracked && tracked == false)
        {
            if (lastSendStrength != 0)
            {
                serialController.SendSerialMessage(new byte[] { 0 });
                lastSendStrength = 0;
            }
        }
        else
        {
            float distance = Vector3.Distance(objA.transform.position, objB.transform.position);

            float strength = Mathf.Clamp(distance * Multiplier, MinStrength, MaxStrength);
            byte sendStrength = (byte)(CloserIsFaster ? MaxStrength - strength : strength);
            sendStrength = (byte)Mathf.Clamp(sendStrength, MinStrength, MaxStrength);

            if (sendStrength != lastSendStrength)
            {
                serialController.SendSerialMessage(new byte[] { sendStrength });
                Debug.Log($"For distance: {distance}, Send strength " + sendStrength + " " + new byte[] { sendStrength });
                lastSendStrength = sendStrength;
            }
        }
    }

    public void ToggleCloserIsFaster()
    {
        CloserIsFaster = !CloserIsFaster;
    }

    private void SetMinValue()
    {
        serialController.SendSerialMessage(new byte[] { MinStrength });
    }

    private void OnApplicationQuit()
    {
        SetMinValue();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        SetMinValue();
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
