using UnityEngine;
using System.Collections;

public class EnemyCtrl : MonoBehaviour {

	NavMeshAgent nav;
	GameObject player;
	Animator anim;
	public int blood;
	float timer;
	float deathTimer = 0;
	bool isDeath;
	public int state;
	[SerializeField]ParticleSystem hitParticle;
	[SerializeField]ParticleSystem deathParticle;

	// Use this for initialization
	void Start () {
		nav = GetComponent<NavMeshAgent> ();
		player = GameObject.Find ("Player");
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isDeath) {
			nav.SetDestination (player.transform.position);
			if (nav.remainingDistance > 0.5f) {
				anim.SetBool ("isMoving", true);
			} else {
				anim.SetBool ("isMoving", false);
			}
		}
		if (isDeath) {	
			deathTimer += Time.deltaTime;
			if (deathTimer >= 1.2f) {
				deathTimer = 0;
				switch (state) {
				case 1:
					transform.SetParent (GameObject.Find ("Chi1").transform);
					break;
				case 2:
					transform.SetParent (GameObject.Find ("Chi2").transform);
					break;
				case 3:
					transform.SetParent (GameObject.Find ("Chi3").transform);
					break;
				}
				isDeath = false;
				gameObject.SetActive (false);
			}
		}
	}

	public void EnemyHit(){
		timer += Time.deltaTime;
		if (timer >= 0.07f && !isDeath) {
			blood -= 8;
			if (!hitParticle.isPlaying) {
				hitParticle.Play ();
			}
			timer = 0;
		}
		if (blood<=0 && !isDeath) {
			isDeath = true;
			anim.SetTrigger ("isDeath");
			deathParticle.Play ();
			switch (state) {
			case 1:
			case 2:
				PlayerCtrl.score += 100;
				break;
			case 3:
				PlayerCtrl.score += 300;
				break;
			}
		}
	}
}
