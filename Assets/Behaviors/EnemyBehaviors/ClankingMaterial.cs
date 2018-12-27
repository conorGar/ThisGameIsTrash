using UnityEngine;
using System.Collections;

public class ClankingMaterial : MonoBehaviour
{
	//Rigidbody2D player;

	public AudioClip clankSound;
	public EnemyTakeDamage parentsETD;
	public bool pushBack = true; // is this enemy pushed when clanked with( usually yes, not with enemies that have armor during a dash or jump or something, like RhinoBeetle)
	//public Rigidbody2D parentEnemyBody;

	// Use this for initialization
	void Start ()
	{
		//player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
		if(clankSound == null){
			clankSound = SoundManager.instance.GetSFX(SFXBANK.CLANK);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.tag == "Weapon"){
			parentsETD.Clank(clankSound,gameObject.transform.position, pushBack);
		} 
	}




}

