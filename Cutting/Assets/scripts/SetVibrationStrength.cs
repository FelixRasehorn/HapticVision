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
    public bool turnOfVibrationIfNotTracked = true;

    private bool tracked = false;

    byte lastSendStrength = 0;

    protected override void Start()
    {
        base.Start();

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
