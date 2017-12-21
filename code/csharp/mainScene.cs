using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainScene : MonoBehaviour {
	UnityEngine.UI.Text mainText;
	GameObject inputPanel;
	UnityEngine.UI.Text mainInput;
	UnityEngine.UI.Button confirmBtn;
	CanvasGroup inputCanvas;
	GameObject mainCharObject;
	GameObject enemyCharObject;
	bool leaveGame = false;
	int i;
	// Use this for initialization
	void Start () {
		mainText = findObject<UnityEngine.UI.Text> ("MainText");
		mainInput = findObject<UnityEngine.UI.Text> ("MainInput");
		confirmBtn = findObject<UnityEngine.UI.Button> ("InputConfirm");
		inputCanvas = findObject<CanvasGroup> ("inputPanel");
		mainCharObject = GameObject.Find ("mainCharObject");
		enemyCharObject = GameObject.Find ("enemyCharObject");

		mainCharObject.SetActive (false);
		enemyCharObject.SetActive (false);

		hideShowInput (false);

		runGame ();
	}

	T findObject<T>(string objectName){
		GameObject target = GameObject.Find (objectName);
		return target.GetComponent<T> ();
	}

	
	// Update is called once per frame
	void Update () {
		bool isShowingInput = this.inputCanvas.blocksRaycasts;
		if (isShowingInput && Input.GetKeyDown("return")) {
			getInput ();
		}
	}

	void hideShowInput(bool isShow){
		if (!isShow) {
			this.inputCanvas.alpha = 0f;
			this.inputCanvas.blocksRaycasts = false;
		} else {
			this.inputCanvas.alpha = 1f;
			this.inputCanvas.blocksRaycasts = true;
		}
	}

	void testText(){
		i++;
		int period = i % 100;
		if (period <= 50) {
			setMainText("hello");
		} else {
			setMainText("world");
		}
	}

	void runGame(){
		setMainText("hello world! Please Enter your Name");
		hideShowInput (true);
		UnityEngine.Events.UnityAction action = getInput;
		confirmBtn.onClick.AddListener(action);
		while (!leaveGame) {
			leaveGame = true;
		}
	}

	void getInput(){
		string username = mainInput.text;
		string input = "Your character name is " + username;
		setMainText (input);
		hideShowInput (false);
		mainCharObject.SetActive (true);
		enemyCharObject.SetActive (true);
	}

	void setMainText(string text){
		mainText.text = text;
	}
}
