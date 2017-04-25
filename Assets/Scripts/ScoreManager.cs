using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {
	public Text scoreText;
	private int score = 0;

	void Update () {
		score += 1;
		scoreText.text = "Score: " + score;
	}
}
