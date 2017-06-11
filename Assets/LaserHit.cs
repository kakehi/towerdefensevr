using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHit : MonoBehaviour {

	int disappearCounter = 0;

	public void LaserIsHit(){
		disappearCounter = 30;
		transform.gameObject.SetActive (true);
	}

	void Update(){

		if(disappearCounter > 0)
			disappearCounter--;

		if(disappearCounter == 0)
			transform.gameObject.SetActive (false);
	}

}
