using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_CinematicBars : MonoBehaviour {

	private RectTransform topBar, botBar;
	private float targetSize;
	private float changeSizeAmount;
	private bool isActive;


	void Awake(){
		GameObject go = new GameObject("topBar",typeof(Image));
		go.transform.SetParent(transform,false);
		go.GetComponent<Image>().color = Color.black;
		topBar = gameObject.GetComponent<RectTransform>();
		topBar.anchorMin = new Vector2(0,0);
		topBar.anchorMax = new Vector2(1,1);
		topBar.sizeDelta = new Vector2(0,300);

		go = new GameObject("botBar",typeof(Image));
		go.transform.SetParent(transform,false);
		go.GetComponent<Image>().color = Color.black;
		botBar = gameObject.GetComponent<RectTransform>();
		botBar.anchorMin = new Vector2(0,0);
		botBar.anchorMax = new Vector2(1,0);
		botBar.sizeDelta = new Vector2(0,300);
	}


	private void Update(){
		if(isActive){
			Vector2 sizeDelta = topBar.sizeDelta;
			sizeDelta.y += changeSizeAmount * Time.deltaTime;
			if(changeSizeAmount > 0){
				if(sizeDelta.y >= targetSize){
					sizeDelta.y = targetSize;
					isActive = false;
				}
			}else{
			
				if(sizeDelta.y <= targetSize){
					sizeDelta.y = targetSize;
					isActive = false;
				}
			
		}
			topBar.sizeDelta = sizeDelta;
			botBar.sizeDelta = sizeDelta;
		}
	}

	public void Show(float targetSize, float time){
		this.targetSize = targetSize;
		changeSizeAmount = (targetSize - topBar.sizeDelta.y)/time;
		isActive = true;
	}

	public void Hide(float time){
		targetSize = 0f;
		changeSizeAmount = (targetSize - topBar.sizeDelta.y)/time;
		isActive = true;
	}


}
