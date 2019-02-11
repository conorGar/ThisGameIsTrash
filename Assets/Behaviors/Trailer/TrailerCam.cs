using UnityEngine;
using System.Collections;

public class TrailerCam : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
        if (PlayerManager.Instance.player != null)
            transform.position = new Vector3(PlayerManager.Instance.player.transform.position.x, PlayerManager.Instance.player.transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (PlayerManager.Instance.player != null)
            transform.position = new Vector3(PlayerManager.Instance.player.transform.position.x, PlayerManager.Instance.player.transform.position.y, transform.position.z);
	}
}

