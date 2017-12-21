using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyChar : gameChar {

	protected float expContain;
	protected equipment eq;
	protected skill[] skillArr;

	void start(){
		randomContainedEq (1);
	}

	enemyChar(){
		expContain = maxHP * 0.5f + maxMP * 0.1f + attack * 4 + defense * 4;
		eq = new equipment();
	}

	public float getExpContain(){ return expContain; }
	void setExpContain(){ expContain = maxHP * 0.5f + maxMP * 0.1f + attack * 4 + defense * 4; }

	new void printStat(){
		/*
		std::cout << "Enemy character value: " << endl;
		gameChar::printStat();
		std::cout << " exp: " << expContain << endl;
		cout << endl;
		*/
	}

	void printEq(){
		//eq.printEq();
	}

	new string getType(){ return "enemyChar";}

	equipment getEquipment(){return eq;}

	void randChar(string enemyName,int lv){
		base.randChar(lv);
		expContain = maxHP * 0.5f + maxMP * 0.1f + attack * 4 + defense * 4;
		eq.randomEquipment(lv, this);
		setName(enemyName);
		setExpContain();
	}

	public void randomContainedEq(int lv){
		eq.randomEquipment (lv, this);
	}

}

