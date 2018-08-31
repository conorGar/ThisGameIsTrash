using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GUI_RockItemHUD : MonoBehaviour
{
	public List<Image> itemsToCollect = new List<Image>();
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void UpdateItemsCollected(Sprite spr){
		for(int i = 0; i< itemsToCollect.Count;i++){
			if(itemsToCollect[i].sprite = spr){
				itemsToCollect[i].color = Color.white;
				break;
			}
		}
	}
}

