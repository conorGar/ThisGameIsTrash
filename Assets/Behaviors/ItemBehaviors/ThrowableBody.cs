using UnityEngine;
using System.Collections;

public class ThrowableBody : ThrowableObject
{
	public int bodyHP = 3;
	public GameObject fliesPS;

	[HideInInspector]
	public bool poisoned;

	string mySpawnerID;

	Vector2 impactForce = new Vector2(3f,5f);

	public override void PickUp(){
        physicalCollision.enabled = false;
        PlayerManager.Instance.player.GetComponent<PlayerTakeDamage>().currentlyCarriedObject = this.gameObject;
		base.PickUp();
	}

	public void Poison(){
		poisoned = true;
		gameObject.GetComponent<tk2dSprite>().color = new Color(.17f,1f,.25f);//green color to toxic 
		fliesPS.SetActive(true);
	}

	public void SetSpawnerID(string id){
		mySpawnerID = id;
	}

	public IEnumerator Impact(GameObject pointOfImpact){
		gameObject.GetComponent<BoxCollider2D>().enabled = false;
		gameObject.transform.position = pointOfImpact.transform.position;
		float currentDirection = gameObject.GetComponent<Rigidbody2D>().velocity.x;
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		ObjectPool.Instance.GetPooledObject("effect_thrownImpact",transform.position);
		if(currentDirection < 0)
			gameObject.GetComponent<Rigidbody2D>().AddForce(impactForce,ForceMode2D.Impulse);
		else
			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(impactForce.x*-1,impactForce.y),ForceMode2D.Impulse);
		yield return new WaitForSeconds(.2f);

	}

	public void TakeDamage(){
		bodyHP--;
		if(bodyHP <= 0){
			Death();
		}
	}

	public void Death(){
		Debug.Log("Body death() activated.......");
		GlobalVariableManager.Instance.BASIC_ENEMY_LIST[this.mySpawnerID].bodyDestroyed = true;
		myBody.gravityScale = 0f;
		myBody.velocity = new Vector2(0,0f);
		canThrow = false;
		myShadow.SetActive(false);
		gameObject.layer = 11; //switch to item layer.
		ObjectPool.Instance.GetPooledObject("effect_landingSmoke",transform.position);
		ObjectPool.Instance.GetPooledObject("effect_vanishStars",transform.position);



	
        beingThrown = false;
        myBody.gravityScale = 0f;
       	myBody.velocity = new Vector2(0, 0f);
        spinning = false;
        canThrow = false;

        if (myShadow != null) {
                        myShadow.transform.parent = this.gameObject.transform;
                        myShadow.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .75f);
                        myShadow.transform.rotation = Quaternion.identity;
						myShadow.GetComponent<Renderer>().sortingLayerName = "Layer01";
						myShadow.transform.localPosition = Vector2.zero;
        }

     
        gameObject.layer = 11; //switch to item layer.
        for (int i = 0; i < behaviorsToStop.Count; i++) {
            behaviorsToStop[i].enabled = true;
        }
        gameObject.transform.localScale = Vector2.one;
        gameObject.GetComponent<Renderer>().sortingLayerName = "Layer01";
        gameObject.GetComponent<IsometricSorting>().enabled = true;
        gameObject.GetComponent<CannotExitScene>().enabled = false;
        if (physicalCollision != null)
              physicalCollision.enabled = true;


		this.gameObject.SetActive(false);
	}

	public IEnumerator DeathImpact(GameObject pointOfImpact){
		impactForce = new Vector2(6f,8f);
		StartCoroutine("Impact",pointOfImpact);
		yield return new WaitForSeconds(.5f);
		impactForce = new Vector2(3f,5f);
		Death();
	}
}

