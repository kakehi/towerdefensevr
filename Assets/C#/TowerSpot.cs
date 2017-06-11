using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpot : MonoBehaviour {

	GameManager gm;
	BuildingManager bm;
	ScoreManager sm;

	GameObject towerObject;
		
	public bool automated = false;


	float appearCounter = 0f;
	float appearCount = 2.0f;

	float disappearCounter = 0f;
	float disappearCount = 10.0f;

	bool towerAppeared = false;



	void Start(){

		gm = GameObject.FindObjectOfType<GameManager> ();
		bm = GameObject.FindObjectOfType<BuildingManager> ();
		sm = GameObject.FindObjectOfType<ScoreManager> ();

		// Make this gameobject deactivated when VR is on
		if (!gm.isVR) {
			return;
		}

		// only if the auto populate is turned on, populate the tower in the beginning.
		if (gm.autoPopulateTowers) {
			towerObject = Instantiate (bm.selectedTower, transform.parent.position-new Vector3(0.0f, 15.0f, 0.0f), transform.parent.rotation) as GameObject;
			towerObject.transform.parent = transform.parent;
			towerObject.SetActive(false);
		}


		appearCount = Random.Range(3.0f, 7.0f);

		disappearCount = Random.Range(8.0f, 13.0f);
	}




	void OnMouseUp(){


		if (bm.selectedTower != null && !gm.autoPopulateTowers) {

			if (sm.money < bm.selectedTower.GetComponent<Tower> ().cost) {
				Debug.Log ("Not enough money!");
				return;
			}

			sm.money -= bm.selectedTower.GetComponent<Tower> ().cost;
		

			Instantiate (bm.selectedTower, transform.parent.position, transform.parent.rotation);
			Destroy (transform.parent.gameObject);
		}
	}




	void Update(){

		// Make sure tower object still exists and also tower object is not dead.
		if (towerObject != null/* && towerObject.GetComponent<Tower>().dead*/) {
			
			if (appearCounter >= appearCount) {
				appearCounter = 0;
				appearCount = Random.Range (3.0f, 7.0f);
				towerAppeared = true;
				towerObject.SetActive (true);
				towerObject.GetComponent<Tower> ().GetReadyToShoot ();
			}

			if (disappearCounter >= disappearCount) {
				disappearCounter = 0;
				disappearCount = Random.Range (8.0f, 13.0f);
				towerAppeared = false;
				//towerObject.SetActive(false);
				towerObject.GetComponent<Tower> ().BackToHouse ();
			}


			// Counting down to manage appear/disappear
			if (!towerAppeared)
				appearCounter += Time.deltaTime;

			if (towerAppeared)
				disappearCounter += Time.deltaTime;

		}

	}
}
