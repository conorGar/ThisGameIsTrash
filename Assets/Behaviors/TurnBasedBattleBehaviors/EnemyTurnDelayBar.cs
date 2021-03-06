﻿using UnityEngine;
using System.Collections;

public class EnemyTurnDelayBar : TurnDelayBar
{

	HeroAttacker targetedHero;

	void Awake(){
		speed = gameObject.GetComponent<EnemyAttacker>().speed;
	}
	// Use this for initialization
	protected override void BarFilledEvent(){
		base.BarFilledEvent();
		int randomTarget = Random.Range(0, BattleManager.Instance.heroList.Count);
		targetedHero = BattleManager.Instance.heroList[randomTarget];

		BattleManager.Instance.EnemyAttack(targetedHero, this.gameObject.GetComponent<EnemyAttacker>(), gameObject.GetComponent<EnemyAttacker>().damageStr); //TODO: Choose random hero to attack?
	}


}

