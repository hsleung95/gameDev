﻿using System.Collections;
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
		GameObject mainTextObject = GameObject.Find ("MainText");
		mainText = mainTextObject.GetComponent<UnityEngine.UI.Text> ();

		GameObject mainInputObject = GameObject.Find ("MainInputText");
		mainInput = mainInputObject.GetComponent<UnityEngine.UI.Text> ();

		inputPanel = GameObject.Find ("inputPanel");

		GameObject btnConfirm = GameObject.Find ("InputConfirm");
		confirmBtn = btnConfirm.GetComponent<UnityEngine.UI.Button> ();

		inputCanvas = inputPanel.GetComponent<CanvasGroup> ();
		mainCharObject = GameObject.Find ("mainCharObject");
		enemyCharObject = GameObject.Find ("enemyCharObject");
		mainCharObject.SetActive (false);
		enemyCharObject.SetActive (false);
		hideShowInput (false);
		runGame ();
	}
	
	// Update is called once per frame
	void Update () {
		//testText ();
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
			mainText.text = "hello";
		} else {
			mainText.text = "world";
		}
	}

	void runGame(){
		mainText.text = "hello world! Please Enter your Name";
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
		setText (input);
		mainCharObject.SetActive (true);
		enemyCharObject.SetActive (true);
	}

	void setText(string text){
		mainText.text = text;
	}
}
