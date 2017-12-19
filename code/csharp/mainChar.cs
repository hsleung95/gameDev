using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mainChar : gameChar
{
	protected float exp;
	protected float expCap;
	protected equipment[] wearing;
	protected equipment[] owned;
	protected List<skill> skillList;

	public float setExpCap(int lv){
		return (lv * lv)/2 + (125 * lv);
	}

	public mainChar(){
		exp = 0;
		expCap = setExpCap(1);
		wearing = new equipment[wearingNum];
		owned = new equipment[ownedNum];
	}

	public bool changeEquipment(equipment eq){
		if(lv < eq.getLvCap()){		//check if character has the lv to equip item
			return false;
		}
		for(int i=0;i<wearingNum;i++){
			if(wearing[i].isNull() || eq.getEqType() == wearing[i].getEqType()){	//check if the slot is empty(constructor) or same type in the slot
				equipment temp = wearing[i];
				wearing[i].unEquipChar(this);
				eq.equipChar(this);
				this.wearing[i] = eq;
				addEquipment(temp);
				return true;
			}
		}
		return false;
	}

	public mainChar(string name, float hpVal,float mpVal,float att,float def,float intl) : base(name,hpVal,mpVal,att,def, intl){
		exp = 0;
		expCap = setExpCap(lv);		// expCap = (x^2)/2 + 125x
		for(int i=0;i<wearingNum;i++){		//for each wearing places in character
			equipment.eqType eqtype = (equipment.eqType)(i);		//get type of the place
			equipment temp = new equipment(eqtype);		//create new equipment
			changeEquipment(temp);		//equip the new equipment to character
		}
	}

	~mainChar(){
		skillList.Clear();
	}

	public bool addExp(float expAmount){
		exp += expAmount;
		if(exp >= expCap){
			return true;
		}
		return false;
	}

	public bool lvUp(){
		return true;
	}

	public bool lvUp(int lv){
		return true;
	}

	public override void printStat(){
		/*
		cout << "Your character value: "<< endl;
		gameChar::printStat();
		std::cout << " LV: " << lv << " Current EXP: " << exp << "/" << expCap << std::endl << endl;
		*/
	}

	public override string getType(){ return "mainChar";}

	public bool addEquipment(equipment eq){
		for(int i=0;i<ownedNum;i++){	//loop through owned equipment array, put the equipment into empty slot or cannot add
			if(owned[i].isNull()){
				owned[i] = eq;
				return true;
			}
		}
		return false;
	}

	public bool dropEquipment(equipment eq){
		for(int i=0; i<wearingNum;i++){		//check if the equipment is worn
			if(wearing[i] == eq){
				wearing[i] = new equipment();
				return true;
			}
		}
		for(int i=0;i<ownedNum;i++){		//check if the equipment is owned
			if(owned[i] == eq){
				owned[i] = new equipment();
				return true;
			}
		}
		return false;						//not exist in character, return false
	}

	public equipment[] getWearingArr(){return wearing;}
	public equipment[] getOwnedArr(){return owned;}
	public List<skill> getSkillList(){return skillList;}

	public void printSkill(){
		foreach(skill skillNode in skillList){
			/*
			cout << "skill key: " << (it)->getKey();
			cout << ", skill name: " << (it)->getSkillName();
			cout << ", skill cost: " << (it)->getCost();
			cout << " , description: " << (it)->getDescription();
			cout << endl;
			*/
		}
	}

	bool checkMagicKey(char userInput){
		foreach(skill skillNode in skillList){
			if(userInput == skillNode.getKey()) return true;		//check if userInput match any skill key in user skill list
		}
		return false;
	}
}

