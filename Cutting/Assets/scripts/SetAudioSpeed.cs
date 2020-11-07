using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetAudioSpeed : MonoBehaviour
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

    AudioMixerGroup pitchShifter = null;

    

    // Start is called before the first frame update
    void Start()
    {
        pitchShifter = audioSource.outputAudioMixerGroup;
    }

    // Update is called once per frame
    void Update()
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

        

        pitchShifter.audioMixer.SetFloat ("Volume", volumeFactor * VolumeMultiplyer);


        Debug.Log("Distance " + distance);
    }

    public void ToggleCloserIsFaster()
    {
        CloserIsFaster = !CloserIsFaster;
    }
}
