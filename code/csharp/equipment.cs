using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pair<T1, T2>{
	public T1 first;
	public T2 second;

	public Pair(){
		first = default(T1);
		second = default(T2);
	}

	public Pair(T1 tfirst, T2 tsecond){
		first = tfirst;
		second = tsecond;
	}

	public void setFirst(T1 val){
		first = val;
	}

	public void setSecond(T2 val){
		second = val;
	}

	public void setPair(T1 firstVal, T2 secondVal){
		first = firstVal;
		second = secondVal;
	}
}

public class equipment
{
	public enum eqType {head=0, shoulders=1, arms=2, body=3, legs=4, boots=5, leftHand=6, rightHand=7};	//8 types of eq
	public enum attribute {attack=0, hp=1, mp=2, defense=3, intelligence=4};	//5 atrributes
	public static string[] eqTypeStr = {"head","shoulders","arms","body","legs","boots","leftHand","rightHand"};

	protected eqType eqtype;
	protected string eqName;
	protected gameChar owner;
	protected string description;
	protected Pair<attribute, float>[] attributePair;
	protected int lv;
	protected int rank;

	void start(){
		//equipment ();
	}

	public equipment(){
		eqName = "";
		description = "";
		lv = 1;
		rank = 1;
		attributePair = new Pair<attribute, float>[4];
		for(int i=0;i<4;i++){
			//attributePair[i] =  new Pair<attribute, float>((attribute)(Random.Range(0,4)), (0));
			attribute attr = (attribute)0;
			attributePair[i] =  new Pair<attribute, float>();
			attributePair [i].setPair (attr, 0);
		}
	}

	public equipment(eqType equipType){
		eqtype = equipType;
		eqName = "";
		description = "";
		lv = 1;
		rank = 1;
		for(int i=0;i<4;i++){
			attributePair[i] =  new Pair<attribute, float>((attribute)0, 0);
		}
	}

	public equipment(string name, gameChar ownedBy, string eqDescription, int lvCap, eqType equipType, int eqRank, Pair<attribute, float>[] attrPair){
		eqtype = equipType;
		eqName = name;
		owner = ownedBy;
		description = eqDescription;
		lv = lvCap;
		if(eqRank > 4) eqRank = 4;
		rank = eqRank;
		for(int i=0;i<4;i++){
			attributePair[i] = attrPair[i];
		}
	}

	public void setEquipment(string name, gameChar ownedBy, string eqDescription, int lvCap, eqType equipType, int eqRank, Pair<attribute, float>[] attrPair){
		eqtype = equipType;
		eqName = name;
		owner = ownedBy;
		description = eqDescription;
		lv = lvCap;
		if(eqRank > 4) eqRank = 4;
		rank = eqRank;
		for(int i=0;i<4;i++){
			attributePair[i] = attrPair[i];
		}
	}

	public eqType getEqType(){ return eqtype;}
	public attribute getAttribute(int index){ return attributePair[index].first;}
	public string getEqName(){return eqName;}
	public gameChar getOwner(){return owner;}
	public string getDescription(){return description;}
	public float getEqVal(int index){return attributePair[index].second;}
	public int getLvCap(){return lv;}
	public int getRank(){return rank;}

	public void randomEquipment(int lvCap, gameChar ownedChar){		//random setter with known lv and owner
		eqName = "random equipment";
		description = "random equipment";
		eqtype = (eqType)(Random.Range(0,gameChar.wearingNum));
		owner = ownedChar;
		lv = lvCap;
		int randNum = Random.Range(0,100);		// rand a number and distribute percentage
		if(randNum < 20) rank = 1;
		else if(randNum < 60) rank = 2;
		else if(randNum < 90) rank = 3;
		else rank = 4;
		for(int i=0;i<rank;i++){
			attribute attr = (attribute)(Random.Range(0,5f));
			float attrVal = (Random.Range(1,10) * lv);
			attributePair[i] = new Pair<attribute, float>(attr, attrVal);
		}
	}

	public bool setOwner(gameChar ch){	//change owner
		owner = ch;
		return true;
	}

	public void printEq(){
		/*
		cout << "Equipment Type: " << eqTypeStr[eqType] << endl;
		cout << "Name: " << eqName << endl;
		cout << "Rank: " << rank << endl;
		cout << "Description: " << description << endl;
		//cout << "+" << effectingVal  << " " << gameChar::attrStr[(int)attribute] << endl;
		for(int i=0;i<rank;i++){
			cout << "+" << attributePair[i].second << " " << gameChar::attrStr[(int)attributePair[i].first] << endl;
		}
		cout << "can be equiped after lv" << lv << endl;
		*/
	}

	public bool equipChar(gameChar equiped){
		for(int i=0;i<rank;i++){		//for each rank, set the owner's attribute
			float effectingVal = attributePair[i].second;
			switch ((int)(attributePair[i].first)) {
			case 0:{
					equiped.setAttr<float>("maxHP", equiped.getAttr<float>("maxHP")+effectingVal);
					equiped.setAttr<float>("currentHP", equiped.getAttr<float>("currentHP")+effectingVal);
					break;
				}
			case 1:{
					equiped.setAttr<float>("maxMP", equiped.getAttr<float>("maxMP")+effectingVal);
					equiped.setAttr<float>("currentMP", equiped.getAttr<float>("currentMP")+effectingVal);
					break;
				}
			case 2:{
					equiped.setAttr<float>("attack", equiped.getAttr<float>("attack")+effectingVal);
					break;
				}
			case 3:{
					equiped.setAttr<float>("defense", equiped.getAttr<float>("defense")+effectingVal);
					break;
				}
			case 4:{
					equiped.setAttr<float>("intelligence", equiped.getAttr<float>("intelligence")+effectingVal);
					break;
				}

			default:{
					return false;
					break;
				}
			}
		}
		return true;
	}

	public bool unEquipChar(gameChar equiped){
		for(int i=0;i< rank;i++){		//for each rank, unset owner's attribute
			float effectingVal = attributePair[i].second;
			switch ((int)(attributePair[i].first)) {
			case 0:{
					equiped.setAttr<float>("maxHP", equiped.getAttr<float>("maxHP") - effectingVal);
					equiped.setAttr<float>("currentHP", equiped.getAttr<float>("currentHP") - effectingVal);
					break;
				}
			case 1:{
					equiped.setAttr<float>("maxMP", equiped.getAttr<float>("maxMP") - effectingVal);
					equiped.setAttr<float>("currentMP", equiped.getAttr<float>("currentMP") - effectingVal);
					break;
				}
			case 2:{
					equiped.setAttr<float>("attack", equiped.getAttr<float>("attack") - effectingVal);
					break;
				}
			case 3:{
					equiped.setAttr<float>("defense", equiped.getAttr<float>("defense") - effectingVal);
					break;
				}
			case 4:{
					equiped.setAttr<float>("intelligence", equiped.getAttr<float>("intelligence") - effectingVal);
					break;
				}

			default:{
					return false;
					break;
				}
			}
		}
		return true;
	}

	public bool isNull(){		//function to check if the equipment is empty
		if(eqName=="" && description == "" && lv == 1 && rank == 1) return true;
		return false;
	}

	public override bool Equals(object equip){
		equipment eq = (equipment)equip;
		return (this.eqName == eq.eqName &&
		this.eqtype == eq.eqtype &&
		this.owner == eq.owner &&
		this.lv == eq.lv &&
		this.rank == eq.rank &&
		this.attributePair == eq.attributePair &&
		this.description == eq.description);
	}

	public static bool operator != (equipment eq1, equipment eq2){
		return !eq1.Equals (eq2);
	}

	public static bool operator == (equipment eq1, equipment eq2){
		return eq1.Equals(eq2);
	}

	public override int GetHashCode(){
		return  0;
	}

}
