using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {
	public GUISkin guiSkin;

	public GameState state;

	public int score;

	GUIText scoreTxt;
	GUIText scoreShadowTxt;

	// Use this for initialization
	void Start () {
		score = 0;
		scoreTxt = GameObject.Find("Score").GetComponent<GUIText>();
		scoreShadowTxt = GameObject.Find("Score Shadow").GetComponent<GUIText>();
		int highScore = PlayerPrefs.GetInt("highScore");
		GameObject.Find("High Score").GetComponent<GUIText>().text += highScore;
		GameObject.Find("High Score Shadow").GetComponent<GUIText>().text += highScore;
	}
	
	void OnGUI() {
		GUI.skin = guiSkin;
		int width = (int)(Screen.width / 5.5f);
		int height = Screen.height / 8;
		int x = (int)(Screen.width * 0.65f) - width / 2;
		int y = Screen.height / 2 - height / 2;
		
		switch (state) {
		case GameState.GameNew:
			if(GUI.Button(new Rect(x, y, width, height), "Start")) {
				if (state == GameState.GameNew) {
					state = GameState.GameRun;
				}
			}
			break;
		case GameState.GameRun:
			scoreTxt.text = score.ToString();
			scoreShadowTxt.text = score.ToString();
			break;
		default:
			break;
		}
	}

	void GameSaveResult() {
		int highScore = PlayerPrefs.GetInt("highScore");
		if (highScore < score) {
			PlayerPrefs.SetInt("highScore", score);
			string hsStr = "Best: " + score;
			GameObject.Find("High Score").GetComponent<GUIText>().text = hsStr;
			GameObject.Find("High Score Shadow").GetComponent<GUIText>().text = hsStr;
		}
		PlayerPrefs.Save();
	}

	// Update is called once per frame
	void Update () {
		switch (state) {
		case GameState.GameOver:
			GameSaveResult();
			GameObject.Find("Screen Fader").GetComponent<SceneFadeInOut>().EndScene();
			break;
		default:
			break;
		}
	}
}
