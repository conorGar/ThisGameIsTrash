using UnityEngine;
using System.Collections;

public class CoonItemSpawner : MonoBehaviour
{
	public string myRiddleText;
	public AudioClip hitSound;
	GameObject myHiddenTrash;
	int hitOnceCheck;
	// Use this for initialization
	void Start ()
	{
	
	}
	void OnEnable(){
		hitOnceCheck = 0;
	}
	// Update is called once per frame
	void Update ()
	{
	
	}
	void OnTriggerEnter2D(Collider2D collision){
		Debug.Log("Trash can hit collision detected");
		if(collision.gameObject.tag == "Weapon" && hitOnceCheck == 0){
			ObjectPool.Instance.GetPooledObject("effect_SmokePuff",gameObject.transform.position);
			SoundManager.instance.PlaySingle(hitSound);
			myHiddenTrash.SetActive(true);
			hitOnceCheck = 1;
			}
		
	}

	public void SetItem(GameObject go){//given by CooneliusFriend.cs
		go.transform.position = gameObject.transform.position;
		myHiddenTrash = go;
	}

}

