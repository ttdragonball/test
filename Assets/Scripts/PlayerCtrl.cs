using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour {

	[SerializeField]ETCJoystick moveJoystick;
	[SerializeField]ETCJoystick shootJoystick;
	[SerializeField]GameObject lineRenderObj;
	[SerializeField]GameObject gun;
	[SerializeField]GameObject fireLight;
	[SerializeField]ParticleSystem firePointParticle;
	[SerializeField]Slider bloodSlider;
	[SerializeField]Text scoreText;
	[SerializeField]GameObject hitImage;
	[SerializeField]GameObject pannel;
	[SerializeField]Text endScore;
	[SerializeField]Text maxScore;
	bool isHit;
	public static int score;
	LineRenderer lineRender;
	AudioSource fireSound;
	float timer;
	float hitTimer;
	float hitStayTimer;
	Animator anim;
	int playerBlood;
	bool isOver;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		lineRender = lineRenderObj.GetComponent<LineRenderer> ();
		fireSound = gun.GetComponent<AudioSource> ();
		playerBlood = 100;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isOver) {
			float x = shootJoystick.axisX.axisValue;
			float z = shootJoystick.axisY.axisValue;
			RaycastHit rayHit;
			if (x != 0 || z != 0) {
				Vector3 direction = new Vector3 (x, 0, z);
				transform.forward = direction;
				Ray ray = new Ray (gun.transform.position, direction);
				Physics.Raycast (ray, out rayHit);
				ShowFire (direction.normalized, rayHit);
			} else {
				lineRenderObj.SetActive (false);
				fireLight.SetActive (false);
			}


			float moveX = moveJoystick.axisX.axisValue;
			float moveY = moveJoystick.axisY.axisValue;

			if (moveX != 0 || moveY != 0) {
				anim.SetBool ("isMoving", true);
			} else {
				anim.SetBool ("isMoving", false);
			}

			bloodSlider.value = playerBlood;
			scoreText.text = "score:" + score;

			if (isHit) {
				hitImage.SetActive (true);
				hitTimer += Time.deltaTime;
				if (hitTimer > 0.1f) {
					hitImage.SetActive (false);
					isHit = false;
					hitTimer = 0;
				}
			}

			if (playerBlood <= 0) {
				endScore.text = "YOU SCORE:" + score;
				if (PlayerPrefs.GetInt ("maxscore") != null) {
					if (score > PlayerPrefs.GetInt ("maxscore")) {
						PlayerPrefs.SetInt ("maxscore", score);
					}
					maxScore.text = "MAX SCORE:" + PlayerPrefs.GetInt ("maxscore");
				} else {
					PlayerPrefs.SetInt ("maxscore", score);
					maxScore.text = "MAX SCORE:" + PlayerPrefs.GetInt ("maxscore");
				}
				isOver = true;
				pannel.SetActive (true);
			}
		}
	}

	void ShowFire(Vector3 direction,RaycastHit rayHit){
		lineRender.SetPosition (0, gun.transform.position);
		if (rayHit.point != null) {
			lineRender.SetPosition (1, rayHit.point);
			if (rayHit.transform.CompareTag ("Enemy")) {
				EnemyCtrl en = rayHit.transform.gameObject.GetComponent<EnemyCtrl> ();
				en.EnemyHit ();
			}
		} else {
			lineRender.SetPosition (1, gun.transform.position + direction * 20);
		}
		timer += Time.deltaTime;
		if (timer<=0.07f) {
			if (fireLight.activeSelf != true) {
				lineRenderObj.SetActive (true);
				fireLight.SetActive (true);
				fireSound.Play ();
				firePointParticle.Play ();
			}
		} else {
			lineRenderObj.SetActive (false);
			fireLight.SetActive (false);
			if (timer >= 0.14f) {
				timer = 0;
			}
		}
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.CompareTag("Enemy")) {
			playerBlood -= 12;
			isHit = true;
			hitStayTimer = 0;
		}
	}

	void OnCollisionStay(Collision col){
		if (col.gameObject.CompareTag("Enemy")) {
			hitStayTimer += Time.deltaTime;
			if (hitStayTimer > 0.5f) {
				playerBlood -= 8;
				isHit = true;
				hitStayTimer = 0;
			}
		}
	}

	public void RestartGame(){
		score = 0;
		SceneManager.LoadScene (0);
	}
}
