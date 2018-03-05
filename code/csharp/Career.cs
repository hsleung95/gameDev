using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class career : mainChar
{
	protected skill restoration = new restore_health("restore health",6,2,'r',"Restore health based on intelligence");	//root skill of all class

	protected skill doubleAttack = new physical_skill("double attack", 5, 2, 'd', "attack dealing double damage");
	protected skill trippleAttack = new physical_skill("tripple attack", 10, 3, 't', "attack dealing tripple damage");

	protected skill piercing_ball = new magic("piercing ball",10,2,'m',"Damaging enemy using magic");

	protected skill heavyAttack = new physical_skill("heavy attack", 5, 4, 'h', "heavy attack dealing a lot of damage");

	protected skill decreaseDef = new attribute_modifier("decrease defense", 10, -10,'d', "decrease enemy defense",3);

	protected List<skillNode> skillTree;
	public career (){}

	public career (string name, float hpVal,float mpVal,float att,float def,float intl)
	{
		mainChar (name, hpVal, mpVal, att, def, intl);
		skillNode first = skillNode(1, restoration);
		skillTree.Add(first);
	}

	public bool learnSkill(){
		if(skillTree.Count > 0){
			skillNode first = skillTree.IndexOf(0);
			if(lv >= first.getAttr<int>("unlockLv")){
				skill learning = first.getAttr<skill>("containSkill");
				skillList.Add(learning);
				skillTree.RemoveAt(0);
				learnSkill();
				return true;
			}
		}
		return false;
	}

	public string getCareer(){ return "Career";}

	public bool addExp(float expAmount){
		if(base.addExp(expAmount)){
			lvUp();
			return true;
		}
		return false;
	}

	public bool lvUp(){return true;}
	public abstract void generateEq();
}

public class adventurer : career {
	public adventurer(string name, float hpVal,float mpVal,float att,float def,float intl){
		career (name, hpVal, mpVal, att, def, intl);
		generateEq();
		skillTree.Add(skillNode(1, doubleAttack));
		skillTree.Add(skillNode(1, trippleAttack));
		skillTree.Add(skillNode(1, piercing_ball));
	}

	public string getCareer(){return "adventurer";}

	public bool lvUp(){
		lv += 1;
		expCap += setExpCap(lv);
		maxHP += 10;
		currentHP += 10;
		maxMP +=5;
		currentMP += 5;
		attack += 3;
		defense += 3;
		intelligence += 3;
		expCap -= 50*lv*lv;
		if(expCap < exp){
			expCap= exp + 100;
		}
		learnSkill();
		return true;
	}
	public void generateEq(){
		for(int i=0;i<wearingNum;i++){		//for each wearing places in character
			equipment.eqType eqtype = (equipment.eqType)(i);		//get type of the place
			equipment.attribute attr = (equipment.attribute)(Random.Range(0,attrNum));	//random attribute(hp/mp/att/def/intl)
			Pair<equipment.attribute, float> attrPair;
			attrPair[0] = Pair<equipment.attribute, float>(attr,5);
			equipment temp = equipment(("adventurer " + equipment.eqTypeStr[i]),*this, "First equipments of player",lv,eqtype,1,attrPair);		//create new equipment
			changeEquipment(temp);		//equip the new equipment to character
		}

		for(int i=0;i<ownedNum;i++){	//set owned equipment as empty equipment
			owned[i] = equipment();
		}
	}
}

public class magician : career {
	public magician(string name, float hpVal,float mpVal,float att,float def,float intl){
		career (name, hpVal, mpVal, att, def, intl);
		generateEq();
		skillTree.Add(skillNode(1, doubleAttack));
		skillTree.Add(skillNode(1, trippleAttack));
		skillTree.Add(skillNode(1, piercing_ball));
	}

	public string getCareer(){return "magician";}

	public bool lvUp(){
		lv += 1;
		expCap += setExpCap(lv);
		maxHP += 5;
		currentHP += 5;
		maxMP += 10;
		currentMP += 10;
		attack += 2;
		defense += 2;
		intelligence += 5;
		expCap += setExpCap (lv);
		learnSkill();
		return true;
	}
	public void generateEq(){
		for(int i=0;i<wearingNum;i++){		//for each wearing places in character
			equipment.eqType eqtype = (equipment.eqType)(i);		//get type of the place
			int randAttrNum = Random.Range(0,attrNum);
			randAttrNum = (randAttrNum < 2 ? 1 : 4);
			equipment.attribute attr = (equipment.attribute)(randAttrNum);	//random attribute(hp/mp/att/def/intl)
			Pair<equipment.attribute, float> attrPair;
			attrPair[0] = Pair<equipment.attribute, float>(attr,5);
			equipment temp = equipment(("magician " + equipment.eqTypeStr[i]),*this, "First equipments of player",lv,eqtype,1,attrPair);		//create new equipment
			changeEquipment(temp);		//equip the new equipment to character
		}

		for(int i=0;i<ownedNum;i++){	//set owned equipment as empty equipment
			owned[i] = equipment();
		}
	}
}

public class fighter : career {
	public fighter(string name, float hpVal,float mpVal,float att,float def,float intl){
		career (name, hpVal, mpVal, att, def, intl);
		generateEq();
		skillTree.Add(skillNode(1, doubleAttack));
		skillTree.Add(skillNode(1, trippleAttack));
		skillTree.Add(skillNode(1, piercing_ball));
	}

	public string getCareer(){return "fighter";}

	public bool lvUp(){
		lv += 1;
		expCap += setExpCap(lv);
		maxHP += 15;
		currentHP += 15;
		maxMP += 5;
		currentMP += 5;
		attack += 5;
		defense += 5;
		intelligence += 1;
		expCap += setExpCap (lv);
		learnSkill();
		return true;
	}
	public void generateEq(){
		for(int i=0;i<wearingNum;i++){		//for each wearing places in character
			equipment.eqType eqtype = (equipment.eqType)(i);		//get type of the place
			int randAttrNum = Random.Range(0,attrNum);
			if (randAttrNum == 1)
				randAttrNum = 0;
			else if (randAttrNum == 4)
				randAttrNum = 2;
			equipment.attribute attr = (equipment.attribute)(randAttrNum);	//random attribute(hp/mp/att/def/intl)
			Pair<equipment.attribute, float> attrPair;
			attrPair[0] = Pair<equipment.attribute, float>(attr,5);
			equipment temp = equipment(("fighter " + equipment.eqTypeStr[i]),*this, "First equipments of player",lv,eqtype,1,attrPair);		//create new equipment
			changeEquipment(temp);		//equip the new equipment to character
		}

		for(int i=0;i<ownedNum;i++){	//set owned equipment as empty equipment
			owned[i] = equipment();
		}
	}
}

