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

		GameObject quill2 = ObjectPool.Instance.GetPooledObject("projectile_quill",gameObject.transform.position);
		quill2.GetComponent<Rigidbody2D>().velocity = new Vector2(-5,0);
		quill1.transform.rotation = Quaternion.Euler(0,0,90);
		GameObject quill3 = ObjectPool.Instance.GetPooledObject("projectile_quill",gameObject.transform.position);
		quill3.GetComponent<Rigidbody2D>().velocity = new Vector2(0,5);
		quill1.transform.rotation = Quaternion.Euler(0,0,0);
		GameObject quill4 = ObjectPool.Instance.GetPooledObject("projectile_quill",gameObject.transform.position);
		quill4.GetComponent<Rigidbody2D>().velocity = new Vector2(0,-5);
		quill1.transform.rotation = Quaternion.Euler(0,0,180);

		yield return new WaitForSeconds(1f);

		gameObject.GetComponent<WanderWithinBounds>().GoAgain();

		yield return new WaitForSeconds(Random.Range(1.5f, 3f));

		StopCoroutine("FireQuills");
		StartCoroutine("FireQuills");

	}

}

