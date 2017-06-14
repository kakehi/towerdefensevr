using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	// If VREnemy is on do not kill since the camera is part of the gameobject
	GameManager gm;
	bool VREnemy = false;

	GameObject pathGO;

	Transform targetPathNode;
	int pathNodeIndex = 0;

	float speed = 10.0f;

	public float health = 10f;


	public int moneyValue = 1;


	// -- REMOTE
	public GameObject playerArmObject;
	GvrController GvrController;

	// Laser
	public LineRenderer laser;
	float laserCurrentReach = 0.0f; //
	float laserIncreaseReach = 0.1f; // As user touching laser extends by this rate
	float laserEndingReach = 0.25f; // As user releases laser shrinks by this rate

	public GameObject LaserHit_Container;
	int laserHitCounter = 0;
	int laserHitCoolDownCounter = 0;
	int laserHitCoolDown = 5;

	Quaternion targetRotation;

	void Start() {

		gm = GameObject.FindObjectOfType<GameManager> ();
		if (gm.isVR)
			VREnemy = true;

		pathGO = GameObject.Find ("PathString");

		// Controller

		// First, make sure not to run controller code if it's not a VR enemy.
		if (!VREnemy)
			return;

		laser = playerArmObject.GetComponent<LineRenderer> ();

	}

	void GetNextPathNode(){

		targetPathNode = pathGO.transform.GetChild (pathNodeIndex);

		pathNodeIndex++;
	}
		
	void Update () {
		
		// Finding the next node but if it doesn't exist just call function reached goal
		if (targetPathNode == null) {
			GetNextPathNode ();
			if (targetPathNode == null) {
				// We've run out of path!
				ReachedGoal();
			}

		}

		// Finding the direction to the next node
		Vector3 dir = targetPathNode.position - this.transform.localPosition;

		float distThisFrame = speed * Time.deltaTime;

		if (dir.magnitude <= distThisFrame) {
			// We reached the target node
			targetPathNode = null;
		} else {
			// Move towards the target node
			transform.Translate(dir.normalized * distThisFrame, Space.World);

			// Look towards the target node
			// Instantaneours Change
			Quaternion.LookRotation (dir);
			// slow change
			targetRotation = Quaternion.LookRotation (dir);
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, targetRotation, Time.deltaTime / 2.0f );

		}

		// CONTROLLER

		// First, make sure not to run controller code if it's not a VR enemy.
		if (!VREnemy)
			return;

		// Example: get controller's current orientation:
		Quaternion ori = GvrController.Orientation;
		Vector3 v = ori * transform.forward;

		// ...or you can just change the rotation of some entity on your scene
		// (e.g. the player's arm) to match the controller's orientation
		playerArmObject.transform.localRotation = ori;

		// Example: check if touchpad was just touched
		if (GvrController.TouchDown) {
			// Do something.
			// TouchDown is true for 1 frame after touchpad is touched.
		}

		// Example: check if app button was just released.
		if (GvrController.AppButtonUp) {
			// Do something.
			// AppButtonUp is true for 1 frame after touchpad is touched.
		}

		// Controller is being touched
		if (GvrController.IsTouching) {
			Vector2 touchPos = GvrController.TouchPos;

			// increase laser reach by the rate as user keeps finger on the touchpad
			if (laserCurrentReach < 1.0f)
				laserCurrentReach += laserIncreaseReach;
			else
				laserCurrentReach = 1.0f;

		}else{

			// decrease laser reach by the rate as user keeps finger on the touchpad
			if (laserCurrentReach > 0.0f)
				laserCurrentReach -= laserEndingReach;
			else
				laserCurrentReach = 0;
			
		}

		// The direction of the laser is the forward direction of the player arm object
		ShootLaserFromTargetPosition(playerArmObject.transform.forward, 200f);
	}

	void ReachedGoal(){
		// TODO: optimize this
		GameObject.FindObjectOfType<ScoreManager> ().LoseLife ();
		Destroy (gameObject);
	}
		

	void ShootLaserFromTargetPosition(Vector3 laserDirection, float length){

		Vector3 targetPosition = playerArmObject.transform.position;

		// Shoot only if the raser current reach is at certain limit
		if (laserCurrentReach > 0.5f) {
			Ray ray = new Ray (targetPosition, laserDirection);
			RaycastHit raycastHit;

			// Apply Damage While RayCast Hit
			if (Physics.Raycast (ray, out raycastHit, length)) {

				GameObject hitTowerObject = raycastHit.transform.gameObject;

				if (hitTowerObject.tag == "Tower") {

					// Hit
					GenerateLaserHit (raycastHit.point);

					// Give damage
					hitTowerObject.GetComponent<Tower> ().GetDamage ();
				}
			}
		}

		Vector3 endPosition = targetPosition + ( length * laserDirection * laserCurrentReach );
		laser.SetPosition( 0, targetPosition );
		laser.SetPosition( 1, endPosition );

	}

	void GenerateLaserHit (Vector3 hitPosition){


		if (laserHitCoolDownCounter == 0) {
			LaserHit_Container.transform.GetChild (laserHitCounter).transform.position = hitPosition;
			LaserHit_Container.transform.GetChild (laserHitCounter).GetComponent<LaserHit>().LaserIsHit();
		}

		// adjust child counter
		laserHitCounter ++;
		if(laserHitCounter == LaserHit_Container.transform.childCount)
			laserHitCounter = 0;

		// coold down so it doesn't overpopulate
		laserHitCoolDownCounter ++;
		if (laserHitCoolDownCounter == laserHitCoolDown)
			laserHitCoolDownCounter = 0;
	}

	public void TakeDamage(float damage){
		health -= damage;
		if (health <= 0) {

			if(!VREnemy)
				Die();
		}
	}

	public void Die(){
		// TODO: optimize this
		GameObject.FindObjectOfType<ScoreManager> ().money += moneyValue;
		Destroy (gameObject);
	}
}
