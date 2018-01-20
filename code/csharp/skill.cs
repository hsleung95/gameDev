using UnityEngine;
using System.Collections;

public class skill : ComponentSearchableGameObject
{
	public static int skillTypeNum = 3;
	//public static string skillTypeArr;

	protected string skillName;
	protected float cost;
	protected float effectVal;
	protected char key;
	protected string description;

	public skill(){

	}

	public skill(string name, float skillCost, float skillVal, char skillKey, string skillDes){
		skillName = name;
		cost = skillCost;
		effectVal = skillVal;
		key = skillKey;
		description = skillDes;
	}
		
	public virtual string getSkillClass(){return "skill";}

	public virtual float cast(gameChar source, gameChar target, int currentRound){
		return 0;
	}

	public void setTargetParam(gameChar target, int field, int effectVal){
		switch(field){
		case 0: target.setAttr<float>("currentHP", target.getAttr<float>("currentHP") + effectVal); break;
		case 1: target.setAttr<float>("currentMP", target.getAttr<float>("currentMP") + effectVal); break;
		case 2: target.setAttr<float>("attMod", target.getAttr<float>("attMod") + effectVal); break;
		case 3: target.setAttr<float>("defMod", target.getAttr<float>("defMod") + effectVal); break;
		case 4: target.setAttr<float>("intMod", target.getAttr<float>("intMod") + effectVal); break;
		}
	}

	public virtual void printEffect(gameChar source, gameChar target, float value){
		
	}
}

public class magic : skill{
	public magic(){}
	public magic(string name, float skillCost, float skillVal, char skillKey, string skillDes) : base(name,skillCost,skillVal,skillKey,skillDes){}
	public override float cast(gameChar source, gameChar target, int currentRound){
		float sourceMP = source.getAttr<float>("currentMP");
		if(sourceMP < cost) return -1;
		source.setAttr<float>("currentMP", sourceMP - cost);

		float effectingVal = 0;
		float magicDef = target.getAttr<float>("intVal") + target.getAttr<float>("intMod");
		effectingVal = effectVal * (source.getAttr<float>("intVal") + source.getAttr<float>("intMod"))* 1 + magicDef;
		//value = -(spellAmount * intelligence * amplifier - targetIntelligence)
		if(effectingVal <= 0 ) effectingVal = 1;	//if no damage, set damage to 1
		effectingVal = -(effectingVal);
		if(Mathf.Abs(effectingVal) >= target.getAttr<float>("currentHP")) effectingVal = -(target.getAttr<float>("currentHP"));
		//if damage > current health, set difference = currentHP (currentHP - currentHP = 0)
		base.setTargetParam(target, 0, (int)(effectingVal));
		return -(effectingVal);		//effectingVal must be <0 and negate it
	}
	public override void printEffect(gameChar source, gameChar target, float value){
		//cout << source.getName() << "has casted " << skillName << ", dealing " << value << " damage to " << target.getName() << endl;
	}
	public override string getSkillClass(){return "magic";}
}

public class physical_skill : skill{
	public physical_skill(){}
	public physical_skill(string name, float skillCost, float skillVal, char skillKey, string skillDes) : base(name,skillCost,skillVal,skillKey,skillDes){}
	public override float cast(gameChar source, gameChar target, int currentRound){
		float effectingVal = 0;
		int chance = Random.Range(0,100);
		if(chance < 40) return 0;

		effectingVal = effectVal * (source.getAttr<float>("attVal") + source.getAttr<float>("attMod")) - (target.getAttr<float>("defVal") + target.getAttr<float>("defMod") );
		//damage = val * att - def
		if(effectingVal <= 0) effectingVal = 1;	//if no damage, set damage to 1
		effectingVal = -(effectingVal);
		if(Mathf.Abs(effectingVal) >= target.getAttr<float>("currentHP")) effectingVal = -(target.getAttr<float>("currentHP"));
		//if damage > current health, set difference = currentHP (currentHP - currentHP = 0)
		setTargetParam(target, 0, (int)(effectingVal));
		return -(effectingVal);		//effectingVal must be <0 and negate it
	}
	public override void printEffect(gameChar source, gameChar target, float value){
		//cout << source.getName() << "has casted " << skillName << ", dealing " << value << " damage to " << target.getName() << endl;
	}
	public override string getSkillClass(){return "physical_skill";}
}

public class restore_health : skill{
	public restore_health(){}
	public restore_health(string name, float skillCost, float skillVal, char skillKey, string skillDes) : base(name,skillCost,skillVal,skillKey,skillDes){}
	public override float cast(gameChar source, gameChar target, int currentRound){
		float sourceMP = source.getAttr<float>("currentMP");
		if(sourceMP < cost) return -1;
		source.setAttr<float>("currentMP", sourceMP - cost);

		float effectingVal = 0;
		effectingVal = effectVal * (source.getAttr<float>("intVal") + source.getAttr<float>("intMod")) * 1;
		//value = spellAmount * intelligence * amplifier
		if((target.getAttr<float>("currentHP") + effectingVal) >= target.getAttr<float>("maxHP")) effectingVal = target.getAttr<float>("maxHP") - target.getAttr<float>("currentHP");
		//if amount + current health > max health, set difference = max hp - current hp
		setTargetParam(target, 0, (int)(effectingVal));
		return effectingVal;		//effectingVal must be <0 and negate it
	}
	public override void printEffect(gameChar source, gameChar target, float value){
		//cout << source.getName() << "has casted " << skillName << ", dealing " << value << " damage to " << target.getName() << endl;
	}
	public override string getSkillClass(){return "restore_health";}
}

public class attribute_modifier : skill{
	int field;

	public attribute_modifier(){}
	public attribute_modifier(string name, float skillCost, float skillVal, char skillKey, string skillDes, int skillField) : base(name,skillCost,skillVal,skillKey,skillDes){
		if(skillField >= 2 && skillField <= 4) field = skillField;
	}
	public override float cast(gameChar source, gameChar target, int currentRound){
		float sourceMP = source.getAttr<float>("currentMP");
		if(sourceMP < cost) return -1;
		source.setAttr<float>("currentMP", sourceMP - cost);

		float effectingVal = 0;
		effectingVal = effectVal * source.getAttr<float>("intVal") * 0.1f;
		target.decreaseAttr(field, effectingVal, currentRound);
		return effectingVal;

	}
	public override void printEffect(gameChar source, gameChar target, float value){
		//cout << source.getName() << "has casted " << skillName << ", dealing " << value << " damage to " << target.getName() << endl;
	}
	public override string getSkillClass(){return "attribute_modifier";}
}