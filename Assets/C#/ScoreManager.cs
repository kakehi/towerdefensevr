using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public int lives = 20;
	public int money = 100;

	public Text moneyText;
	public Text livesText;

	public void LoseLife(int l = 1){
		lives -= 1;

		if (lives <= 0){
			GameOver();
		}
	}

	public void GameOver(){

		// TODO: think what to do for the gameover;
		Debug.Log ("Game Over");
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}


	void Update(){
		// TODO: Remove this from 
		moneyText.text = "Money: $" + money.ToString();
		livesText.text = "Lives: " + lives.ToString();

	}
}
