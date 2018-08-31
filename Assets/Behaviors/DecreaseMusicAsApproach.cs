using UnityEngine;
using System.Collections;

public class DecreaseMusicAsApproach : MonoBehaviour
{

	public float distanceTillStartDecreasing;
	public float decreaseVolTo;
	public GameObject player;

	float currentMusicVol;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Vector2.Distance(gameObject.transform.position,player.transform.position) < distanceTillStartDecreasing){
			
		}
	}
}

