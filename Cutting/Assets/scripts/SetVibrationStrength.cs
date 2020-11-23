using UnityEngine;
using Vuforia;

public class SetVibrationStrength : MonoBehaviour, ITrackableEventHandler
{
    public GameObject objA;
    public GameObject objB;
    public SerialControllerCustomDelimiter serialController;

    public byte MinStrength = 1;
    public byte MaxStrength = 255;
    public float Multiplier = 100.0f;
    public bool CloserIsFaster = true;
    public bool turnOfVibrationIfNotTracked = true;

    private bool tracked = false;

    byte lastSendStrength = 0;

    private void Start()
    {
        SetMinValue();
    }

    void Update()
    {
        if (turnOfVibrationIfNotTracked && tracked == false)
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

    private void OnDestroy()
    {
        SetMinValue();
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (tracked == false && (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED))
        {
            tracked = true;
        }
        else if (tracked && newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            tracked = false;
        }
    }
}
