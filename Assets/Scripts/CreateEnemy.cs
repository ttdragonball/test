using UnityEngine;
using System.Collections;

public class CreateEnemy : MonoBehaviour {

	[SerializeField]GameObject enemy;
	[SerializeField]float createTime;
	[SerializeField]int state;
	float timer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= createTime) {
			if (transform.childCount > 0) {
				Transform tr = transform.GetChild (0);
				tr.position = transform.position;
				tr.gameObject.SetActive (true);
				tr.SetParent (null);
				EnemyCtrl en = tr.GetComponent<EnemyCtrl> ();
				switch (state) {
				case 1:
				case 2:en.blood=100;
					break;
				case 3:en.blood=300;
					break;
				}
				timer = 0;
			} else {
				GameObject go = Instantiate (enemy, transform.position, Quaternion.identity) as GameObject;
				timer = 0;
			}
		}
	}
}
