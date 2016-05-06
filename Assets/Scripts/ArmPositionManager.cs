using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class CharStore {
	public int value;
	public string UUID;
}

public class ArmPositionManager: MonoBehaviour {
	public BluetoothManager bluetoothManager;
	public List<CharStore> writeStore = new List<CharStore>();
	public List<CharStore> readStore = new List<CharStore>();
	public string[] writeIds = {
		"1004",
		"1005",
		"2004",
		"2005",
		"3004",
		"3005"
	};

	public string[] readIds = {
		"1001",
		"1002",
		"1003",
		"2001",
		"2002",
		"2003",
		"3001",
		"3002",
		"3003"
	};


	void Start() {
		// create the keys for the characteristics
		Debug.Log("...Initializing Read and Write Characteristic Stores...");
		for (int i = 0; i < writeIds.Length; i++) {
			CharStore newStore = new CharStore ();
			newStore.UUID = writeIds[i];
			writeStore.Add(newStore);
		}
		for (int i = 0; i < readIds.Length; i++) {
			CharStore newStore = new CharStore ();
			newStore.UUID = readIds[i];
			readStore.Add(newStore);
		}
		Debug.Log ("Current UUIDs in Write Store:");
		foreach (CharStore armPos in writeStore)
			Debug.Log ("UUID: " + armPos.UUID);
		Debug.Log ("Current UUIDs in Read Store:");
		foreach (CharStore armPos in readStore)
			Debug.Log ("UUID: " + armPos.UUID);

		// UNCOMMENT FOR TESTING:
//		StartCoroutine (TestChangingValues ());
	}

	IEnumerator TestChangingValues() {
		while (true) {
			yield return new WaitForSeconds (3);
			for (int i = 0; i < readStore.Count; i++) {
				if (i == 3 || i == 4 || i == 5) {
					readStore [i].value -= 40;
				}
				readStore [i].value += 50;
			}
		}
	}

	public float ValueForID (string UUID) {
//		Debug.Log ("Fetching value for Id: " + UUID);
		foreach (CharStore armPos in readStore) {
			if (armPos.UUID == UUID) {
//				Debug.Log ("Found value for Id: " + UUID + "value: " + armPos.value);
				return armPos.value * 1.0f;
			}
		}
		foreach (CharStore armPos in writeStore) {
			if (armPos.UUID == UUID) {
				return armPos.value * 1.0f;
			}
		}
		
		return 0 * 1.0f;
	}

	public void AddReadValue(string UUID, string value) {
		foreach (CharStore armPos in readStore) {
			if (armPos.UUID == UUID) {
				int floatValue = int.Parse(value, System.Globalization.NumberStyles.HexNumber);
				armPos.value = floatValue;
				Debug.Log ("Stored Characteristic ID: " + armPos.UUID + "VALUE: " + armPos.value); 
			}
		}
	}

	public void AddWriteValue(string UUID, bool value) {
		foreach (CharStore armPos in writeStore) {
			if (armPos.UUID == UUID) {
				int floatValue = value ? 1 : 0;
				armPos.value = floatValue;
				Debug.Log ("Write Characteristic ID: " + armPos.UUID + "VALUE: " + armPos.value); 
			}
		}
	}
}