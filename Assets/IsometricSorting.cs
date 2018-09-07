using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricSorting : MonoBehaviour {
	//public int frontOrderInLayer = 4;
	//public int backOrderInLayer = 0;
	public bool hastk2dSprite = true;
	public bool movingObject;
	public bool legs;
	GameObject player;
	//public float yPositionMark;
	float myY;
	tk2dSprite mytk2dSprite;
	SpriteRenderer mySprite;


	// Use this for initialization
	void Start () {
		//player = GameObject.FindGameObjectWithTag("Player");
		//myY = gameObject.transform.position.y + yPositionMark;
		if(hastk2dSprite){
		mytk2dSprite = gameObject.GetComponent<tk2dSprite>();
		}else{
		mySprite = gameObject.GetComponent<SpriteRenderer>();
		}
			if(!movingObject){
				if(hastk2dSprite){
			
				mytk2dSprite.SortingOrder = -1*(int)gameObject.transform.position.y;

			}else{
				
				mySprite.sortingOrder = -1*(int)gameObject.transform.position.y;

			}
		}

	}

	// Update is called once per frame
	void Update () {
		if(movingObject){
			if(hastk2dSprite){
				//if(myY > player.transform.position.y && mytk2dSprite.SortingOrder != backOrderInLayer){
				//	mytk2dSprite.SortingOrder = backOrderInLayer;
				//}else if(myY < player.transform.position.y && mytk2dSprite.SortingOrder != frontOrderInLayer){
				//	mytk2dSprite.SortingOrder = frontOrderInLayer;

				//}
				if(!legs){
				mytk2dSprite.SortingOrder = -1*(int)gameObject.transform.position.y;
				}else{
					mytk2dSprite.SortingOrder = -1*(int)gameObject.transform.position.y-2;
				}

			}else{
				/*if(myY > player.transform.position.y && mySprite.sortingOrder != backOrderInLayer){
					mySprite.sortingOrder = backOrderInLayer;
				}else if(myY < player.transform.position.y && mySprite.sortingOrder != frontOrderInLayer){
					mySprite.sortingOrder = frontOrderInLayer;

				}*/
				mySprite.sortingOrder = -1*(int)gameObject.transform.position.y;

			}
		}
	}
}
