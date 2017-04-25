using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager Instance { get; private set; }
	void Awake () {
		if (Instance != null && Instance != this) {
			Destroy (this.gameObject);
		} else {
			Instance = this;
		}

		DontDestroyOnLoad (gameObject);
	}

	public void Restart () {
		Time.timeScale = 0.2f;
		Invoke ("DoRestart", 0.5f);
	}

	void DoRestart() {
		Time.timeScale = 1f;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}
