using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleMarkerEventListener : MarkerEventListener {
    public float TargetScaleFactor = 2.0f;
    public float ScaleTime = 1.0f;

    public bool LoopAnimation = false;

    private Vector3 initialScale;
    private float scaleFactor;

    protected override void Start() {
        initialScale = transform.localScale;
        base.Start();
    }

	public override void OnTrackingFound() {
		base.OnTrackingFound();
        if (!LoopAnimation) {
            StartCoroutine(ScaleAnimation(true));
        } else {
            StartCoroutine(LoopScaleAnimation());
        }
		
	}

	public override void OnTrackingLost() {
		base.OnTrackingLost();
		StopAllCoroutines();
        transform.localScale = initialScale;
        if (!LoopAnimation) {
            scaleFactor = 1;
        }
	}

    private IEnumerator ScaleAnimation(bool loopIn) {
        float startTime = Time.time;
        while (scaleFactor != (loopIn ? TargetScaleFactor : 1)) {
            if (loopIn) {
                scaleFactor = Mathf.Lerp(1, TargetScaleFactor, (Time.time - startTime) / ScaleTime);
            } else {
                scaleFactor = Mathf.Lerp(TargetScaleFactor, 1, (Time.time - startTime) / ScaleTime);
            }
            
            transform.localScale = initialScale * scaleFactor;

            yield return null;
        }
    }

    private IEnumerator LoopScaleAnimation() {
        while(true) { // Endlosschleife
            yield return ScaleAnimation(true);
            yield return ScaleAnimation(false);
        }
    }
}
