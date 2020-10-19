using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CollisionEvent : UnityEvent<string> { }

public class UnityEventTrigger : MonoBehaviour
{
    public string ID;
    public CollisionEvent OnEnter;
    public CollisionEvent OnExit;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        OnEnter.Invoke(ID);
    }

    private void OnTriggerExit(Collider other)
    {
        OnExit.Invoke(ID);
    }
}
