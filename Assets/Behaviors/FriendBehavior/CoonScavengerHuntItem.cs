using UnityEngine;
using System.Collections;

public class CoonScavengerHuntItem : MonoBehaviour
{

	public int whichOne;
	Friend friend;

	void Start ()
	{
		friend = FriendManager.Instance.GetFriend("Coonelius");
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

