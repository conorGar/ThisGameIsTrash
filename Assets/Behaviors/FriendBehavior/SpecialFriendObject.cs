using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpecialFriendObject : MonoBehaviour
{

	// Use this for initialization
	public RockFriend friend;

	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.tag == "Player") {
            friend.PickUpObject(this);
            GUIManager.Instance.rockItemHUD.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
	}


}

