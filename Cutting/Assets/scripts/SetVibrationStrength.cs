using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVibrationStrength : MonoBehaviour
{
    public GameObject objA;
    public GameObject objB;
    public SerialControllerCustomDelimiter serialController;

    public float DefaultVibrationDistance = 1f;
    public float DefaultVolumeDistance = 0.3f; //volume
    public byte MinStrength = 0;
    public byte MaxStrength = 255;
    public float VolumeMultiplyer = 1.0f;
    public bool CloserIsFaster = true;
    // public byte HardCodedStrength = 100;

 



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        float distance = Vector3.Distance(objA.transform.position, objB.transform.position);

        float normDistance = distance / DefaultVibrationDistance;
        float pitchFactor = 1 + normDistance * 0.5f; // math, e.g. = 1 + normDistance * 0.5f;

        pitchFactor = Mathf.Clamp(pitchFactor, 0, 1);
        byte Strength = (byte) (pitchFactor * MaxStrength);
        byte SentStrength = CloserIsFaster ? (byte)(MaxStrength - Strength) : Strength;
        SentStrength = Strength;
        serialController.SendSerialMessage(new byte[] { SentStrength });

        Debug.Log("SentStrength " + SentStrength + " " + new byte[] {SentStrength});
        Debug.Log("Distance " + distance);
    }

    public void ToggleCloserIsFaster()
    {
        CloserIsFaster = !CloserIsFaster;
    }
}
