using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Transform))]
public class CubeResponder : MonoBehaviour, ICardboardGazeResponder {

	public BluetoothManager bluetoothManager;

	float speed = 1.0f;
	float amount = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (bluetoothManager.isShocking()) {
			transform.position = new Vector3 (Mathf.Sin(Time.time * speed), 0, transform.position.z);
		}
//		bluetoothManager
	}

	void OnTriggerEnter() {
//		bluetoothManager.ToggleShock ("2004", true);
	}

	void OnTriggerExit() {
//		bluetoothManager.ToggleShock ("2004", false);
	}




	#region ICardboardGazeResponder implementation

	/// Called when the user is looking on a GameObject with this script,
	/// as long as it is set to an appropriate layer (see CardboardGaze).
	public void OnGazeEnter() {
//		SetGazedAt(true);
		Debug.Log ("GAZE ENTER CALLED");
	}

	/// Called when the user stops looking on the GameObject, after OnGazeEnter
	/// was already called.
	public void OnGazeExit() {
		Debug.Log ("GAZE EXIT CALLED");
//		SetGazedAt(false);
	}

	// Called when the Cardboard trigger is used, between OnGazeEnter
	/// and OnGazeExit.
	public void OnGazeTrigger() {
		Debug.Log ("GAZE TRIGGER CALLED");
//		bluetoothManager.ToggleShock (1, 1);
	}

	#endregion
}
