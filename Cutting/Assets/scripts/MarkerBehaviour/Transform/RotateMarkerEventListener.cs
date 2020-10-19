using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMarkerEventListener : MarkerEventListener
{

    public Vector3 RotationPerSecond;

    private bool rotate;

    protected override void Start() {

        base.Start();
    }

    public override void OnTrackingFound() {
        base.OnTrackingFound();
        rotate = true;
    }

    public override void OnTrackingLost() {
        base.OnTrackingLost();
        rotate = false;
    }

    private void Update() {
        if (rotate) {
            transform.Rotate(RotationPerSecond * Time.deltaTime, Space.Self);
        }
            
    }
}
