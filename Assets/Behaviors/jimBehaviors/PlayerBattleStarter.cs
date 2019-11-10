using UnityEngine;
using System.Collections;

public class PlayerBattleStarter : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnCollisionEnter2D(Collision2D enemy){
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
			if (GetComponent<JimStateController>().IsHittable()) {
				if (enemy.gameObject.layer == 9) { //layer 9 = enemies
					BattleManager.Instance.AddHero(this.gameObject.GetComponent<HeroAttacker>()); //add jim to battle
					if(GlobalVariableManager.Instance.partners.Count > 0){
						foreach(HeroAttacker hero in GlobalVariableManager.Instance.partners){
							hero.gameObject.SetActive(true);
							BattleManager.Instance.AddHero(hero);
						}
					}
					enemy.gameObject.GetComponent<EnemyBattleStarter>().LeapBack();
					BattleManager.Instance.StartBattle();
				}
			}
		}
	}
}

