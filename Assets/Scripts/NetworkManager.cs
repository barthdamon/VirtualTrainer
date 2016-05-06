using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Networking;
//using UnityEngine.Networking.NetworkSystem;

public class NetworkManager : MonoBehaviour {

	int reliableChannelId;
	int maxConnections = 6;
	int socketId;
	int socketPort = 8000;
	int connectionId;


	// Use this for initialization
	void Start () {
		NetworkTransport.Init ();
		ConnectionConfig defaultConfig = new ConnectionConfig ();
		reliableChannelId = defaultConfig.AddChannel (QosType.Reliable);
		HostTopology topology = new HostTopology (defaultConfig, maxConnections);
	
		socketId = NetworkTransport.AddHost (topology, socketPort);
	}
	
	// Update is called once per frame
	void Update () {

		ListenNetwork ();
	}

	void ListenNetwork () {
		int recieveHostId;
		int recieveConnectionId;
		int recieveChannelId;

		byte[] recieveBuffer = new byte[1024];
		int bufferSize = 1024;
		int dataSize;
		byte error;

		NetworkEventType recieveNetworkEvent = NetworkTransport.Receive (out recieveHostId, out recieveConnectionId,
			out recieveChannelId, recieveBuffer, bufferSize, out dataSize, out error);
		
		switch (recieveNetworkEvent) {
			case NetworkEventType.ConnectEvent:
				Debug.Log ("New Client Connected");
				break;
			case NetworkEventType.DataEvent:
				Stream stream = new MemoryStream (recieveBuffer);
				BinaryFormatter formatter = new BinaryFormatter ();
				string message = formatter.Deserialize (stream) as string;
				Debug.Log ("Message From Client Recieved: " + message);
				break;
		}
	}

	public void Connect() {
		byte error;
		connectionId = NetworkTransport.Connect (socketId, "24.105.162.150", socketPort, 0, out error);
		if (error == null) {
			Debug.Log ("Connected to server");
		} else {
			Debug.Log ("Error connecting to server");
		}
	}

//	public void SendConnectionMessage() {
//		byte error;
//		byte[] buffer = new byte[1024];
//		Stream stream = new MemoryStream (buffer);
//		BinaryFormatter
//	}
}
