using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	GameManager gm;

	float spawnCD = 2.0f;
	float spawnCDremaining = 0;

	[System.Serializable]
	public class WaveComponent {
		public GameObject enemyPrefab;
		public int num;
		[System.NonSerialized]
		public int spawned = 0;
	}

	public WaveComponent[] waveComps;

	// Use this for initialization
	void Start () {

		gm = GameObject.FindObjectOfType<GameManager> ();

		if (gm.isVR)
			gameObject.SetActive (false);
		
	}
	
	// Update is called once per frame
	void Update () {

		if (gm.isVR)
			return;
		
		spawnCDremaining -= Time.deltaTime;
		if (spawnCDremaining < 0) {
			spawnCDremaining = spawnCD;

			bool didSpawn = false;

			foreach (WaveComponent wc in waveComps) {
				if (wc.spawned < wc.num) {
					// Spawn
					Instantiate(wc.enemyPrefab, this.transform.position, this.transform.rotation);
					didSpawn = true;
					wc.spawned++;
					break;
				}
			}
				

			if (didSpawn == false && transform.parent.GetChild (1) != null) {

				// Activate the next spawner
				transform.parent.GetChild (1).gameObject.SetActive (true);
				// Destroy yourself
				Destroy (gameObject);
			}
		}


	}
}
