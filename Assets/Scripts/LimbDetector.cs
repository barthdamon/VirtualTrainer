using UnityEngine;
using System.Collections;

public class LimbDetector : MonoBehaviour {

	public ArmJoint joint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			Vector3 point = contact.point;
			if (point.z < 0) {
				// trigger back vibrator on
				Debug.Log("Back Vibrator Triggered On");
				joint.SendShock (forUpper: false, shock: true);
			} else {
				Debug.Log("Front Vibrator Triggered On");
				joint.SendShock (forUpper: true, shock: true);
				// trigger front vibrator on
			}
		}
	}

	void OnCollisionExit(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			Vector3 point = contact.point;
			if (point.z < 0) {
				// trigger back vibrator off
				Debug.Log("Back Vibrator Triggered Off");
				joint.SendShock (forUpper: false, shock: false);
			} else {
				Debug.Log("Front Vibrator Triggered Off");
				joint.SendShock (forUpper: true, shock: false);
				// trigger front vibrator off
			}
		}
	}
	
}
