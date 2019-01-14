using UnityEngine;
using System.Collections;

public class TrailerCam : MonoBehaviour
{
	public GameObject player;
	// Use this for initialization
	void Start ()
	{
		//player = GameObject.Find("Jim");
        if (player != null)
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (player != null)
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
	}
}

