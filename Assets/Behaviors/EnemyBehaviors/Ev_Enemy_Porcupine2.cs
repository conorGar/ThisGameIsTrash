using UnityEngine;
using System.Collections;

public class Ev_Enemy_Porcupine2 : MonoBehaviour
{
	/// <summary>
	/// Wanders around, stops to fire quills, which fire in straight lines in north south east west directions.
	/// </summary>


	void OnEnable(){
		//Invoke("FireQuills",Random.Range(1.5f,3.5f));
		StartCoroutine("FireQuills");
	}

	IEnumerator FireQuills(){

		yield return new WaitForSeconds(Random.Range(1.5f, 3.5f));


		gameObject.GetComponent<WanderWithinBounds>().StopMoving();


		gameObject.GetComponent<tk2dSpriteAnimator>().Play("shake");

		yield return new WaitForSeconds(.5f);




		GameObject quill1 = ObjectPool.Instance.GetPooledObject("projectile_quill",gameObject.transform.position);
		quill1.GetComponent<Rigidbody2D>().velocity = new Vector2(5,0);
		quill1.transform.rotation = Quaternion.Euler(0,0,-90);
		quill1.GetComponent<Projectile>().myShadow.transform.localPosition = new Vector2(0,1.26f);

		GameObject quill2 = ObjectPool.Instance.GetPooledObject("projectile_quill",gameObject.transform.position);
		quill2.GetComponent<Rigidbody2D>().velocity = new Vector2(-5,0);
		quill2.transform.rotation = Quaternion.Euler(0,0,90);
		quill2.GetComponent<Projectile>().transform.localPosition = new Vector2(0,-.89f);

		GameObject quill3 = ObjectPool.Instance.GetPooledObject("projectile_quill",gameObject.transform.position);
		quill3.GetComponent<Rigidbody2D>().velocity = new Vector2(0,5);
		quill3.transform.rotation = Quaternion.Euler(0,0,0);
		quill3.GetComponent<Projectile>().transform.localPosition = new Vector2(1.66f,.27f);


		GameObject quill4 = ObjectPool.Instance.GetPooledObject("projectile_quill",gameObject.transform.position);
		quill4.GetComponent<Rigidbody2D>().velocity = new Vector2(0,-5);
		quill4.transform.rotation = Quaternion.Euler(0,0,180);
		quill4.GetComponent<Projectile>().transform.localPosition = new Vector2(-.52f,0f);

		yield return new WaitForSeconds(1f);

		gameObject.GetComponent<WanderWithinBounds>().GoAgain();

		yield return new WaitForSeconds(Random.Range(1.5f, 3f));

		StopCoroutine("FireQuills");
		StartCoroutine("FireQuills");

	}

}

