using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject fruit;
	public float lifespan = 7f;

	// Use this for initialization
	void Start () {
		StartCoroutine (SpawnFruit ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator SpawnFruit () {
		while (true) {
			yield return new WaitForSeconds(5f);
			Vector3 startPosition = new Vector3 (2.4f, 10f, 3f); 
			GameObject newFruit = Instantiate (fruit, startPosition, Random.rotation) as GameObject;
			Destroy (newFruit, lifespan);
		}
	}
}
