using UnityEngine;
using System.Collections;

public class Cameraflowel : MonoBehaviour {

	[SerializeField]GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (player.transform.position.x, player.transform.position.y + 8, player.transform.position.z - 5);
	}
}
