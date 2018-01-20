using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ComponentSearchableGameObject : MonoBehaviour {
	protected string[] attrContainers;
	protected Dictionary<string, string> parentAttr;

	public T findObject<T>(string objectName){
		GameObject target = GameObject.Find(objectName);
		return target.GetComponent<T> ();
	}

	public T getAttr<T>(string attribute){
		T res = (T)(this.GetType ().GetField (attribute).GetValue (this));
		return res;
	}

	public void setAttr<T>(string attribute,T val){
		System.Reflection.FieldInfo info = this.GetType ().GetField (attribute,
			System.Reflection.BindingFlags.Public |
			System.Reflection.BindingFlags.NonPublic |
			System.Reflection.BindingFlags.Instance );
			info.SetValue (this, val);
			string attrCon = attribute + "Con";
		print (attrContainers);
			if(attrContainers.Contains(attribute)){
				GameObject container = (GameObject)(this.GetType ().GetField (attrCon).GetValue (this));
				UnityEngine.UI.Text containerText = container.GetComponent<UnityEngine.UI.Text> ();
				containerText.text = val.ToString ();
			}

			if(parentAttr.ContainsKey(attribute)){
				string related = parentAttr[attribute];
				setAttr<T>(related, val);
			}
	}
}

public class mainScene : ComponentSearchableGameObject {
	UnityEngine.UI.Text mainText;
	GameObject inputPanel;
	UnityEngine.UI.Text mainInput;
	UnityEngine.UI.Button confirmBtn;
	CanvasGroup inputCanvas;
	GameObject mainCharObject;
	mainChar mainCharProp;
	GameObject enemyCharObject;
	enemyChar enemyCharProp;
	bool leaveGame = false;
	bool inputConfirmed = false;
	int i;
	string name;
	string career;
	private IEnumerator coroutine;
	// Use this for initialization
	void Start () {
		name = "";
		career = "";

		mainText = findObject<UnityEngine.UI.Text> ("MainText");
		mainInput = findObject<UnityEngine.UI.Text> ("MainInput");
		confirmBtn = findObject<UnityEngine.UI.Button> ("InputConfirm");
		inputCanvas = findObject<CanvasGroup> ("inputPanel");
		mainCharObject = GameObject.Find ("mainCharObject");
		enemyCharObject = GameObject.Find ("enemyCharObject");

		mainCharObject.SetActive (false);
		enemyCharObject.SetActive (false);

		hideShowInput (false);
		coroutine = WaitForInput ();

		StartCoroutine (coroutine);

	}
	
	// Update is called once per frame
	void Update () {
		/*
		bool isShowingInput = this.inputCanvas.blocksRaycasts;
		if (isShowingInput && Input.GetKeyDown("return")) {
			getInput ();
		}
		*/
	}

	IEnumerator testText(){
		while (true) {
			i = (i >= 10) ? 0 : ++i;
			int period = i % 10;
			print (i);
			if (period <= 5) {
				print ("hello");
			} else {
				print ("world");
			}
			yield return new WaitForSeconds (0.5f);
		}
	}

	/*
	 *	Listener function for confirm input button 
	 */
	public void getInput(){
		if (name == "") {
			name = mainInput.text;
		} else if (career == "") {
			career = mainInput.text;
		}
	}

	private IEnumerator WaitForInput(){
		//bool condition = !(Input.GetKeyDown (KeyCode.Return) || inputConfirmed);
		while (name == "" || career == "") {
			hideShowInput (true);
			if (name == "") {
				setMainText ("your name is: ");
			} else {
				setMainText ("your career is: ");
			}
			yield return new WaitForSeconds (0.5f);
		}
		runGame ();
	}

	/*
	 * Set text of main panel
	 */
	void setMainText(string text){
		mainText.text = text;
	}

	/*
	 * Hide/Show the input text panel
	 */
	void hideShowInput(bool isShow){
		if (!isShow) {
			this.inputCanvas.alpha = 0f;
			this.inputCanvas.blocksRaycasts = false;
		} else {
			this.inputCanvas.alpha = 1f;
			this.inputCanvas.blocksRaycasts = true;
		}
	}

	void runGame(){
		StopCoroutine (coroutine);
		hideShowInput (false);
		mainCharObject.SetActive (true);
		enemyCharObject.SetActive (true);
		mainCharProp = new mainChar ();
		enemyCharProp = new enemyChar ();
		mainCharProp = mainCharObject.GetComponent<mainChar> ();
		mainCharProp.setChar(this.name, 50, 50, 10, 10, 10, 1);
		enemyCharProp = enemyCharObject.GetComponent<enemyChar> ();
		enemyCharProp.randChar (1);

		/*
		while (!leaveGame) {
			leaveGame = true;
		}
		*/
	}
}
