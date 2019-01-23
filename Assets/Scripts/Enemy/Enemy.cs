using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    //public bool IsArmored = false;

    public int health;
    public int attkPower;

    public bool moveWhenHit;
    public bool respawningEnemyType;
    public bool toxicEnemy;

    int daysDead = 0;

	// Use this for initialization
	void Start () {
		if(toxicEnemy && GlobalVariableManager.Instance.IsPinEquipped(PIN.IRRADIATED)){
			attkPower--;
		}
	}
	void OnEnable(){
		if(toxicEnemy && (GlobalVariableManager.Instance.TUT_POPUPS_SHOWN & GlobalVariableManager.TUTORIALPOPUPS.TOXICENEMIES) != GlobalVariableManager.TUTORIALPOPUPS.TOXICENEMIES){
			GUIManager.Instance.tutorialPopup.gameObject.SetActive(true);
			GameStateManager.Instance.PushState(typeof(DialogState));
			GUIManager.Instance.tutorialPopup.SetData("RadioactiveEnemy");
		}
	}

	// Update is called once per frame
	void Update () {
        if (GameStateManager.Instance.GetCurrentState() != typeof(GameplayState)) {
            if (gameObject.GetComponent<Rigidbody2D>()!=null){
				gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}
		}
	}
}
