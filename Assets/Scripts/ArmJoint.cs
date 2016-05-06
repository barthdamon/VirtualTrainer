using UnityEngine;
using System.Collections;

[System.Serializable]
public class JointUUIDSet: System.Object {
	public string x;
	public string y;
	public string z;
	public string upperShock;
	public string lowerShock;
}

public class ArmJoint : MonoBehaviour {
	
	public BluetoothManager bluetoothManager;
	public ArmPositionManager positionManager;
	public JointUUIDSet jointIds;

	Quaternion currentRotation;

	// Use this for initialization
	void Start () {
		currentRotation = Quaternion.identity;
	}
	
	// Update is called once per frame
	void Update () {
		getCurrentPosition();
	}

	void FixedUpdate () {
		transform.rotation = Quaternion.Lerp(transform.rotation, currentRotation, Time.deltaTime);
	}


	void getCurrentPosition() {
		float x = positionManager.ValueForID (jointIds.x);
		float y = positionManager.ValueForID (jointIds.y);
		float z = positionManager.ValueForID (jointIds.z);
//		Debug.Log ("ARM JOINT POSITION: " + "x: " + x + "y: " + y + "z: " + z);
		currentRotation = Quaternion.Euler (currentRotation.x + x, currentRotation.y + y, currentRotation.z + z);
	}

	public void SendShock(bool forUpper, bool shock) {
		string shocked = forUpper ? jointIds.upperShock : jointIds.lowerShock;
		positionManager.bluetoothManager.ToggleShock (shocked, shock);
	}
}
