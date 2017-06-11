using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

	Transform turretTransform;

	float range = 500f;
	public GameObject bulletPrefab;

	public int cost = 5;

	// Health
	public GameObject healthRanger;
	public float healthMax = 10.0f;
	public float health = 10.0f;
	public float healthRecovery = 0.01f; // Recover slowly when not shot
	public bool dead = false;
	float healthAppearanceThreashHold = 1.0f; // Healthranger appears above this threashold.

	// Fire
	float fireCooldown = 5.0f;
	float fireCooldownLeft = 0;
	float fireThreashHold = 6.9f; // Shoot above this threashold.

	public float damage = 1f;
	public float radius = 0f;

	// Tower Position
	Vector3 currentPosition;
	Vector3 targetPosition;
	float t = 0;
	float towerTopPosition = -22.0f;

	// Use this for initialization
	void Start () {
		targetPosition = transform.localPosition;
		turretTransform = transform.Find ("turret");
	}
	
	// Update is called once per frame
	void Update () {

		// TODO: Optimize
		Enemy[] enemies = GameObject.FindObjectsOfType<Enemy> ();

		Enemy nearestEnemy = null;
		float dist = Mathf.Infinity;

		foreach (Enemy e in enemies) {
			float d = Vector3.Distance (this.transform.position, e.transform.position);
			if (nearestEnemy == null || d < dist) {
				nearestEnemy = e;
				dist = d;
			}
		}


		// Tower Positioning
		t += Time.deltaTime/30.0f;
		transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, t);

		// ---------- TOWER DAMAGE MANAGER
		if (health <= 0)
			Destroy ();

		// Health recovers slowly
		if (health < healthMax)
			health += healthRecovery;
		else
			health = healthMax;

		// Rotate to camera
		healthRanger.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
		// Scale based on health value
		healthRanger.transform.localScale = new Vector3(health, 1, 1);
		healthRanger.transform.localPosition = new Vector3(health-10.0f, 20, 0);
		// Appear when tower is certain height, show the health range
		if(transform.localPosition.y > healthAppearanceThreashHold)
			healthRanger.SetActive(true);
		else
			healthRanger.SetActive(false);
		
		if (nearestEnemy == null) {
			return;
		}
		// ---------- TOWER DAMAGE MANAGER

		Vector3 dir = nearestEnemy.transform.position - this.transform.position;
		Quaternion lookRot = Quaternion.LookRotation (dir);
		// Tip: Make sure to use Euler so you can assign only to y
		turretTransform.rotation = Quaternion.Euler (0, lookRot.eulerAngles.y, 0);


		// Tower Fire
		if(transform.localPosition.y > fireThreashHold){
			fireCooldownLeft -= Time.deltaTime;
			if (fireCooldownLeft <= 0 && dir.magnitude <= range) {
				fireCooldownLeft = fireCooldown;
				ShootAt (nearestEnemy);
			}
		}

	}

	public void GetReadyToShoot(){
		t = 0;
		targetPosition = transform.localPosition + new Vector3(0f, towerTopPosition, 0f);
	}

	public void BackToHouse(){
		t = 0;
		targetPosition = transform.localPosition - new Vector3(0f, towerTopPosition, 0f);
	}

	public void GetDamage(){
		health -= 0.4f;
	}
	void Destroy(){
		dead = true;
		transform.gameObject.SetActive (false);
	}

	void ShootAt(Enemy e){
		GameObject bulletGO = (GameObject)Instantiate (bulletPrefab, this.transform.position, this.transform.rotation);

		Bullet b = bulletGO.GetComponent<Bullet> ();
		b.target = e.transform;

		b.damage = damage;
		b.radius = radius;
	}
}
