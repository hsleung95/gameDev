using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameChar : ComponentSearchableGameObject {

	public static string[] attrStr = {"hp","mp","attack","defense","intelligence"};
	public static int wearingNum = 8;
	public static int ownedNum = 10;
	public static int attrNum = 5;
	public static int randCount;
	public bool defensed;
	public string charName;

	protected int lv;
	protected float maxHP, currentHP;
	protected float maxMP, currentMP;
	protected float attack, attVal, attMod;	int decreasedAttRound; bool decreasedAtt;
	protected float defense, defVal, defMod; int defenseRound, decreasedDefRound;	bool decreasedDef;	//	value for defense action
	protected float intelligence, intVal, intMod; int decreasedIntRound;	bool decreasedInt;

	public GameObject charNameCon, currentHPCon, currentMPCon, attValCon, defValCon, intValCon;

	public gameChar(){
		charName = "";
		currentHP=maxHP=currentMP=maxMP=attack=attVal=defense=defVal=intelligence=intVal=attMod=defMod=intMod=0;
		lv = 1;
		decreasedAttRound=decreasedDefRound=decreasedIntRound=defenseRound = -1;
		decreasedAtt=decreasedDef=decreasedInt=false;
		base.attrContainers = new string[6] {"charNameCon", "currentHPCon", "currentMPCon","attValCon", "defValCon", "intValCon"};
		base.parentAttr = new Dictionary<string, string> ();
		base.parentAttr ["attack"] = "attVal";
		base.parentAttr ["defense"] = "defVal";
		base.parentAttr ["intelligence"] = "intVal";
	}

	public gameChar(string name, float hpVal, float mpVal, float att, float def,  float inti){
		defenseRound = -1;
		maxHP = currentHP = hpVal;
		maxMP = currentMP = mpVal;
		attack=attVal=att;
		defense=defVal=def;
		intelligence=intVal=inti;
		charName = name;
		lv = 1;
		attMod=defMod=intMod=0;
	}

	public virtual string getType(){return "gameChar";}

	public int getRandCount(){		//counter for monster generated
		return (randCount++);
	}
		
	public void setChar(string name, float charHP, float charMP, float charAtt, float charDef,float charInt, int charLv){
		setAttr<string> ("charName", name);
		setAttr<float> ("maxHP", charHP);
		setAttr<float> ("maxMP", charMP);
		setAttr<float> ("attack", charAtt);
		setAttr<float> ("defense", charDef);
		setAttr<float> ("intelligence", charInt);
		setAttr<float> ("currentHP", charHP);
		setAttr<float> ("currentMP", charMP);
		setAttr<int> ("lv", charLv);
		attMod = defMod = intMod = 0;
	}

	public virtual void printStat(){
		/*
		cout << "name: " << charName;
		cout << " lv: " << lv;
		cout << " HP: " << (currentHP <= 0? 0 : currentHP) << "/" << maxHP;
		cout << " MP: " << (currentMP <= 0? 0 : currentMP) << "/" << maxMP;
		cout << " Attack: " << attVal + attMod;
		cout << " Defense: " << defVal + defMod;
		cout << " Intelligence: " << intVal + intMod;
		*/
	}

	public float randValWithLV(int min, int max,int lv){	//function to rand stat
		/*
		srand((int)time(NULL) * (int)time(NULL));
		for(int i=0;i<10;i++) rand();
		return (min * lv) + rand() %  (max * lv);f
		*/
		int minVal = min * lv;
		int maxVal = max * lv;
		return Random.Range (minVal, maxVal);
	}

	public void randChar(int charLv){	//set char stat with random value
		if (charLv <= 0) {
			charLv = 1;
		}
		setAttr<int> ("lv", charLv);
		float maxHP = randValWithLV (10, 20, lv);
		float maxMP = randValWithLV (5, 50, lv);
		setAttr<float> ("maxHP", maxHP);
		setAttr<float> ("currentHP", maxHP);
		setAttr<float> ("maxMP", maxMP);
		setAttr<float> ("currentMP", maxMP);
		setAttr<float> ("attack", randValWithLV(5, 10, lv));
		setAttr<float> ("attMod", 0);
		setAttr<float> ("defense", randValWithLV(5, 10, lv));
		setAttr<float> ("defMod", 0);
		setAttr<float> ("intelligence", randValWithLV(5, 10, lv));
		setAttr<float> ("intMod", 0);
	}

	public float attackChar(gameChar target){	//attack function
		float ownAttack = getAttr<float>("attVal") + attMod;
		float targetDefense = target.getAttr<float>("defVal") + target.getAttr<float>("defMod");
		float damage = (ownAttack  - targetDefense) * 1.5f ;
		if(damage<=0) damage = 1;
		return damage;
	}

	public bool defenseAction(int currentRound){
		defenseRound = currentRound;
		defVal = getAttr<float>("defense") * 2;
		defensed = true;
		return true;
	}

	public bool stopDefense(){
		defVal = getAttr<float>("defense");
		defensed = false;
		return true;
	}

	//function that set char's modifier, used by other char
	public void decreaseAttr(int attribute, float effectVal, int currentRound){
		switch (attribute) {
		case 2:{
				attMod = effectVal;
				decreasedAttRound = currentRound;
				decreasedAtt = true;
				break;
			}
		case 3:{
				defMod = effectVal;
				decreasedDefRound = currentRound;
				decreasedDef = true;
				break;
			}
		case 4:{
				intMod = effectVal;
				decreasedIntRound = currentRound;
				decreasedInt = true;
				break;
			}
		default:
			break;
		}
	}

	public void checkRoundStat(int currentRound){	//reset all modifier after 7 moves(including enemy and myself)
		if(decreasedAtt){
			if(decreasedAttRound + 7 == currentRound){
				attMod = 0;
				decreasedAtt = false;
			}
		}
		if(decreasedDef){
			if(decreasedDefRound + 7 == currentRound){
				defMod = 0;
				decreasedDef = false;
			}
		}
		if(decreasedInt){
			if(decreasedIntRound + 7 == currentRound){
				intMod = 0;
				decreasedInt = false;
			}
		}
	}

}
