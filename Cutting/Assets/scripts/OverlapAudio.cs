﻿using UnityEngine;
using Vuforia;

public class OverlapAudio : DefaultTrackableEventHandler
{
    public GameObject objA;
    public GameObject objB;
    public AudioSource audioSourceA;
    public AudioClip clipA;
    public AudioSource audioSourceB;
    public AudioClip clipB;

    public float MinDistance = 0.2f;
    public float MaxDistance = 1f;
    public bool CloserIsA = true;
    public bool turnOfAudioIfNotTracked = true;

    private bool tracked = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        audioSourceA.clip = clipA;
        audioSourceB.clip = clipB;

        audioSourceA.loop = true;
        audioSourceB.loop = true;

        audioSourceA.Play();
        audioSourceB.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (turnOfAudioIfNotTracked && tracked == false)
        {
            audioSourceA.volume = 0;
            audioSourceB.volume = 0;
        }
        else
        {
            float distance = Vector3.Distance(objA.transform.position, objB.transform.position);
            distance = Mathf.Clamp(distance, MinDistance, MaxDistance);
            float normDistance = (distance - MinDistance) / (MaxDistance - MinDistance);

            if (CloserIsA)
            {
                audioSourceA.volume = 1 - normDistance;
                audioSourceB.volume = normDistance;

                MeshRenderer foo = null;

                foo.material.color = new Color();
            }
            else
            {
                audioSourceA.volume = normDistance;
                audioSourceB.volume = 1 - normDistance;
            }
        }
    }

    public void ToggleCloserIsFaster()
    {
        CloserIsA = !CloserIsA;
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
