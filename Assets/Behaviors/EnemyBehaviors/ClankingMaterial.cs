using UnityEngine;
using System.Collections;

public class ClankingMaterial : MonoBehaviour
{
	//Rigidbody2D player;

	public AudioClip clankSound;
	public EnemyTakeDamage parentsETD;
	//public Rigidbody2D parentEnemyBody;

	// Use this for initialization
	void Start ()
	{
		//player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.tag == "Weapon"){
			parentsETD.Clank(clankSound,gameObject.transform.position);
		} 
	}




}

