using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {


	float speed = 50f;
	public Transform target;

	public float health = 1f;

	public float damage = 1f;
	public float radius = 0f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// Check if enemy exists and if not just return here!!
		if (target == null) {
			Destroy (gameObject);
			return;
		}
			
		// Finding the direction to the next node
		Vector3 dir = target.position - this.transform.localPosition;

		float distThisFrame = speed * Time.deltaTime;

		if (dir.magnitude <= distThisFrame) {
			// We reached the target node
			DoBulletHit();
		} else {
			// Move towards the target node
			transform.Translate(dir.normalized * distThisFrame, Space.World);

			// Look towards the target node
			// Instantaneours Change
			Quaternion.LookRotation (dir);
			// slow change
			Quaternion targetRotation = Quaternion.LookRotation (dir);
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, targetRotation, Time.deltaTime * 5);

		}
	}

	void DoBulletHit(){

		// All enemies within radius will die.
		if (radius == 0) {
			target.GetComponent<Enemy> ().TakeDamage (damage);
		} else {
			Collider[] cols = Physics.OverlapSphere (transform.position, radius);

			foreach (Collider c in cols) {
				Enemy e = c.GetComponent<Enemy> ();
				if (e != null) {
					e.TakeDamage (damage);
				}
			}
		}

		Destroy (gameObject);
	}
}
