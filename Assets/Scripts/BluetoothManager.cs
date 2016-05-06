using UnityEngine;
using System.Collections;

public class BluetoothManager : MonoBehaviour {

	public GameObject cubeIndicator;
	public Material connectedMaterial;
	public Material disconnectedMaterial;
	public ArmPositionManager armPositionManager;

	bool Connected = false;
	bool _connecting = false;
	bool _shocking = false;
	bool armFound = false;

	public float jointX;
	public float jointY;
	public float jointZ;

	private string _connectedID = null;
	private string _serviceUUID = "00110011-0000-0000-0000-000000000000";
	private string _positionCharacteristicUUID = "1001";
	private string _shockCharacteristicUUID = "1004";
	private string[] peripherals = {};



	public void InitializeBluetooth ()
	{
		BluetoothLEHardwareInterface.Initialize (true, false, () => {
			StartLoggingPeripherals ();
		}, (error) => {
			Debug.Log("Initialization Error: " + error);
		});
	}

	public bool isShocking() {
		return _shocking;
	}

	public void ToggleShock(string UUID, bool shock) {
		Debug.Log ("Toggling Shocking");
		byte[] data;
		data = shock ? new byte[] {0x01} : new byte[] {0x00};
//		armPositionManager.AddWriteValue (UUID, shock);
		BluetoothLEHardwareInterface.WriteCharacteristic(_connectedID, _serviceUUID, UUID, data, data.Length, true, message => {
			Debug.Log("Characteristic write success with message: " + message);
		});
	}
		

	public Quaternion GetRotationForJoint(int jointIndex) {
//		Debug.Log ("Returning joint rotation");
		Quaternion jointRotation = Quaternion.Euler (90, 30, 0);
		return jointRotation;
	}

	void ConnectionAchieved(string address) {
		Debug.Log("<<<<<  :) CONNECTION TO VR ARM ACHIEVED @ " + address + ">>>>>");
		cubeIndicator.GetComponent<Renderer>().material = connectedMaterial;
		Connected = true;
		_connecting = false;
		_connectedID = address;
//		MonitorArmCharacteristics ();
	}

	void ConnectToVRArm(string armAddress) {
			Debug.Log("Attempting to connect to VRArm.....");
			BluetoothLEHardwareInterface.ConnectToPeripheral (armAddress, (address) => {
				Debug.Log("<<<<<CONNECTED TO VR ARM>>>>");
				ConnectionAchieved(address);
			},
				(address, serviceUUID) => {
					if (serviceUUID == "00110011-0000-0000-0000-000000000000") {
						Debug.Log("<<<<<CONNECTED TO VR ARM with address and service uuid: " + serviceUUID + ">>>>");
						_serviceUUID = serviceUUID;
					}
				},
				(address, serviceUUID, characteristicUUID) => {
					if (serviceUUID == "00110011-0000-0000-0000-000000000000") {
						_positionCharacteristicUUID = characteristicUUID;
						Debug.Log("<<<<<ServiceUUID: " + serviceUUID + " CharacteristicUUID: " + characteristicUUID + ">>>>>");
						ReadValueAtId(characteristicUUID, serviceUUID);
//						ConnectionAchieved(address);
					}
				}, (address) => {
					Debug.Log("Device Disconnected");
					// this will get called when the device disconnects
					// be aware that this will also get called when the disconnect
					// is called above. both methods get call for the same action
					// this is for backwards compatibility
//					Debug.Log("<<<<<  :( VR ARM Disconnected>>>>>");
//					Connected = false;
//					cubeIndicator.GetComponent<Renderer>().material = disconnectedMaterial;
					//				ConnectToArduino();
				});

			_connecting = true;
	}

	void SubscribeForUpdates(string id, string serviceUUID) {
		BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress (_connectedID, serviceUUID, id, (notifyId, val) => {
		}, (address, charId, value) => {
			if (value != null) {
				string hexString = BytesToString(value);
				int hexInt = int.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
				Debug.Log("!!!!!!ID: " + id + " VALUE: " + BytesToString(value) + " actual value: " + hexInt + "!!!!!!");
				armPositionManager.AddReadValue(id, BytesToString(value));
//				if (hexInt > 45) {
////						_shocking = true;
//					Debug.Log("Triggering Shock on");
//					byte[] data = new byte[] {0x01};
//					BluetoothLEHardwareInterface.WriteCharacteristic(_connectedID, _serviceUUID, charId, data, data.Length, true, message => {
//						Debug.Log("Characteristic write success with message: " + message);
//					});
////						ToggleShock("1004", true);
//				} else {
//					byte[] data = new byte[] {0x00};
//					Debug.Log("Triggering Shock off");
//					BluetoothLEHardwareInterface.WriteCharacteristic(_connectedID, _serviceUUID, charId, data, data.Length, true, message => {
//						Debug.Log("Characteristic write success with message: " + message);
////							_shocking = false;
//					});
////						ToggleShock("1004", false);
//				}
			}
		});
	}

	void Update() {
		// UNCOMMENT TO READ INSTAD OF SUBSCRIBE
//		foreach (CharStore armPos in armPositionManager.readStore) {
//			if (armPos.UUID == "1001" || armPos.UUID == "1002" || armPos.UUID == "1003") {
//				ReadValueAtId (armPos.UUID, "00110011-0000-0000-0000-000000000000");
//			}
//		}
	}

	void ReadValueAtId(string id, string serviceUUID) {
//		Debug.Log ("Reading value at id: " + id);
		BluetoothLEHardwareInterface.ReadCharacteristic (_connectedID, serviceUUID, id, (charId, value) => {
			if (value != null) {
//				Debug.Log("!!!!!!ID: " + id + " VALUE: " + BytesToString(value) + " !!!!!!");
				armPositionManager.AddReadValue(id, BytesToString(value));
				// UNCOMMENT TO SUBSCRIBE INSTEAD OF READ
				SubscribeForUpdates(id, serviceUUID);
			}
		});
	}

	void StartLoggingPeripherals() {
		BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (null, (address, name) => {

			AddPeripheral (name, address);

		}, (address, name, rssi, advertisingInfo) => {

			if (advertisingInfo != null)
				BluetoothLEHardwareInterface.Log (string.Format ("Device: {0} RSSI: {1} Data Length: {2} Bytes: {3}", name, rssi, advertisingInfo.Length, BytesToString (advertisingInfo)));
		});
	}

	void AddPeripheral(string name, string address) {
		if (name == "Adafruit Bluefruit LE") {
			Debug.Log("<<<Atttempting to connect to Bluefruit>>>");
			ConnectToVRArm (address);
			BluetoothLEHardwareInterface.StopScan ();
		}
	}

	// Use this for initialization
	void Start () {
		Debug.Log("<<<<< STARTING BLUETOOTH MANAGER>>>>");
		cubeIndicator.GetComponent<Renderer>().material = disconnectedMaterial;
		InitializeBluetooth ();
	}

	void OnApplicationQuit() {
		BluetoothLEHardwareInterface.DisconnectPeripheral (_connectedID, null);
	}





	#region bluetooth helpers
	protected string BytesToString (byte[] bytes)
	{
		string result = "";

		foreach (var b in bytes)
			result += b.ToString ("X2");

		return result;
	}

	string FullUUID (string uuid)
	{
		return "0000" + uuid + "-0000-1000-8000-00805f9b34fb";
	}

	bool IsEqual(string uuid1, string uuid2)
	{
		if (uuid1.Length == 4)
			uuid1 = FullUUID (uuid1);
		if (uuid2.Length == 4)
			uuid2 = FullUUID (uuid2);

		return (uuid1.ToUpper().CompareTo(uuid2.ToUpper()) == 0);
	}
	#endregion
}
