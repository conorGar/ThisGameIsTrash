using UnityEngine;
using System.Collections;

public class HeroAttacker : MonoBehaviour
{
	public int currentHP;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void TakeDamage(int damage){
		Debug.Log(gameObject.name + "Took" + damage + "damage!");
		currentHP -= damage;
		//TODO:Check for death
	}
}

