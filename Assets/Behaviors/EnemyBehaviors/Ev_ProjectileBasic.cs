using UnityEngine;
using System.Collections;

public class Ev_ProjectileBasic : MonoBehaviour
{
	public float speedX;
	public float speedY;
	// Use this for initialization
	void Update ()
	{
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(speedX,speedY);
	}

}

