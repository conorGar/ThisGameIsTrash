using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    //public bool IsArmored = false;

    public int health;
    public int attkPower;

    public bool moveWhenHit;
    public bool respawningEnemyType;

    int daysDead = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (gameObject.GetComponent<Rigidbody2D>()!=null){
				gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}
		}
	}
}
