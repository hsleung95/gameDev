using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameChar {

	public static string[] attrStr = {"hp","mp","attack","defense","intelligence"};
	public static int wearingNum = 8;
	public static int ownedNum = 10;
	public static int attrNum = 5;
	public static int randCount;
	public bool defensed;

	protected int lv;
	protected float maxHP, currentHP;
	protected float maxMP, currentMP;
	protected float attack, attVal, attMod;	int decreasedAttRound; bool decreasedAtt;
	protected float defense, defVal, defMod; int defenseRound, decreasedDefRound;	bool decreasedDef;	//	value for defense action
	protected float intelligence, intVal, intMod; int decreasedIntRound;	bool decreasedInt;
	string charName;

	public gameChar(){
		currentHP=maxHP=currentMP=maxMP=attack=attVal=defense=defVal=intelligence=intVal=attMod=defMod=intMod=0;
		lv = 1;
		decreasedAttRound=decreasedDefRound=decreasedIntRound=defenseRound = -1;
		decreasedAtt=decreasedDef=decreasedInt=false;
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

	public float getMaxHP(){return maxHP;}
	public float getMaxMP(){return maxMP;}
	public float getCurrentHP(){return currentHP;}
	public float getCurrentMP(){return currentMP;}
	public float getAttack(){return attack;}
	public float getDefense(){return defense;}
	public float getIntelligence(){return intelligence;}
	public float getAttVal(){return attVal;}
	public float getDefVal(){return defVal;}
	public float getIntVal(){return intVal;}
	public float getAttMod(){return attMod;}
	public float getDefMod(){return defMod;}
	public float getIntMod(){return intMod;}
	public string getName(){return charName;}
	public virtual string getType(){return "gameChar";}
	public int getLV(){ return lv; }

	public int getRandCount(){		//counter for monster generated
		return (randCount++);
	}
	public void setMaxHP(float val){maxHP = val;}
	public void setCurrentHP(float val){currentHP = val;}
	public void setMaxMP(float val){maxMP = val;}
	public void setCurrentMP(float val){currentMP = val;}
	public void setAttack(float val){
		attack=val;
		attVal = attack;
	}
	public void setDefense(float val){
		defense=val;
		defVal = defense;
	}
	public void setIntelligence(float val){
		intelligence=val;
		intVal = intelligence;
	}
	public void setAttVal(float val){attVal=val;}
	public void setDefVal(float val){defVal=val;}
	public void setIntVal(float val){intVal=val;}
	public void setAttMod(float val){attMod=val;}
	public void setDefMod(float val){defMod=val;}
	public void setIntMod(float val){intMod=val;}
	public void setName(string name){charName=name;}
	public void setChar(string name, float charHP, float charMP, float charAtt, float charDef,float charInt, int charLv){
		charName = name;
		maxHP=charHP;
		maxMP=charMP;
		attack = charAtt;
		attVal = charAtt;
		defense = charDef;
		defVal = charDef;
		intelligence = charInt;
		intVal = charInt;
		currentHP = maxHP;
		currentMP = maxMP;
		lv = charLv;
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
		return (min * lv) + rand() %  (max * lv);
		*/
		int minVal = min * lv;
		int maxVal = max * lv;
		return Random.Range (minVal, maxVal);
	}

	public void randChar(int charLv){	//set char stat with random value
		if(charLv <= 0) charLv = 1;
		lv = charLv;
		maxHP= randValWithLV(10, 20, lv);   //val = min + rand  % max
		maxMP= randValWithLV(5, 50, lv);
		attack=randValWithLV(5, 10, lv);
		attVal = attack;
		attMod = 0;
		defense =randValWithLV(5, 10, lv);
		defVal = defense;
		defMod = 0;
		intelligence = randValWithLV(5, 10, lv);
		intVal = intelligence;
		intMod = 0;
		currentHP = maxHP;
		currentMP = maxMP;
	}

	public float attackChar(gameChar target){	//attack function
		float ownAttack = getAttVal() + attMod;
		float targetDefense = target.getDefVal() + target.getDefMod();
		float damage = (ownAttack  - targetDefense) * 1.5f ;
		if(damage<=0) damage = 1;
		return damage;
	}

	public bool defenseAction(int currentRound){
		defenseRound = currentRound;
		defVal = getDefense() * 2;
		defensed = true;
		return true;
	}

	public bool stopDefense(){
		defVal = getDefense();
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
