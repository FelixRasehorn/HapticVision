using UnityEngine;

public class SetVibrationStrength : MonoBehaviour
{
    public GameObject objA;
    public GameObject objB;
    public SerialControllerCustomDelimiter serialController;

    //public float DefaultVibrationDistance = 1f;
    //public float DefaultVolumeDistance = 0.3f; //volume
    public byte MinStrength = 1;
    public byte MaxStrength = 255;
    public float Multiplier = 100.0f;
    public bool CloserIsFaster = true;
    // public byte HardCodedStrength = 100;

    byte lastSendStrength = 0;

    private void Start()
    {
        SetMinValue();
    }

    void Update()
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
}
