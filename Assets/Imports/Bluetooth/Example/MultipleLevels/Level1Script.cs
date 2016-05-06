using UnityEngine;
using System.Collections.Generic;

public class Level1Script : MonoBehaviour
{
	public void OnScanClick ()
	{
		BluetoothLEHardwareInterface.Initialize (true, false, () => {

			FoundDeviceListScript.DeviceAddressList = new List<DeviceObject> ();

			BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (null, (address, name) => {

				FoundDeviceListScript.DeviceAddressList.Add (new DeviceObject (address, name));

			}, null);

		}, (error) => {

			BluetoothLEHardwareInterface.Log ("BLE Error: " + error);

		});
	}

	public void OnStartLevel2 ()
	{
		Application.LoadLevel ("Level2");
	}
}
