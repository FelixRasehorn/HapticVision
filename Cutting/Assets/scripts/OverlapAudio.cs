using UnityEngine;

public class OverlapAudio : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        audioSourceA.clip = clipA;
        audioSourceB.clip = clipB;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(objA.transform.position, objB.transform.position);
        distance = Mathf.Clamp(distance, MinDistance, MaxDistance);
        float normDistance = (distance - MinDistance) / (MaxDistance - MinDistance);

        if (CloserIsA)
        {
            audioSourceA.volume = 1 - normDistance;
            audioSourceB.volume = normDistance;
        }
        else
        {
            audioSourceA.volume = normDistance;
            audioSourceB.volume = 1 - normDistance;
        }
    }

    public void ToggleCloserIsFaster()
    {
        CloserIsA = !CloserIsA;
    }
}
